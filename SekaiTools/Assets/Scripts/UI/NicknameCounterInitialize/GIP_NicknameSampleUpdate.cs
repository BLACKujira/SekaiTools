using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.UI.Downloader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    [System.Obsolete]
    public class GIP_NicknameSampleUpdate : MonoBehaviour
    {
        [Header("Prefab")]
        public Window downloaderPrefab;

        public string folder_Sample = EnvPath.AssetFolder;
        FileStruct fileStruct = FileStruct.Server;

        public void DownloadSamples()
        {
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            downloadFileInfos.AddRange(GetDownloadFile_Unit());
            downloadFileInfos.AddRange(GetDownloadFile_Event());
            downloadFileInfos.AddRange(GetDownloadFile_Card());
            downloadFileInfos.AddRange(GetDownloadFile_Map());
            downloadFileInfos.AddRange(GetDownloadFile_Live());
            downloadFileInfos.AddRange(GetDownloadFile_Other());

            Downloader.Downloader.Settings settings = new Downloader.Downloader.Settings();
            settings.downloadFiles = downloadFileInfos.ToArray();
            settings.existingFileProcessingMode = ExistingFileProcessingMode.Pass;
            settings.disableLogView = true;
            Downloader.Downloader downloader
                = WindowController.windowController.currentWindow.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            downloader.Initialize(settings);
            downloader.OnComplete += () =>
            {
                if (downloader.HasError)
                {
                    downloader.EnableLogView();
                    WindowController.ShowMessage(Message.Error.STR_ERROR, "有一部分样本未能下载，统计的样本中不会包含这些文件");
                    return;
                }
                downloader.window.Close();
            };
            downloader.StartDownload();
        }

        DownloadFileInfo[] GetDownloadFile_Unit()
        {
            MasterUnitStory[] masterUnitStories = EnvPath.GetTable<MasterUnitStory>("unitStories");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterUnitStory in masterUnitStories)
            {
                foreach (var masterUnitStoryChapter in masterUnitStory.chapters)
                {
                    foreach (var masterUnitStoryEpisode in masterUnitStoryChapter.episodes)
                    {
                        string url = $"{SekaiViewer.AssetUrl}/scenario/unitstory/{masterUnitStoryChapter.assetbundleName}_rip/{masterUnitStoryEpisode.scenarioId}.asset";
                        string savepath = null;
                        switch (fileStruct)
                        {
                            case FileStruct.Server: savepath = $"{folder_Sample}/scenario/unitstory/{masterUnitStoryChapter.assetbundleName}_rip/{masterUnitStoryEpisode.scenarioId}.json";
                                break;
                            case FileStruct.Classic: savepath = $"{folder_Sample}/{NicknameCountData.unitStoriesFolder}/{masterUnitStoryEpisode.scenarioId}.json";
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

        DownloadFileInfo[] GetDownloadFile_Event()
        {
            MasterEventStory[] masterEventStories = EnvPath.GetTable<MasterEventStory>("eventStories");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterEventStory in masterEventStories)
            {
                foreach (var eventStoryEpisode in masterEventStory.eventStoryEpisodes)
                {
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

        DownloadFileInfo[] GetDownloadFile_Card()
        {
            MasterCardEpisode[] masterCardEpisodes = EnvPath.GetTable<MasterCardEpisode>("cardEpisodes");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterCardEpisode in masterCardEpisodes)
            {
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

        DownloadFileInfo[] GetDownloadFile_Map()
        {
            MasterActionSet[] masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterActionSet in masterActionSets)
            {
                if (masterActionSet.id <= 4) continue;
                string url = $"{SekaiViewer.AssetUrl}/scenario/actionset/group{masterActionSet.id/100}_rip/{masterActionSet.scenarioId}.asset";
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

        DownloadFileInfo[] GetDownloadFile_Live()
        {
            MasterVirtualLive[] masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
            foreach (var masterVirtualLive in masterVirtualLives)
            {
                foreach (var masterVirtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                {
                    if (masterVirtualLiveSetlist.VirtualLiveSetlistType != VirtualLiveSetlistType.mc) continue;
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

        DownloadFileInfo[] GetDownloadFile_Other()
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

    public class StoryUseCheck
    {
        Dictionary<StoryType, HashSet<string>> storyUsingStatus;

        public StoryUseCheck()
        {
            MasterUnitStory[] masterUnitStories = EnvPath.GetTable<MasterUnitStory>("unitStories");
            HashSet<string> usedStory_Unit = new HashSet<string>();
            foreach (var masterUnitStory in masterUnitStories)
            {
                foreach (var masterUnitStoryChapter in masterUnitStory.chapters)
                {
                    foreach (var masterUnitStoryEpisode in masterUnitStoryChapter.episodes)
                    {
                        usedStory_Unit.Add(masterUnitStoryEpisode.scenarioId);
                    }
                }
            }

            MasterEventStory[] masterEventStories = EnvPath.GetTable<MasterEventStory>("eventStories");
            HashSet<string> usedStory_Event = new HashSet<string>();
            foreach (var masterEventStory in masterEventStories)
            {
                foreach (var eventStoryEpisode in masterEventStory.eventStoryEpisodes)
                {
                    usedStory_Event.Add(eventStoryEpisode.scenarioId);
                }
            }

            MasterCardEpisode[] masterCardEpisodes = EnvPath.GetTable<MasterCardEpisode>("cardEpisodes");
            HashSet<string> usedStory_Card = new HashSet<string>();
            foreach (var masterCardEpisode in masterCardEpisodes)
            {
                usedStory_Card.Add(masterCardEpisode.scenarioId);
            }

            MasterActionSet[] masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            HashSet<string> usedStory_Map = new HashSet<string>();
            foreach (var masterActionSet in masterActionSets)
            {
                usedStory_Map.Add(masterActionSet.scenarioId);
            }

            MasterVirtualLive[] masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
            HashSet<string> usedStory_Live = new HashSet<string>();
            foreach (var masterVirtualLive in masterVirtualLives)
            {
                foreach (var masterVirtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                {
                    usedStory_Live.Add(masterVirtualLiveSetlist.assetbundleName);
                }
            }

            MasterSpecialStory[] masterSpecialStories = EnvPath.GetTable<MasterSpecialStory>("specialStories");
            HashSet<string> usedStory_Other = new HashSet<string>();
            for (int i = 1; i < 27; i++)
            {
                usedStory_Other.Add($"self_{(Character)i}");
            }
            foreach (var masterSpecialStory in masterSpecialStories)
            {
                foreach (var masterSpecialStoryEpisode in masterSpecialStory.episodes)
                {
                    usedStory_Other.Add(masterSpecialStoryEpisode.scenarioId);
                }
            }

            storyUsingStatus = new Dictionary<StoryType, HashSet<string>>();
            storyUsingStatus[StoryType.UnitStory] = usedStory_Unit;
            storyUsingStatus[StoryType.EventStory] = usedStory_Event;
            storyUsingStatus[StoryType.CardStory] = usedStory_Card;
            storyUsingStatus[StoryType.MapTalk] = usedStory_Map;
            storyUsingStatus[StoryType.LiveTalk] = usedStory_Live;
            storyUsingStatus[StoryType.OtherStory] = usedStory_Other;
        }

        public bool CheckIfUsed(StoryType storyType,string fileNameWithoutExtension)
        {
            if (storyType == StoryType.SystemVoice) return true;
            else return storyUsingStatus[storyType].Contains(fileNameWithoutExtension);
        }
    }
}