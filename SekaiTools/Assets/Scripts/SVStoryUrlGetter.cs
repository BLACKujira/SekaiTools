using SekaiTools.DecompiledClass;
using SekaiTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using static SekaiTools.ConstData;

namespace SekaiTools
{
    /// <summary>
    /// 用于获得一篇剧情在SEKAI VIEWER上的URL
    /// </summary>
    public class SVStoryUrlGetter
    {
        const string STORY_READER = "storyreader";

        MasterCardEpisode[] cardEpisodes;
        MasterCard[] cards;
        MasterSpecialStory[] specialStories;
        MasterActionSet[] masterActionSets;
        MasterVirtualLive[] masterVirtualLives;

        public static string[] RequireMasterTables => new string[]
        {
            "cardEpisodes", "cards","specialStories","actionSets","virtualLives"
        };

        public SVStoryUrlGetter()
        {
            cardEpisodes = EnvPath.GetTable<MasterCardEpisode>("cardEpisodes");
            cards = EnvPath.GetTable<MasterCard>("cards");
            specialStories = EnvPath.GetTable<MasterSpecialStory>("specialStories");
            masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
        }

        public string GetUrl(StoryType storyType, string fileName)
        {
            switch(storyType)
            {
                case StoryType.UnitStory: return GetUrl_UnitStory(fileName);
                case StoryType.EventStory: return GetUrl_EventStory(fileName);
                case StoryType.CardStory: return GetUrl_CardStory(fileName);
                case StoryType.MapTalk: return GetUrl_MapTalk(fileName);
                case StoryType.LiveTalk: return GetUrl_LiveTalk(fileName);
                case StoryType.OtherStory: return GetUrl_OtherStory(fileName);
            }

            WindowController.ShowMessage("错误", fileName + ": 未知的剧情类型");
            return null;
        }

        public string GetUrl_UnitStory(string fileName)
        {
            UnitStoryInfo unitStoryInfo = ConstData.IsUnitStory(fileName);
            if(unitStoryInfo == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不是一个有效的组合剧情文件名");
                return null;
            }

            string unitStr = null;
            switch(unitStoryInfo.unit)
            {
                case "leo": unitStr = "light_sound"; break;
                case "mmj": unitStr = "idol"; break;
                case "street": unitStr = "street"; break;
                case "wonder": unitStr = "theme_park"; break;
                case "nightcode": unitStr = "school_refusal"; break;
                case "vsleo": unitStr = "piapro"; break;
                case "vsmmj": unitStr = "piapro"; break;
                case "vsstreet": unitStr = "piapro"; break;
                case "vswonder": unitStr = "piapro"; break;
                case "vsnightcode": unitStr = "piapro"; break;
            }

            if(string.IsNullOrEmpty(unitStr))
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的组合名称");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/unitStory/{unitStr}/{unitStoryInfo.season}/{unitStoryInfo.chapter}";
        }

        public string GetUrl_EventStory(string fileName)
        {
            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(fileName);
            if(eventStoryInfo == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不是一个有效的活动剧情文件名");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/eventStory/{eventStoryInfo.eventId}/{eventStoryInfo.chapter}";
        }

        public string GetUrl_CardStory(string fileName)
        {
            CardStoryInfo cardStoryInfo = ConstData.IsCardStory(fileName);
            if(cardStoryInfo == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不是一个有效的卡片剧情文件名");
                return null;
            }

            MasterCard masterCard = cards
                .Where(c => c.assetbundleName.Equals(fileName))
                .FirstOrDefault();

            if(masterCard == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的卡片");
                return null;
            }

            MasterCardEpisode masterCardEpisode = cardEpisodes
                .Where(ce => ce.cardId == masterCard.id && ce.seq == cardStoryInfo.chapter)
                .FirstOrDefault();

            if(masterCardEpisode == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的卡片剧情");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/cardStory/{cardStoryInfo.cardId}/{masterCardEpisode.id}";
        }

        public string GetUrl_MapTalk(string fileName)
        {
            MasterActionSet masterActionSet = masterActionSets
                .Where(a => a.scenarioId.Equals(fileName))
                .FirstOrDefault();
        
            if(masterActionSet == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的地图剧情");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/areaTalk/{masterActionSet.areaId}/{masterActionSet.id}";
        }

        public string GetUrl_LiveTalk(string fileName)
        {
            var live = masterVirtualLives
                .SelectMany(vl => vl.virtualLiveSetlists.Select(vls => (vl, vls)))
                .Where(t => t.vls.virtualLiveSetlistType.Equals("mc") && t.vls.assetbundleName.Equals(fileName))
                .FirstOrDefault();

            if(live.vl == null || live.vls == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的虚拟LIVE");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/virtual_live/{live.vl.id}";
        }

        public string GetUrl_OtherStory(string fileName)
        {
            var episode = specialStories
                .SelectMany(ss => ss.episodes.Select(sse => (ss,sse)))
                .Where(t => t.sse.assetbundleName.Equals(fileName))
                .FirstOrDefault();

            if(episode.ss == null || episode.sse == null)
            {
                WindowController.ShowMessage("错误", fileName + ": 不存在的特殊剧情");
                return null;
            }

            return $"{SekaiViewerInterface.Env.SEKAI_VIEWER}/{STORY_READER}/specialStory/{episode.ss.id}/{episode.sse.episodeNo}";
        }
    }
}
