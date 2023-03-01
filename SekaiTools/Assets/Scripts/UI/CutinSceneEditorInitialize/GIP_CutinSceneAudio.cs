using SekaiTools.Cutin;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CutinSceneEditorInitialize
{
    public class GIP_CutinSceneAudio : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public LoadFileSelectItem file_LoadData;
        public Text txt_DataInfo;
        [Header("Settings")]
        public Window downloaderPrefab;

        SerializedAudioData serializedAudioData;
        public SerializedAudioData SerializedAudioData => serializedAudioData;

        CutinSceneData cutinSceneData;

        private void Awake()
        {
            file_LoadData.onPathChange.AddListener(
                (path) =>
                {
                    if(string.IsNullOrEmpty(path))
                    {
                        serializedAudioData = null;
                    }
                    else
                    {
                        try
                        {
                            serializedAudioData = SerializedAudioData.LoadData(File.ReadAllText(path));
                        }
                        catch
                        {
                            serializedAudioData = null;
                        }
                    }
                });
        }

        public void Initialize(CutinSceneData cutinSceneData)
        {
            this.cutinSceneData = cutinSceneData;
            string audPath = Path.ChangeExtension(cutinSceneData.SavePath, ".aud");
            if(File.Exists(audPath))
            {
                file_LoadData.SelectedPath = audPath;
                serializedAudioData = SerializedAudioData.LoadData(File.ReadAllText(audPath));
            }
            RefreshInfo();
        }

        public void RefreshInfo()
        {
            if (serializedAudioData == null)
                txt_DataInfo.text = "请选择文件";
            else
            {
                MediaMatchInfo audioMatchInfo = serializedAudioData.GetAudioMatchInfo(
                    cutinSceneData.cutinScenes
                    .Select((cs) => cs.talkData_First.talkVoice)
                    .Concat(cutinSceneData.cutinScenes
                    .Select((cs) => cs.talkData_First.talkVoice)));
                txt_DataInfo.text = $"在{cutinSceneData.cutinScenes.Count*2}段语音中，有{audioMatchInfo.matchcing}段匹配，有{audioMatchInfo.missingKey}段缺失，有{audioMatchInfo.missingFile}段文件丢失";
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (serializedAudioData == null)
                errors.Add("未设置音频资料或文件损坏");
            return GenericInitializationCheck.GetErrorString("音频资料错误", errors);
        }

        public void CreateData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResultIter1 = folderBrowserDialog.ShowDialog();
            if (dialogResultIter1 != DialogResult.OK)
                return;

            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
            saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(cutinSceneData.SavePath, ".aud"));
            DialogResult dialogResultIter2 = saveFileDialog.ShowDialog();
            if (dialogResultIter2 != DialogResult.OK)
                return;

            CreateDataFrom(folderBrowserDialog.SelectedPath, saveFileDialog.FileName);
        }

        public void DownloadAndCreate()
        {
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var cutinScene in cutinSceneData.cutinScenes)
            {
                CutinVoiceInfo cutinVoiceInfoF = new CutinVoiceInfo(CutinVoiceType.bondscp, cutinScene.charFirstID, cutinScene.charSecondID, CutinVoiceOrder.first, cutinScene.dataID);
                CutinVoiceInfo cutinVoiceInfoS = new CutinVoiceInfo(CutinVoiceType.bondscp, cutinScene.charFirstID, cutinScene.charSecondID, CutinVoiceOrder.second, cutinScene.dataID);
                
                string urlF = $"{SekaiViewer.AssetUrl}/live/voice/cutin/{cutinVoiceInfoF.StandardizeName}_rip/{cutinVoiceInfoF.StandardizeName}.mp3";
                string savePathF = $"{EnvPath.AssetFolder}/assets/live/voice/cutin/{cutinVoiceInfoF.StandardizeName}_rip/{cutinVoiceInfoF.StandardizeName}.mp3";
                downloadFileInfos.Add(new DownloadFileInfo(urlF, savePathF));

                string urlS = $"{SekaiViewer.AssetUrl}/live/voice/cutin/{cutinVoiceInfoS.StandardizeName}_rip/{cutinVoiceInfoS.StandardizeName}.mp3";
                string savePathS = $"{EnvPath.AssetFolder}/assets/live/voice/cutin/{cutinVoiceInfoS.StandardizeName}_rip/{cutinVoiceInfoS.StandardizeName}.mp3";
                downloadFileInfos.Add(new DownloadFileInfo(urlS, savePathS));
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
                SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);
                saveFileDialog.FileName = Path.GetFileName(Path.ChangeExtension(cutinSceneData.SavePath, ".aud"));
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                    return;

                CreateDataFrom($"{EnvPath.AssetFolder}/assets/live/voice/cutin", saveFileDialog.FileName);
            };
            downloader.StartDownload();
        }

        void CreateDataFrom(string folderPath, string savePath)
        {
            Dictionary<string, string> rawSerializedAudioData = new Dictionary<string, string>();

            ScanFile_Classic(folderPath, rawSerializedAudioData);
            ScanFile_SV(folderPath, rawSerializedAudioData);

            SerializedAudioData sad = new SerializedAudioData(rawSerializedAudioData);
            File.WriteAllText(savePath, JsonUtility.ToJson(sad));
            file_LoadData.SelectedPath = savePath;
            serializedAudioData = sad;
            RefreshInfo();
        }

        private void ScanFile_SV(string folderPath, Dictionary<string, string> rawSerializedAudioData)
        {
            string[] directories = Directory.GetDirectories(folderPath).Select((dir) => Path.GetFileName(dir)).ToArray();
            Dictionary<string, string[]> files = new Dictionary<string, string[]>();
            foreach (var folder in directories)
            {
                files[folder] = Directory.GetFiles(Path.Combine(folderPath, folder)).Select((file) => Path.GetFileName(file)).ToArray();
            }
            foreach (var cutinScene in cutinSceneData.cutinScenes)
            {
                foreach (var voice in new string[] { cutinScene.talkData_First.talkVoice, cutinScene.talkData_Second.talkVoice})
                {
                    foreach (var folder in directories)
                    {
                        if (folder.Equals(voice) || folder.Equals($"{voice}_rip"))
                        {
                            foreach (var file in files[folder])
                            {
                                if (file.StartsWith(voice)
                                    && ExtensionTools.IsAudioFile(file))
                                {
                                    rawSerializedAudioData[voice]
                                        = Path.Combine(folderPath, folder, file);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ScanFile_Classic(string folderPath, Dictionary<string, string> rawSerializedAudioData)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                foreach (var cutinScene in cutinSceneData.cutinScenes)
                {
                    foreach (var voice in new string[] { cutinScene.talkData_First.talkVoice, cutinScene.talkData_Second.talkVoice })
                    {
                        if (fileName.StartsWith(voice) && ExtensionTools.IsAudioFile(file))
                        {
                            rawSerializedAudioData[voice] = file;
                        }
                    }
                }
            }
        }
    }
}