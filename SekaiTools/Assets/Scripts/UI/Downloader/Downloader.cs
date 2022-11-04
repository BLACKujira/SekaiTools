using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace SekaiTools.UI.Downloader
{
    public enum ExistingFileProcessingMode 
    { 
        /// <summary>
        /// 覆盖现有文件
        /// </summary>
        Override,
        /// <summary>
        /// 跳过已有文件
        /// </summary>
        Pass,
        /// <summary>
        /// 计算已有文件hash，相同则跳过
        /// </summary>
        CheckHash 
    }

    public class DownloadFileInfo
    {
        public string url;
        public string savePath;
        public string hash;

        public DownloadFileInfo(string url, string savePath, string hash = null)
        {
            this.url = url;
            this.savePath = savePath;
            this.hash = hash;
        }
    }

    public enum DownloadResult
    {
        Null,
        Failure,
        Complete,
        PassExist,
        PassSameHash
    }

    public class DownloaderLogItem
    {
        public DateTime startTime;
        public DateTime endTime;
        public DownloadFileInfo downloadFileInfo;
        public DownloadResult downloadResult;
        public string error = null;

        public DownloaderLogItem(DateTime startTime, DownloadFileInfo downloadFileInfo)
        {
            this.startTime = startTime;
            this.downloadFileInfo = downloadFileInfo;
        }
    }


    public class Downloader : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Downloader_PerecntBar perecntBar;
        public MessageAreaTypeA messageArea;
        [Header("Settings")]
        public float retryWaitTime = 3;
        public int retryTimes = 5;
        public ExistingFileProcessingMode existingFileProcessingMode = ExistingFileProcessingMode.Override;

        DownloadFileInfo[] downloadFiles;
        List<DownloaderLogItem> downloaderLog = new List<DownloaderLogItem>();
        bool isDone = false;
        
        public bool IsDone => isDone;
        public DownloaderLogItem[] DownloaderLog => downloaderLog.ToArray();
        public bool HasError
        {
            get
            {
                foreach (var downloaderLogItem in downloaderLog)
                {
                    if (downloaderLogItem.downloadResult == DownloadResult.Failure)
                        return true;
                }
                return false;
            }
        }

        public event Action OnComplete;

        public class Settings
        {
            public float retryWaitTime = 3;
            public int retryTimes = 5;
            public ExistingFileProcessingMode existingFileProcessingMode = ExistingFileProcessingMode.Override;
            public DownloadFileInfo[] downloadFiles;
        }

        public void Initialize(Settings settings)
        {
            retryWaitTime = settings.retryWaitTime;
            retryTimes = settings.retryTimes;
            existingFileProcessingMode = settings.existingFileProcessingMode;
            downloadFiles = settings.downloadFiles;

            window.OnClose.AddListener(() =>
            {
                StopAllCoroutines();
                if (currentGetRequest != null && !currentGetRequest.isDone)
                {
                    currentGetRequest.Abort();
                    currentGetRequest.Dispose();
                }
                if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            });
        }

        public void StartDownload()
        {
            StartCoroutine(IDownload());
        }

        string tempFilePath = null;
        UnityWebRequest currentGetRequest = null;
        IEnumerator IDownload()
        {
            for (int i = 0; i < downloadFiles.Length; i++)
            {
                DownloadFileInfo downloadFileInfo = downloadFiles[i];
                DownloaderLogItem downloaderLogItem = new DownloaderLogItem(DateTime.Now, downloadFileInfo);
                string fileName = Path.GetFileName(downloadFileInfo.savePath);
                yield return null;

                perecntBar.priority = (float)i / downloadFiles.Length;
                perecntBar.info = $"正在下载 {Path.GetFileName(downloadFileInfo.savePath)}";

                string dir = Path.GetDirectoryName(downloadFileInfo.savePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                //检查文件是否存在
                switch (existingFileProcessingMode)
                {
                    case ExistingFileProcessingMode.Override:
                        break;
                    case ExistingFileProcessingMode.Pass:
                        if (File.Exists(downloadFileInfo.savePath))
                        {
                            messageArea.AddLine($"{fileName} 已存在，跳过");
                            downloaderLogItem.endTime = DateTime.Now;
                            downloaderLogItem.downloadResult = DownloadResult.PassExist;
                            downloaderLog.Add(downloaderLogItem);
                            continue;
                        }
                        break;
                    case ExistingFileProcessingMode.CheckHash:
                        throw new System.NotImplementedException();
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(tempFilePath))
                    tempFilePath = Path.GetTempFileName();
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                //在重试次数内下载
                for (int t = 0; t < retryTimes; t++)
                {
                    using (UnityWebRequest getRequest = UnityWebRequest.Get(downloadFileInfo.url))
                    {
                        getRequest.downloadHandler = new DownloadHandlerFile(tempFilePath, false);
                        currentGetRequest = getRequest;
                        getRequest.SendWebRequest();
                        messageArea.AddLine($"{fileName} 开始下载");
                        while (!getRequest.isDone)
                        {
                            yield return null;
                        }
                        string responseCode = getRequest.responseCode.ToString();
                        if (getRequest.error != null)
                        {
                            perecntBar.info = $"正在等待重试({retryWaitTime}s) {fileName}";
                            messageArea.AddLine($"{fileName} 下载失败，{getRequest.error}");
                            if (t == retryTimes - 1)
                            {
                                messageArea.AddLine($"{fileName} 超出最大重试次数");
                                downloaderLogItem.error = getRequest.error;
                                downloaderLogItem.downloadResult = DownloadResult.Failure;
                                if (File.Exists(downloadFileInfo.savePath))
                                    File.Delete(downloadFileInfo.savePath);
                            }
                            yield return new WaitForSeconds(retryWaitTime);
                            perecntBar.info = $"第{t + 1}次重试 {fileName}";
                        }
                        else
                        {
                            if (File.Exists(downloadFileInfo.savePath))
                                File.Delete(downloadFileInfo.savePath);
                            File.Copy(tempFilePath, downloadFileInfo.savePath);
                            messageArea.AddLine($"{fileName} 下载完成");
                            downloaderLogItem.downloadResult = DownloadResult.Complete;
                            currentGetRequest = null;
                            break;
                        }
                        currentGetRequest = null;
                    }
                }
                downloaderLog.Add(downloaderLogItem);
            }
            perecntBar.priority = 1;
            perecntBar.info = "已完成";
            OnComplete();
            yield break;
        }
    }
}