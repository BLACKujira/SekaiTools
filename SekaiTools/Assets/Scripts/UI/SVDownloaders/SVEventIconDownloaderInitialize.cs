using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SVScoreDownloaderInitialize
{
    public class SVEventIconDownloaderInitialize : MonoBehaviour
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
            gIP_FolderSelect.pathSelectItems[0].defaultPath = $"{EnvPath.Assets}\\event";
            gIP_FolderSelect.pathSelectItems[0].ResetPath();
        }

        public void Apply()
        {
            string error = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_MasterRefUpdate, gIP_FolderSelect);
            if (!string.IsNullOrEmpty(error))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, error);
                return;
            }

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;

            MasterEvent[] masterEvents;
            try
            {
                masterEvents = EnvPath.GetTable<MasterEvent>("events");
            }
            catch (System.Exception ex)
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, ex.Message);
                return;
            }

            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            string selectedPath = gIP_FolderSelect.pathSelectItems[0].SelectedPath;
            foreach (var masterEvent in masterEvents)
            {
                DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                 $"{SekaiViewer.AssetUrl}/event/{masterEvent.assetbundleName}/logo_rip/logo.png",
                 $"{selectedPath}/{masterEvent.assetbundleName}/logo_rip/logo.png");
                downloadFileInfos.Add(downloadFileInfo);
            }
            settings.downloadFiles = downloadFileInfos.ToArray();

            downloader.Initialize(settings);
            downloader.StartDownload();
        }
    }
}