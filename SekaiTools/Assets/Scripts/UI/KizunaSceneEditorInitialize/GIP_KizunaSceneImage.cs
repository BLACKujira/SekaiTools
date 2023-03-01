using SekaiTools.Kizuna;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{

    public class GIP_KizunaSceneImage : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public LoadFileSelectItem file_LoadData;
        public Text txt_DataInfo;
        [Header("Settings")]
        public Window downloaderPrefab;

        SerializedImageData serializedImageData;
        public SerializedImageData SerializedImageData => serializedImageData;

        KizunaSceneData kizunaSceneData;

        public void Initialize(KizunaSceneData kizunaSceneData)
        {
            this.kizunaSceneData = kizunaSceneData;
            string imdPath = Path.ChangeExtension(kizunaSceneData.SavePath, ".imd");
            if (File.Exists(imdPath))
            {
                file_LoadData.SelectedPath = imdPath;
                serializedImageData = SerializedImageData.LoadData(File.ReadAllText(imdPath));
            }
            RefreshInfo();
        }

        public void RefreshInfo()
        {
            if (serializedImageData == null)
                txt_DataInfo.text = "请选择文件";
            else
            {
                MediaMatchInfo matchInfo = serializedImageData.GetImageMatchInfo(
                    kizunaSceneData.kizunaScenes
                    .Select((kzn) => kzn.textSpriteLv1)
                    .Concat(
                        kizunaSceneData.kizunaScenes
                        .Select((kzn) => kzn.textSpriteLv2))
                    .Concat(
                        kizunaSceneData.kizunaScenes
                        .Select((kzn) => kzn.textSpriteLv3)));
                txt_DataInfo.text = matchInfo.Description;
            }
        }

        public void CreateData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResultIter1 = folderBrowserDialog.ShowDialog();
            if (dialogResultIter1 != DialogResult.OK)
                return;

            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_IMD);
            saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(kizunaSceneData.SavePath, ".imd"));
            DialogResult dialogResultIter2 = saveFileDialog.ShowDialog();
            if (dialogResultIter2 != DialogResult.OK)
                return;

            CreateDataFrom(folderBrowserDialog.SelectedPath, saveFileDialog.FileName);
        }

        public void DownloadAndCreate()
        {
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var kizunaScene in kizunaSceneData.kizunaScenes)
            {
                foreach (var image in new string[] { kizunaScene.textSpriteLv1, kizunaScene.textSpriteLv2, kizunaScene.textSpriteLv3 })
                {
                    string urlF = $"{SekaiViewer.AssetUrl}/bonds_honor/word/{image}_rip/{image}.png";
                    string savePathF = $"{EnvPath.AssetFolder}/assets/bonds_honor/word/{image}_rip/{image}.png";
                    downloadFileInfos.Add(new DownloadFileInfo(urlF, savePathF));
                }
            }
            Downloader.Downloader downloader
                = WindowController.windowController.currentWindow.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.existingFileProcessingMode = ExistingFileProcessingMode.Pass;
            settings.downloadFiles = downloadFileInfos.ToArray();
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
                SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_IMD);
                saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(kizunaSceneData.SavePath, ".imd"));
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                    return;

                CreateDataFrom($"{EnvPath.AssetFolder}/assets/bonds_honor/word", saveFileDialog.FileName);
            };
            downloader.StartDownload();
        }

        void CreateDataFrom(string folderPath, string savePath)
        {
            Dictionary<string, string> rawSerializedImageData = new Dictionary<string, string>();

            ScanFile_Classic(folderPath, rawSerializedImageData);
            ScanFile_SV(folderPath, rawSerializedImageData);

            SerializedImageData sid = new SerializedImageData(rawSerializedImageData);
            File.WriteAllText(savePath, JsonUtility.ToJson(sid));
            file_LoadData.SelectedPath = savePath;
            serializedImageData = sid;
            RefreshInfo();
        }

        void ScanFile_SV(string folderPath, Dictionary<string, string> rawSerializedImageData)
        {
            Dictionary<string, string[]> files = ExtensionTools.GetFilesInSubFolder(folderPath);
            foreach (var kizunaScene in kizunaSceneData.kizunaScenes)
            {
                foreach (var image in new string[] { kizunaScene.textSpriteLv1, kizunaScene.textSpriteLv2 , kizunaScene.textSpriteLv3 })
                {
                    foreach (var keyValuePair in files)
                    {
                        if (keyValuePair.Key.Equals(image) || keyValuePair.Key.Equals($"{image}_rip"))
                        {
                            foreach (var file in files[keyValuePair.Key])
                            {
                                if (file.StartsWith(image)
                                    && ExtensionTools.IsImageFile(file))
                                {
                                    rawSerializedImageData[image]
                                        = Path.Combine(folderPath, keyValuePair.Key, file);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        void ScanFile_Classic(string folderPath, Dictionary<string, string> rawSerializedAudioData)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                foreach (var kizunaScene in kizunaSceneData.kizunaScenes)
                {
                    foreach (var image in new string[] { kizunaScene.textSpriteLv1, kizunaScene.textSpriteLv2, kizunaScene.textSpriteLv3 })
                    {
                        if (fileName.StartsWith(image)&&ExtensionTools.IsImageFile(file))
                        {
                            rawSerializedAudioData[image] = file;
                        }
                    }
                }
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (serializedImageData == null)
                errors.Add("未设置图像资料或文件损坏");
            return GenericInitializationCheck.GetErrorString("图像资料错误", errors);
        }
    }
}