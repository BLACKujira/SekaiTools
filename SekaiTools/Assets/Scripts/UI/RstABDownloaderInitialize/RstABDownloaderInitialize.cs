using SekaiTools.OtherGames.ReStage;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.RstABDownloaderInitialize
{
    public class RstABDownloaderInitialize : MonoBehaviour
    {
        public const string URL_HEAD = "https://rs.rst-game.com/Android2";

        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_PathSelect gIP_PathSelect;
        [Header("Prefab")]
        public Window downloaderPrefab;

        private void Awake()
        {
            gIP_PathSelect.Initialize();
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase,gIP_PathSelect);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR,errors);
            }
            else
            {
                try
                {
                    Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
                    Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
                    settings.retryTimes = gIP_DownloaderBase.retryTimes;
                    settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
                    settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;

                    List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
                    string abvText = File.ReadAllText(gIP_PathSelect.pathSelectItems[0].SelectedPath);
                    List<AssetBundleVersion> assetBundleVersions = AssetBundleVersion.ToList(abvText);
                    string savePath = gIP_PathSelect.pathSelectItems[1].SelectedPath;
                    foreach (var assetBundleVersion in assetBundleVersions)
                    {
                        DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                            $"{URL_HEAD}/{assetBundleVersion.assetBundleName}",
                            $"{savePath}/{assetBundleVersion.assetBundleName}");
                        downloadFileInfos.Add(downloadFileInfo);
                    }

                    settings.downloadFiles = downloadFileInfos.ToArray();

                    downloader.Initialize(settings);
                    downloader.StartDownload();
                }
                catch(System.Exception ex)
                {
                    WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, ex.ToString());
                }
            }
        }
    }
}