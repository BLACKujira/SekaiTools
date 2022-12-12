using SekaiTools.SekaiViewerInterface;
using SekaiTools.SystemLive2D;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.SysL2DShowEditorInitialize
{
    public class GIP_SysL2DShowAudio : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public LoadFileSelectItem file_LoadData;
        public Text txt_DataInfo;
        public Button[] activeButtons;
        [Header("Settings")]
        public Window downloaderPrefab;

        SerializedAudioData serializedAudioData;
        public SerializedAudioData SerializedAudioData => serializedAudioData;

        SysL2DShowData sysL2DShowData;

        public void Awake()
        {
            file_LoadData.defaultPath = string.Empty;
            file_LoadData.onPathReset += (_) =>
            {
                serializedAudioData = null;
                RefreshDataInfo();
            };
            file_LoadData.onPathSelect += (path) =>
            {
                try
                {
                    serializedAudioData = JsonUtility.FromJson<SerializedAudioData>(File.ReadAllText(path));
                }
                catch
                {
                    serializedAudioData = null;
                }
                RefreshDataInfo();
            };
            RefreshDataInfo();
        }

        public void SetData(SysL2DShowData sysL2DShowData = null)
        {
            this.sysL2DShowData = sysL2DShowData;
            file_LoadData.defaultPath = string.Empty;
            file_LoadData.ResetPath();

            if (sysL2DShowData != null)
            {
                string audPath = Path.ChangeExtension(sysL2DShowData.SavePath, ".aud");
                if (File.Exists(audPath))
                {
                    try
                    {
                        file_LoadData.SelectedPath = audPath;
                        serializedAudioData = JsonUtility.FromJson<SerializedAudioData>(File.ReadAllText(audPath));
                    }
                    catch
                    {
                        file_LoadData.ResetPath();
                        serializedAudioData = null;
                    }
                }
            }

            RefreshDataInfo();
        }

        public void CreateData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResultIter1 = folderBrowserDialog.ShowDialog();
            if (dialogResultIter1 != DialogResult.OK)
                return;

            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
            saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(sysL2DShowData.SavePath, ".aud"));
            DialogResult dialogResultIter2 = saveFileDialog.ShowDialog();
            if (dialogResultIter2 != DialogResult.OK)
                return;

            CreateDataFrom(folderBrowserDialog.SelectedPath, saveFileDialog.FileName);
        }

        void CreateDataFrom(string folderPath,string savePath)
        {
            Dictionary<string, string> rawSerializedAudioData = new Dictionary<string, string>();

            string[] directories = Directory.GetDirectories(folderPath).Select((dir)=>Path.GetFileName(dir)).ToArray();
            Dictionary<string, string[]> files = new Dictionary<string, string[]>();
            foreach (var folder in directories)
            {
                files[folder] = Directory.GetFiles(Path.Combine(folderPath, folder)).Select((file) => Path.GetFileName(file)).ToArray();
            }
            foreach (var sysL2DShow in sysL2DShowData.sysL2DShows)
            {
                foreach (var folder in directories)
                {
                    if(folder.Equals(sysL2DShow.systemLive2D.AssetbundleName) || folder.Equals($"{sysL2DShow.systemLive2D.AssetbundleName}_rip"))
                    {
                        foreach (var file in files[folder])
                        {
                            if (file.StartsWith(sysL2DShow.systemLive2D.Voice)
                                && ExtensionTools.IsAudioFile(file))
                            {
                                rawSerializedAudioData[$"{sysL2DShow.systemLive2D.AssetbundleName}-{sysL2DShow.systemLive2D.Voice}"]
                                    = Path.Combine(folderPath,folder,file);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            SerializedAudioData sad = new SerializedAudioData(rawSerializedAudioData);
            File.WriteAllText(savePath, JsonUtility.ToJson(sad));
            file_LoadData.SelectedPath = savePath;
            serializedAudioData = sad;
            RefreshDataInfo();
        }

        public void DownloadAndCreate()
        {
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var sysL2DShow in sysL2DShowData.sysL2DShows)
            {
                string url = $"{SekaiViewer.AssetUrl}/sound/system_live2d/voice/{sysL2DShow.systemLive2D.AssetbundleName}_rip/{sysL2DShow.systemLive2D.Voice}.mp3";
                string savePath = $"{EnvPath.AssetFolder}/assets/sound/system_live2d/voice/{sysL2DShow.systemLive2D.AssetbundleName}_rip/{sysL2DShow.systemLive2D.Voice}.mp3";
                downloadFileInfos.Add(new DownloadFileInfo(url, savePath));
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
                if(downloader.HasError)
                {
                    downloader.EnableLogView();
                    WindowController.ShowMessage(Message.Error.STR_ERROR, "存在未能下载的文件，创建的音频资料不包括这些文件");
                }
                else
                {
                    downloader.window.Close();
                }
                SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
                saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(sysL2DShowData.SavePath, ".aud"));
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                    return;

                CreateDataFrom($"{EnvPath.AssetFolder}/assets/sound/system_live2d/voice", saveFileDialog.FileName);
            };
            downloader.StartDownload();
        }

        void RefreshDataInfo()
        {
            if (sysL2DShowData == null)
            {
                foreach (var button in activeButtons)
                {
                    button.interactable = false;
                }
                txt_DataInfo.text = "请先读取或创建系统Live2D资料";
            }
            else
            {
                foreach (var button in activeButtons)
                {
                    button.interactable = true;
                }
                if (serializedAudioData == null)
                {
                    txt_DataInfo.text = "请读取音频资料";
                }
                else
                {
                    SysL2DShowData.AudioMatchInfo audioMatchInfo = sysL2DShowData.GetAudioMatchInfo(serializedAudioData);
                    txt_DataInfo.text = $"共{sysL2DShowData.sysL2DShows.Count}个片段中，{audioMatchInfo.matchingCount}个语音匹配，{audioMatchInfo.missingCount}个语音缺失";
                }
            }
        }

        public string CheckIfReady()
        {
            if (serializedAudioData == null)
                return GenericInitializationCheck.GetErrorString("语音错误", "没有设置语音");
            return null;
        }
    }
}