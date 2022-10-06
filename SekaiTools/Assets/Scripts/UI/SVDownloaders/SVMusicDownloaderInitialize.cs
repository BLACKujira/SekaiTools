using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SVDownloaders
{
    public class SVMusicDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate; 
        public GIP_SVMusic gIP_SVMusic;
        [Header("Prefab")]
        public Window downloaderPrefab;

        private void Awake()
        {
            gIP_SVMusic.Initialize();
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_MasterRefUpdate, gIP_SVMusic);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, errors);
                return;
            }

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;

            MasterMusicVocal[] masterMusicVocals;
            try
            {
                masterMusicVocals = JsonHelper.getJsonArray<MasterMusicVocal>(
                    File.ReadAllText(
                        Path.Combine(EnvPath.sekai_master_db_diff, "musicVocals.json")));
            }
            catch (System.Exception ex)
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, ex.Message);
                return;
            }

            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterMusicVocal in masterMusicVocals)
            {
                DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                    $"{SekaiViewer.AssetUrl}/music/long/{masterMusicVocal.assetbundleName}_rip/{masterMusicVocal.assetbundleName}{gIP_SVMusic.format}",
                    $"{gIP_SVMusic.saveFolder.SelectedPath}/{masterMusicVocal.assetbundleName}_rip/{masterMusicVocal.assetbundleName}{gIP_SVMusic.format}");
                downloadFileInfos.Add(downloadFileInfo);
            }
            settings.downloadFiles = downloadFileInfos.ToArray();

            downloader.Initialize(settings);
            downloader.StartDownload();
        }
    }
}