using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.Message;
using SekaiTools.DecompiledClass;
using System.Linq;
using System.IO;

namespace SekaiTools.UI.AssetDownloaderInitialize
{
    public class AssetDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_AssetList gIP_AssetList;
        public GIP_AssetListSettings gIP_AssetListSettings;
        public GIP_PathSelect gIP_SavePath;
        [Header("Prefab")]
        public Window downloaderPrefab;

        private void Awake()
        {
            gIP_AssetList.Initialize();
            gIP_SavePath.Initialize();
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(
                gIP_DownloaderBase,
                gIP_AssetList,
                gIP_AssetListSettings,
                gIP_SavePath);
            
            if(errors!=null)
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            BundleRoot bundleRoot;
            //try
            {
                bundleRoot = gIP_AssetList.GetBundleRoot();
            }
            //catch(System.Exception ex)
            //{
            //    WindowController.ShowMessage(Message.Error.STR_ERROR, ex.Message);
            //    return;
            //}

            List<BundlesItem> bundlesItems = new List<BundlesItem>();
            if (!gIP_AssetListSettings.UseStartWith)
            {
                bundlesItems = new List<BundlesItem>(bundleRoot.bundles);
            }
            else
            {
                string startsWith = gIP_AssetListSettings.GetStartWithString();
                bundlesItems = new List<BundlesItem>(
                    from BundlesItem bi in bundleRoot.bundles
                    where bi.bundleName.StartsWith(startsWith)
                    select bi);
            }

            long totalSizeLong = 0;
            foreach (var bundlesItem in bundlesItems)
            {
                totalSizeLong += bundlesItem.fileSize;
            }

            double totalSize = totalSizeLong;
            string sizeStr;
            if (totalSize < 1024 * 1024)
                sizeStr = $"{totalSize / 1024:0.00} KB";
            else if (totalSize < 1024 * 1024 * 1024)
                sizeStr = $"{totalSize / (1024 * 1024):0.00} MB";
            else
                sizeStr = $"{totalSize / (1024 * 1024 * 1024):0.00} GB";

            string cookie = File.ReadAllText(gIP_AssetList.lfsi_Cookie.SelectedPath);

            WindowController.ShowCancelOK($"即将下载{bundlesItems.Count}个文件", $"约{sizeStr}",
                () =>
                {
                    string urlHead = gIP_AssetList.GetURLHead();
                    string savePath = gIP_SavePath.pathSelectItems[0].SelectedPath;
                    Downloader.DownloadFileInfo[] downloadFileInfos = bundlesItems.Select(
                        (bi) => new Downloader.DownloadFileInfo
                        (urlHead + bi.bundleName,
                        Path.Combine(savePath, bi.bundleName),
                        bi.hash, cookie)).ToArray();

                    Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
                    settings.retryTimes = gIP_DownloaderBase.retryTimes;
                    settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
                    settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;
                    settings.downloadFiles = downloadFileInfos;

                    Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
                    downloader.Initialize(settings);
                    downloader.StartDownload();
                });
           
        }
    }
}