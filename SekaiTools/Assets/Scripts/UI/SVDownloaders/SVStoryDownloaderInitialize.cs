using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCounterInitialize;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SVDownloaders
{
    public class SVStoryDownloaderInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_DownloaderBase gIP_DownloaderBase;
        public GIP_SVStoryType gIP_SVStoryType;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        public GIP_SVStory gIP_SVStory;
        [Header("Prefab")]
        public Window downloaderPrefab;

        public static HashSet<string> RequireMasterTables = new HashSet<string>()
        {
            "unitStories","eventStories","cardEpisodes","actionSets","virtualLives","specialStories"
        };

        private void Awake()
        {
            gIP_SVStory.pathSelectItems[0].SelectedPath = EnvPath.Assets;
            gIP_MasterRefUpdate.SetMasterRefUpdateItems(RequireMasterTables);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_DownloaderBase, gIP_MasterRefUpdate, gIP_SVStory);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            FileStruct fileStruct = gIP_SVStory.FileStruct;
            string selectedPath = gIP_SVStory.pathSelectItems[0].SelectedPath;

            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            if (gIP_SVStoryType.Download_Unit) downloadFileInfos.AddRange(GetDownloadFile_Unit(selectedPath, fileStruct));
            if (gIP_SVStoryType.Download_Event) downloadFileInfos.AddRange(GetDownloadFile_Event(selectedPath, fileStruct));
            if (gIP_SVStoryType.Download_Card) downloadFileInfos.AddRange(GetDownloadFile_Card(selectedPath, fileStruct));
            if (gIP_SVStoryType.Download_Map) downloadFileInfos.AddRange(GetDownloadFile_Map(selectedPath, fileStruct));
            if (gIP_SVStoryType.Download_Live) downloadFileInfos.AddRange(GetDownloadFile_Live(selectedPath, fileStruct));
            if (gIP_SVStoryType.Download_Other) downloadFileInfos.AddRange(GetDownloadFile_Other(selectedPath, fileStruct));

            Downloader.Downloader downloader = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.retryTimes = gIP_DownloaderBase.retryTimes;
            settings.retryWaitTime = gIP_DownloaderBase.retryWaitTime;
            settings.existingFileProcessingMode = gIP_DownloaderBase.existingFileProcessingMode;
            settings.downloadFiles = downloadFileInfos.ToArray();

            downloader.Initialize(settings);
            downloader.StartDownload();
        }

        DownloadFileInfo[] GetDownloadFile_Unit(string folder_Sample, FileStruct fileStruct)
        {
            MasterUnitStory[] masterUnitStories = EnvPath.GetTable<MasterUnitStory>("unitStories");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterUnitStory in masterUnitStories)
            {
                foreach (var masterUnitStoryChapter in masterUnitStory.chapters)
                {
                    foreach (var masterUnitStoryEpisode in masterUnitStoryChapter.episodes)
                    {
                        if (string.IsNullOrEmpty(masterUnitStoryChapter.assetbundleName)) continue;
                        string url = $"{SekaiViewer.AssetUrl}/scenario/unitstory/{masterUnitStoryChapter.assetbundleName}_rip/{masterUnitStoryEpisode.scenarioId}.asset";
                        string savepath = null;
                        switch (fileStruct)
                        {
                            case FileStruct.Server:
                                savepath = $"{folder_Sample}/scenario/unitstory/{masterUnitStoryChapter.assetbundleName}_rip/{masterUnitStoryEpisode.scenarioId}.json";
                                break;
                            case FileStruct.Classic:
                                savepath = $"{folder_Sample}/{NicknameCountData.unitStoriesFolder}/{masterUnitStoryEpisode.scenarioId}.json";
                                break;
                            default:
                                break;
                        }
                        downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
                    }
                }
            }
            return downloadFileInfos.ToArray();
        }

        DownloadFileInfo[] GetDownloadFile_Event(string folder_Sample, FileStruct fileStruct)
        {
            MasterEventStory[] masterEventStories = EnvPath.GetTable<MasterEventStory>("eventStories");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterEventStory in masterEventStories)
            {
                foreach (var eventStoryEpisode in masterEventStory.eventStoryEpisodes)
                {
                    if (string.IsNullOrEmpty(masterEventStory.assetbundleName)) continue;
                    string url = $"{SekaiViewer.AssetUrl}/event_story/{masterEventStory.assetbundleName}/scenario_rip/{eventStoryEpisode.scenarioId}.asset";
                    string savepath = null;
                    switch (fileStruct)
                    {
                        case FileStruct.Server:
                            savepath = $"{folder_Sample}/event_story/{masterEventStory.assetbundleName}/scenario_rip/{eventStoryEpisode.scenarioId}.json";
                            break;
                        case FileStruct.Classic:
                            savepath = $"{folder_Sample}/{NicknameCountData.eventStoriesFolder}/{eventStoryEpisode.scenarioId}.json";
                            break;
                        default:
                            break;
                    }
                    downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
                }
            }
            return downloadFileInfos.ToArray();
        }

        DownloadFileInfo[] GetDownloadFile_Card(string folder_Sample, FileStruct fileStruct)
        {
            MasterCardEpisode[] masterCardEpisodes = EnvPath.GetTable<MasterCardEpisode>("cardEpisodes");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterCardEpisode in masterCardEpisodes)
            {
                if (string.IsNullOrEmpty(masterCardEpisode.assetbundleName)) continue;
                string url = $"{SekaiViewer.AssetUrl}/character/member/{masterCardEpisode.assetbundleName}_rip/{masterCardEpisode.scenarioId}.asset";
                string savepath = null;
                switch (fileStruct)
                {
                    case FileStruct.Server:
                        savepath = $"{folder_Sample}/character/member/{masterCardEpisode.assetbundleName}_rip/{masterCardEpisode.scenarioId}.json";
                        break;
                    case FileStruct.Classic:
                        savepath = $"{folder_Sample}/{NicknameCountData.cardStoriesFolder}/{masterCardEpisode.scenarioId}.json";
                        break;
                    default:
                        break;
                }
                downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
            }
            return downloadFileInfos.ToArray();
        }

        DownloadFileInfo[] GetDownloadFile_Map(string folder_Sample, FileStruct fileStruct)
        {
            MasterActionSet[] masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterActionSet in masterActionSets)
            {
                if (masterActionSet.id <= 4) continue;
                if (string.IsNullOrEmpty(masterActionSet.scenarioId)) continue;

                string url = $"{SekaiViewer.AssetUrl}/scenario/actionset/group{masterActionSet.id / 100}_rip/{masterActionSet.scenarioId}.asset";
                string savepath = null;
                switch (fileStruct)
                {
                    case FileStruct.Server:
                        savepath = $"{folder_Sample}/scenario/actionset/group{masterActionSet.id / 100}_rip/{masterActionSet.scenarioId}.json";
                        break;
                    case FileStruct.Classic:
                        savepath = $"{folder_Sample}/{NicknameCountData.mapTalkFolder}/{masterActionSet.scenarioId}.json";
                        break;
                    default:
                        break;
                }
                downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
            }
            return downloadFileInfos.ToArray();
        }

        DownloadFileInfo[] GetDownloadFile_Live(string folder_Sample, FileStruct fileStruct)
        {
            MasterVirtualLive[] masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterVirtualLive in masterVirtualLives)
            {
                foreach (var masterVirtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                {
                    if (masterVirtualLiveSetlist.VirtualLiveSetlistType != VirtualLiveSetlistType.mc) continue;
                    if (string.IsNullOrEmpty(masterVirtualLiveSetlist.assetbundleName)) continue;

                    string url = $"{SekaiViewer.AssetUrl}/virtual_live/mc/scenario/{masterVirtualLiveSetlist.assetbundleName}_rip/{masterVirtualLiveSetlist.assetbundleName}.asset";
                    string savepath = null;
                    switch (fileStruct)
                    {
                        case FileStruct.Server:
                            savepath = $"{folder_Sample}/virtual_live/mc/scenario/{masterVirtualLiveSetlist.assetbundleName}_rip/{masterVirtualLiveSetlist.assetbundleName}.json";
                            break;
                        case FileStruct.Classic:
                            savepath = $"{folder_Sample}/{NicknameCountData.liveTalkFolder}/{masterVirtualLiveSetlist.assetbundleName}.json";
                            break;
                        default:
                            break;
                    }
                    downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
                }
            }
            return downloadFileInfos.ToArray();
        }

        DownloadFileInfo[] GetDownloadFile_Other(string folder_Sample, FileStruct fileStruct)
        {
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            for (int i = 1; i < 27; i++)
            {
                string url = $"{SekaiViewer.AssetUrl}/scenario/profile_rip/self_{(Character)i}.asset";
                string savepath = null;
                switch (fileStruct)
                {
                    case FileStruct.Server:
                        savepath = $"{folder_Sample}/scenario/profile_rip/self_{(Character)i}.json";
                        break;
                    case FileStruct.Classic:
                        savepath = $"{folder_Sample}/{NicknameCountData.otherStoriesFolder}/self_{(Character)i}.json";
                        break;
                    default:
                        break;
                }
                downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
            }

            MasterSpecialStory[] masterSpecialStories = EnvPath.GetTable<MasterSpecialStory>("specialStories");
            foreach (var masterSpecialStory in masterSpecialStories)
            {
                foreach (var masterSpecialStoryEpisode in masterSpecialStory.episodes)
                {
                    if (string.IsNullOrEmpty(masterSpecialStoryEpisode.assetbundleName)) continue;

                    string url = $"{SekaiViewer.AssetUrl}/scenario/special/{masterSpecialStoryEpisode.assetbundleName}_rip/{masterSpecialStoryEpisode.scenarioId}.asset";
                    string savepath = null;
                    switch (fileStruct)
                    {
                        case FileStruct.Server:
                            savepath = $"{folder_Sample}/scenario/special/{masterSpecialStoryEpisode.assetbundleName}_rip/{masterSpecialStoryEpisode.scenarioId}.json";
                            break;
                        case FileStruct.Classic:
                            savepath = $"{folder_Sample}/{NicknameCountData.otherStoriesFolder}/{masterSpecialStoryEpisode.scenarioId}.json";
                            break;
                        default:
                            break;
                    }
                    downloadFileInfos.Add(new DownloadFileInfo(url, savepath));
                }
            }
            return downloadFileInfos.ToArray();
        }
    }
}