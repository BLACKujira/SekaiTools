using SekaiTools.UI.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    public abstract class GIP_ImageData : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public LoadFileSelectItem file_LoadData;
        public Text txt_DataInfo;
        [Header("Settings")]
        public Window downloaderPrefab;

        SerializedImageData serializedImageData;
        public SerializedImageData SerializedImageData => serializedImageData;
        public string SavePath => file_LoadData.SelectedPath;

        protected abstract IEnumerable<string> RequireImageKeys { get; }
        protected virtual string DefaultFileName => string.Empty;
        protected virtual Func<string, bool> NeedImageFile
        {
            get
            {
                HashSet<string> keys = new HashSet<string>(RequireImageKeys);
                return str => keys.Contains(str);
            }
        }
        protected virtual DownloadFileInfo[] RequireFileList
        {
            get
            {
                if (serializedImageData == null)
                {
                    WindowController.ShowLog(Message.Error.STR_ERROR, "需要先选择图像资料");
                }
                string assetFolder = Path.GetFullPath(EnvPath.Assets);
                IEnumerable<string> selectedFiles = serializedImageData.items
                    .Select(value => value.path)
                    .Where(path => Path.GetFullPath(path).StartsWith(assetFolder));
                List<DownloadFileInfo> downloadFileInfos
                    = new List<DownloadFileInfo>(
                        selectedFiles.Select(str =>
                        new DownloadFileInfo(ExtensionTools.GetUrlInSV(str), str)));
                return downloadFileInfos.ToArray();
            }
        }
        protected virtual string DownloadFileFolder { get; }

        public virtual void Initialize()
        {
            file_LoadData.onPathChange.AddListener(
                (path) =>
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        serializedImageData = null;
                    }
                    else
                    {
                        try
                        {
                            serializedImageData = SerializedImageData.LoadData(File.ReadAllText(path));
                        }
                        catch
                        {
                            serializedImageData = null;
                        }
                    }
                    RefreshInfo();
                });
        }

        public void RefreshInfo()
        {
            if (serializedImageData == null)
                txt_DataInfo.text = "请选择文件";
            else
            {
                MediaMatchInfo imageMatchInfo = serializedImageData.GetImageMatchInfo(RequireImageKeys);
                txt_DataInfo.text = $"在{imageMatchInfo.Total}个图像中，有{imageMatchInfo.matchcing}个匹配，有{imageMatchInfo.missingKey}个缺失，有{imageMatchInfo.missingFile}个文件丢失";
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (serializedImageData == null)
                errors.Add("未设置图像资料或文件损坏");
            return GenericInitializationCheck.GetErrorString("图像资料错误", errors);
        }

        public void CreateData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResultIter1 = folderBrowserDialog.ShowDialog();
            if (dialogResultIter1 != DialogResult.OK)
                return;

            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
            saveFileDialog.FileName = DefaultFileName;
            DialogResult dialogResultIter2 = saveFileDialog.ShowDialog();
            if (dialogResultIter2 != DialogResult.OK)
                return;

            CreateDataFrom(folderBrowserDialog.SelectedPath, saveFileDialog.FileName);
        }

        public void DownloadAndCreate()
        {
            Downloader.Downloader downloader
                = WindowController.windowController.currentWindow.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.existingFileProcessingMode = ExistingFileProcessingMode.Pass;
            settings.downloadFiles = RequireFileList;
            settings.disableLogView = true;

            downloader.Initialize(settings);
            downloader.OnComplete += () =>
            {
                if (downloader.HasError)
                {
                    downloader.EnableLogView();
                    WindowController.ShowMessage(Message.Error.STR_ERROR, "存在未能下载的文件，创建的音频资料不包括这些文件");
                }
                else
                {
                    downloader.window.Close();
                }
                SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
                saveFileDialog.FileName = DefaultFileName;
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                    return;

                CreateDataFrom(DownloadFileFolder, saveFileDialog.FileName);
            };
            downloader.StartDownload();
        }

        public void DownloadMissingFiles()
        {

            Downloader.Downloader downloader
                = WindowController.windowController.currentWindow.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.existingFileProcessingMode = ExistingFileProcessingMode.Pass;
            settings.downloadFiles = RequireFileList;
            settings.disableLogView = true;
            downloader.Initialize(settings);
            downloader.OnComplete += () =>
            {
                if (downloader.HasError)
                {
                    downloader.EnableLogView();
                    WindowController.ShowMessage(Message.Error.STR_ERROR, "存在未能下载的文件");
                }
                else
                {
                    downloader.window.Close();
                }
                RefreshInfo();
            };
            downloader.StartDownload();
        }

        protected virtual void CreateDataFrom(string folderPath, string savePath)
        {
            Dictionary<string, string> rawSerializedImageData = new Dictionary<string, string>();

            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (NeedImageFile(file))
                {
                    rawSerializedImageData[Path.GetFileNameWithoutExtension(file)] = file;
                }
            }

            SerializedImageData sad = new SerializedImageData(rawSerializedImageData);
            File.WriteAllText(savePath, JsonUtility.ToJson(sad));
            file_LoadData.SelectedPath = savePath;
            serializedImageData = sad;
            RefreshInfo();
        }
    }
}