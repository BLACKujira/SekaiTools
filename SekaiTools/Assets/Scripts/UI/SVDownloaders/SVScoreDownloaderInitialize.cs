using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SVScoreDownloaderInitialize
{
    public class SVScoreDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        public GIP_PathSelect gIP_FolderSelect;
        [Header("Prefab")]
        public Window downloaderPrefab;

        private void Awake()
        {
            gIP_FolderSelect.pathSelectItems[0].defaultPath = $"{EnvPath.AssetFolder}\\music\\music_score";
            gIP_FolderSelect.pathSelectItems[0].ResetPath();
        }

        public void Apply()
        {
            string error = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_MasterRefUpdate, gIP_FolderSelect);
            if(!string.IsNullOrEmpty(error))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, error);
                return;
            }

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;

            MasterMusic[] masterMusics;
            try
            {
                masterMusics = EnvPath.GetTable<MasterMusic>("musics");
            }
            catch(System.Exception ex)
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, ex.Message);
                return;
            }

            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            string[] difficulties = { "easy", "normal", "hard", "expert", "master" };
            string selectedPath = gIP_FolderSelect.pathSelectItems[0].SelectedPath;
            foreach (var masterMusic in masterMusics)
            {
                foreach (var difficulty in difficulties)
                {
                    DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                     $"{SekaiViewer.AssetUrl}/music/music_score/{masterMusic.id.ToString("0000")}_01_rip/{difficulty}.txt",
                     $"{selectedPath}/{masterMusic.id.ToString("0000")}_01_rip/{difficulty}.txt");
                    downloadFileInfos.Add(downloadFileInfo);
                }
            }
            settings.downloadFiles = downloadFileInfos.ToArray();

            downloader.Initialize(settings);
            downloader.StartDownload();
        }
    }
}