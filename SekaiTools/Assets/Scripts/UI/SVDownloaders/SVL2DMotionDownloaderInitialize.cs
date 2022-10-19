using SekaiTools.SekaiViewerInterface;
using SekaiTools.SekaiViewerInterface.Pages;
using SekaiTools.SekaiViewerInterface.Utils;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SVDownloaders
{
    public class SVL2DMotionDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_PathSelect gIP_PathSelect;
        [Header("Prefab")]
        public Window downloaderPrefab;

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_PathSelect);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            StartCoroutine(CoApply());
        }

        IEnumerator CoApply()
        {
            bool ifError = false;
            ListBucketResult fp_live2d_motion = null;
            string savePath = gIP_PathSelect.pathSelectItems[0].SelectedPath;

            IEnumerator getMotionList = AssetViewer.GetFilePath("live2d/motion",
                            (fp) => fp_live2d_motion = fp,
                            (err) =>
                            {
                                ifError = true;
                                WindowController.ShowMessage(Message.Error.STR_ERROR, err);
                            });
            WindowController.ShowNowLoadingCenter("正在获取动作列表", getMotionList);
            yield return getMotionList;
            if (ifError)
                yield break;

            List<ListBucketResult> fp_live2d_motions = new List<ListBucketResult>();
            foreach (var commonPrefixes in fp_live2d_motion.CommonPrefixes)
            {
                IEnumerator getMotions = AssetViewer.GetFilePath(commonPrefixes.Prefix,
                    (fp) => fp_live2d_motions.Add(fp),
                    (err) =>
                    {
                        ifError = true;
                        WindowController.ShowMessage(Message.Error.STR_ERROR, err);
                    });
                WindowController.ShowNowLoadingCenter($"正在获取动作列表 {commonPrefixes.Prefix}", getMotions);
                yield return getMotions;
                if (ifError)
                    yield break;
            }

            List<DownloadFileInfo> downloadFileInfos = new List<Downloader.DownloadFileInfo>();
            foreach (var filePath in fp_live2d_motions)
            {
                foreach (var contents in filePath.Contents)
                {
                    if (contents.Key.EndsWith("BuildMotionData.json"))
                        continue;
                    DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                        $"{SekaiViewer.AssetUrl}/{contents.Key}",
                        $"{savePath}/{contents.Key}");
                    downloadFileInfos.Add(downloadFileInfo);
                }
            }

            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;
            settings.downloadFiles = downloadFileInfos.ToArray();

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            downloader.Initialize(settings);
            downloader.StartDownload();
        }
    }
}