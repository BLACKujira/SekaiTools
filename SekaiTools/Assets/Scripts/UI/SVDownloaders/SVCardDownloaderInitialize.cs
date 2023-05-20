using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SVDownloaders
{
    public class SVCardDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        public GIP_SVCard gIP_SVCard;
        [Header("Prefab")]
        public Window downloaderPrefab;

        private void Awake()
        {
            gIP_SVCard.Initialize();
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_MasterRefUpdate, gIP_SVCard);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;

            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();

            HashSet<int> selectedCharacters = new HashSet<int>(gIP_SVCard.SelectedCharacters);

            try
            {
                MasterCard[] masterCards = EnvPath.GetTable<MasterCard>("cards");
                foreach (var masterCard in masterCards)
                {
                    if (!selectedCharacters.Contains(masterCard.characterId)) continue;

                    DownloadFileInfo downloadFileInfo = new DownloadFileInfo(
                        $"{SekaiViewer.AssetUrl}/character/member/{masterCard.assetbundleName}_rip/card_normal{gIP_SVCard.format}",
                        $"{gIP_SVCard.folderSelectItem.SelectedPath}/{masterCard.assetbundleName}_rip/card_normal{gIP_SVCard.format}");
                    downloadFileInfos.Add(downloadFileInfo);

                    CardRarityType rarityType = masterCard.RarityType;
                    if (rarityType == CardRarityType.rarity_3 || rarityType == CardRarityType.rarity_4)
                    {
                        DownloadFileInfo downloadFileInfo_at = new DownloadFileInfo(
                            $"{SekaiViewer.AssetUrl}/character/member/{masterCard.assetbundleName}_rip/card_after_training{gIP_SVCard.format}",
                            $"{gIP_SVCard.folderSelectItem.SelectedPath}/{masterCard.assetbundleName}_rip/card_after_training{gIP_SVCard.format}");
                        downloadFileInfos.Add(downloadFileInfo_at);
                    }
                }
            }
            catch(System.Exception ex)
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, ex.Message);
                return;
            }

            settings.downloadFiles = downloadFileInfos.ToArray();

            downloader.Initialize(settings);
            downloader.StartDownload();
        }
    }
}