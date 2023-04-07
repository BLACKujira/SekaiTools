using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

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
        public string cookie;

        public DownloadFileInfo(string url, string savePath, string hash = null, string cookie = null)
        {
            this.url = url;
            this.savePath = savePath;
            this.hash = hash;
            this.cookie = cookie;
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

    public class DownloadResultCount
    {
        public int countFailure = 0;
        public int countComplete = 0;
        public int countPassExist = 0;
        public int countPassSameHash = 0;

        public int CountPass => countPassExist + countPassSameHash;
    }

    public class Downloader : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtDownload;
        public Downloader_PerecntBar perecntBar;
        public MessageAreaTypeA messageArea;
        public List<Button> logViewButtons;
        public List<Button> logSaveButtons;
        [Header("Settings")]
        public float retryWaitTime = 3;
        public int retryTimes = 5;
        public ExistingFileProcessingMode existingFileProcessingMode = ExistingFileProcessingMode.Override;

        DownloadFileInfo[] downloadFiles;
        List<DownloaderLogItem> downloaderLog = new List<DownloaderLogItem>();
        bool isDone = false;
        bool disableLogView = false;

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
            public bool disableLogView = false;
        }

        public void Initialize(Settings settings)
        {
            retryWaitTime = settings.retryWaitTime;
            retryTimes = settings.retryTimes;
            existingFileProcessingMode = settings.existingFileProcessingMode;
            downloadFiles = settings.downloadFiles;
            disableLogView = settings.disableLogView;

            if(settings.disableLogView)
            {
                foreach (var button in logViewButtons)
                {
                    button.interactable = false;
                }
            }
            foreach (var button in logSaveButtons)
            {
                button.interactable = false;
            }

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
            RefreshTextDownload();
            for (int i = 0; i < downloadFiles.Length; i++)
            {
                DownloadFileInfo downloadFileInfo = downloadFiles[i];
                DownloaderLogItem downloaderLogItem = new DownloaderLogItem(DateTime.Now, downloadFileInfo);
                string fileName = Path.GetFileName(downloadFileInfo.savePath);
                yield return null;

                RefreshTextDownload();
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
                        if(!string.IsNullOrEmpty(downloadFileInfo.cookie))
                        {
                            getRequest.SetRequestHeader("cookie", downloadFileInfo.cookie);
                        }
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
                            string saveFolder = Path.GetDirectoryName(downloadFileInfo.savePath);
                            if (!Directory.Exists(saveFolder))
                                Directory.CreateDirectory(saveFolder);
                            File.Copy(tempFilePath, downloadFileInfo.savePath);
                            messageArea.AddLine($"{fileName} 下载完成");
                            downloaderLogItem.downloadResult = DownloadResult.Complete;
                            currentGetRequest = null;
                            break;
                        }
                        currentGetRequest = null;
                    }
                }
                downloaderLogItem.endTime = DateTime.Now;
                downloaderLog.Add(downloaderLogItem);
            }
            RefreshTextDownload();
            perecntBar.priority = 1;
            perecntBar.info = "已完成";
            OnComplete?.Invoke();
            if (!disableLogView)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = true;
                }
            }

            yield break;
        }

        private void RefreshTextDownload()
        {
            DownloadResultCount downloadResultCount = GetDownloadResultCount();
            txtDownload.text = $"下载 成功{downloadResultCount.countComplete} 跳过{downloadResultCount.CountPass} 失败{downloadResultCount.countFailure} / {downloadFiles.Length}个文件";
        }

        public void ViewErrorLog()
        {
            WindowController.ShowLog("错误日志", GetLog(true));
        }

        public void ViewLog()
        {
            WindowController.ShowLog("下载日志", GetLog(true));
        }

        public void SaveLog()
        {
            SaveFileDialog saveFileDialog
                = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_TXT);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            File.WriteAllText(saveFileDialog.FileName, GetLog(false));
        }

        public string GetLog(bool onlyShowError)
        {
            List<string> outStrs = new List<string>();
            foreach (var logItem in downloaderLog)
            {
                if (onlyShowError&&logItem.downloadResult == DownloadResult.Complete)
                    continue;
                switch (logItem.downloadResult)
                {
                    case DownloadResult.Null:
                        break;
                    case DownloadResult.Failure:
                        outStrs.Add($"[{logItem.endTime:T}] 下载失败 {logItem.error}\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.Complete:
                        outStrs.Add($"[{logItem.endTime:T}] 下载完成\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.PassExist:
                        outStrs.Add($"[{logItem.endTime:T}] 已存在，跳过\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.PassSameHash:
                        outStrs.Add($"[{logItem.endTime:T}] hash相同，跳过\n{logItem.downloadFileInfo.url}");
                        break;
                    default:
                        break;
                }
            }
            return string.Join("\n", outStrs);
        }

        public void EnableLogView()
        {
            disableLogView = false;
            foreach (var button in logViewButtons)
            {
                button.interactable = true;
            }
            if(isDone)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = true;
                }
            }
        }

        public void DisableLogView()
        {
            disableLogView = true;
            foreach (var button in logViewButtons)
            {
                button.interactable = false;
            }
            if (isDone)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = false;
                }
            }
        }

        public DownloadResultCount GetDownloadResultCount()
        {
            DownloadResultCount downloadResultCount = new DownloadResultCount();
            foreach (var downloaderLogItem in downloaderLog)
            {
                switch (downloaderLogItem.downloadResult)
                {
                    case DownloadResult.Null:
                        break;
                    case DownloadResult.Failure:
                        downloadResultCount.countFailure++;
                        break;
                    case DownloadResult.Complete:
                        downloadResultCount.countComplete++;
                        break;
                    case DownloadResult.PassExist:
                        downloadResultCount.countPassExist++;
                        break;
                    case DownloadResult.PassSameHash:
                        downloadResultCount.countPassSameHash++;
                        break;
                    default:
                        break;
                }
            }
            return downloadResultCount;
        }
    }
}