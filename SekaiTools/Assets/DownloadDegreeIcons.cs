using SekaiTools.UI;
using SekaiTools;
using SekaiTools.UI.Downloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadDegreeIcons : MonoBehaviour
{
    public Window downloaderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
        for (int i = 1; i < 56; i++)
        {
            DownloadFileInfo downloadFileInfo = new DownloadFileInfo
                (
                $"{SekaiTools.SekaiViewerInterface.SekaiViewer.AssetUrl}/bonds_honor/character/chr_sd_{i:00}_01_rip/chr_sd_{i:00}_01.png",
                $"{EnvPath.Assets}/bonds_honor/character/chr_sd_{i:00}_01_rip/chr_sd_{i:00}_01.png"
                );
            downloadFileInfos.Add(downloadFileInfo);
        }
        Downloader.Settings settings = new Downloader.Settings();
        settings.downloadFiles = downloadFileInfos.ToArray();
        settings.existingFileProcessingMode = ExistingFileProcessingMode.Pass;
        Downloader downloader
            = WindowController.windowController.currentWindow.OpenWindow<Downloader>(downloaderPrefab);
        downloader.Initialize(settings);
        downloader.StartDownload();
    }
}
