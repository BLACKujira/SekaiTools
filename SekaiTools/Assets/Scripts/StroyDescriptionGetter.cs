using SekaiTools.DecompiledClass;
using System.Collections.Generic;

namespace SekaiTools
{
    public class StoryDescriptionGetter
    {
        MasterArea[] masterAreas;
        MasterActionSet[] masterActionSets;
        MasterVirtualLive[] masterVirtualLives;
        MasterEvent[] masterEvents;
        MasterCard[] masterCards;

        public StoryDescriptionGetter()
        {
            masterAreas = EnvPath.GetTable<MasterArea>("areas");
            masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
            masterEvents = EnvPath.GetTable<MasterEvent>("events");
            masterCards = EnvPath.GetTable<MasterCard>("cards");
        }

        const string STR_VS = " 虚拟歌手";

        public static string[] RequireMasterTables => new string[]
            {
                "areas","actionSets","virtualLives","events","cards"
            };

        public string GetStroyDescription(StoryType storyType, string name)
        {
            switch (storyType)
            {
                case StoryType.UnitStory: return GetStroyDescription_Unit(name);
                case StoryType.EventStory: return GetStroyDescription_Event(name);
                case StoryType.CardStory: return GetStroyDescription_Card(name);
                case StoryType.MapTalk: return GetStroyDescription_Map(name);
                case StoryType.LiveTalk: return GetStroyDescription_Live(name);
                case StoryType.OtherStory: return GetStroyDescription_Other(name);
                case StoryType.SystemVoice: return "系统语言";
                default: return string.Empty;
            }
        }

        public string GetStroyDescription_Unit(string name)
        {
            UnitStoryInfo unitStoryInfo = ConstData.IsUnitStory(name);
            string valueStr;
            if (unitStoryInfo != null)
            {
                string unitStr = string.Empty;
                switch (unitStoryInfo.unit)
                {
                    case "leo": unitStr = ConstData.units[Unit.Leoneed].name; break;
                    case "mmj": unitStr = ConstData.units[Unit.MOREMOREJUMP].name; break;
                    case "street": unitStr = ConstData.units[Unit.VividBADSQUAD].name; break;
                    case "wonder": unitStr = ConstData.units[Unit.WonderlandsXShowtime].name; break;
                    case "nightcode": unitStr = ConstData.units[Unit.NightCord].name; break;
                    case "vs": unitStr = ConstData.units[Unit.VirtualSinger].name; break;
                    case "vsleo": unitStr = ConstData.units[Unit.Leoneed].name + STR_VS; break;
                    case "vsmmj": unitStr = ConstData.units[Unit.MOREMOREJUMP].name + STR_VS; break;
                    case "vsstreet": unitStr = ConstData.units[Unit.VividBADSQUAD].name + STR_VS; break;
                    case "vswonder": unitStr = ConstData.units[Unit.WonderlandsXShowtime].name + STR_VS; break;
                    case "vsnightcode": unitStr = ConstData.units[Unit.NightCord].name + STR_VS; break;
                    default: unitStr = "无法识别的组合"; break;
                }
                valueStr = $"主线剧情 {unitStr} 第{unitStoryInfo.chapter}话 [{name}]";
            }
            else
            {
                valueStr = $"未知主线剧情 [{name}]";
            }
            return valueStr;
        }

        public string GetStroyDescription_Event(string name)
        {
            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(name);
            string valueStr;
            if (eventStoryInfo != null)
            {
                string evStr = null;
                foreach (var masterEvent in masterEvents)
                {
                    if (masterEvent.id == eventStoryInfo.eventId)
                    {
                        evStr = masterEvent.name;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(evStr))
                    valueStr = $"未知活动 第{eventStoryInfo.eventId}期 第{eventStoryInfo.chapter}话 [{name}]";
                else
                    valueStr = $"第{eventStoryInfo.eventId}期活动 {evStr} 第{eventStoryInfo.chapter}话 [{name}]";
            }
            else
            {
                valueStr = $"未知活动剧情 [{name}]";
            }
            return valueStr;
        }

        public string GetStroyDescription_Card(string name)
        {
            CardStoryInfo cardStoryInfo = ConstData.IsCardStory(name);
            MasterCard card = null;
            string valueStr;
            if (cardStoryInfo != null)
            {
                string assetbundleName = $"res{cardStoryInfo.charId.ToString("000")}_no{cardStoryInfo.cardId.ToString("000")}";
                foreach (var masterCard in masterCards)
                {
                    if (masterCard.assetbundleName.Equals(assetbundleName))
                    {
                        card = masterCard;
                        break;
                    }
                }
            }
            if (card != null)
                valueStr = $"卡片剧情 {ConstData.characters[cardStoryInfo.charId].Name} {card.prefix} {(cardStoryInfo.chapter == 1 ? "前篇" : "后篇")} [{name}]";
            else
                valueStr = $"未知卡片剧情 [{name}]";
            return valueStr;
        }

        public string GetStroyDescription_Map(string name)
        {
            MasterActionSet actionSet = null;
            MasterArea area = null;
            string valueStr;
            foreach (var masterActionSet in masterActionSets)
            {
                if (name.Equals(masterActionSet.scenarioId))
                {
                    actionSet = masterActionSet;
                    break;
                }
            }

            if (actionSet != null)
            {
                foreach (var masterArea in masterAreas)
                {
                    if (masterArea.id == actionSet.areaId)
                    {
                        area = masterArea;
                        break;
                    }
                }
                List<string> charNames = new List<string>();
                foreach (var charId in actionSet.characterIds)
                {
                    int mergedId = ConstData.MergeVirtualSinger(charId);
                    if (mergedId >= 1 && mergedId <= 26)
                    {
                        charNames.Add(ConstData.characters[mergedId].namae);
                    }
                    else
                    {
                        charNames.Add($"未知角色{mergedId}");
                    }
                }

                valueStr = $"地图对话 {string.Join("、", charNames)} 在{(area == null ? "未知区域" : area.name)} [{name}]";
            }
            else
            {
                valueStr = $"未知区域对话 [{name}]";
            }
            return valueStr;
        }

        public string GetStroyDescription_Live(string name)
        {
            string valueStr;
            MasterVirtualLive virtualLive = null;
            foreach (var masterVirtualLive in masterVirtualLives)
            {
                foreach (var virtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                {
                    if (virtualLiveSetlist == null || virtualLiveSetlist.assetbundleName == null) continue;
                    bool flag = false;
                    if (virtualLiveSetlist.assetbundleName.Equals(name))
                    {
                        virtualLive = masterVirtualLive;
                        flag = true;
                        break;
                    }
                    if (flag) break;
                }
            }

            if (virtualLive != null)
                valueStr = $"Live对话 {virtualLive.name} [{name}]";
            else
                valueStr = $"未知Live对话 [{name}]";
            return valueStr;
        }

        public string GetStroyDescription_Other(string name)
        {
            string valueStr;
            valueStr = $"其他剧情 [{name}]";
            return valueStr;
        }
    }
}