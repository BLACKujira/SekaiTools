using SekaiTools.DecompiledClass;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SekaiTools
{
    public class StoryPublishTimeGetter
    {
        MasterEvent[] events;
        MasterEventStory[] eventStories;
        MasterEventCard[] eventCards;
        MasterReleaseCondition[] releaseConditions;
        MasterCard[] cards;
        MasterActionSet[] actionSets;
        MasterSpecialSeason[] specialSeasons;
        MasterVirtualLive[] virtualLives;
        MasterSpecialStory[] specialStories;

        BundleRoot bundleRoot = null;
        Dictionary<string, int> bundleRootIndexDictionary = null;

        Regex evIconBundleRegex = new Regex("(?<=event/)event_[a-z]+_\\d\\d\\d\\d(?=/icon)");

        Dictionary<MasterActionSet, long> presumedPublishTime_Map = new Dictionary<MasterActionSet, long>();

        public static HashSet<string> RequireMasterTables => new HashSet<string>()
            {
                "events","eventStories","eventCards","releaseConditions",
                "actionSets","specialSeasons","virtualLives","cards",
                "specialStories"
            };

        public StoryPublishTimeGetter()
        {
            events = EnvPath.GetTable<MasterEvent>("events");
            eventStories = EnvPath.GetTable<MasterEventStory>("eventStories");
            eventCards = EnvPath.GetTable<MasterEventCard>("eventCards");
            releaseConditions = EnvPath.GetTable<MasterReleaseCondition>("releaseConditions");
            actionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
            specialSeasons = EnvPath.GetTable<MasterSpecialSeason>("specialSeasons");
            virtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
            cards = EnvPath.GetTable<MasterCard>("cards");
            specialStories = EnvPath.GetTable<MasterSpecialStory>("specialStories");
        }

        public void TurnOnMapTalkPTPresume(BundleRoot bundleRoot)
        {
            this.bundleRoot = bundleRoot;
            bundleRootIndexDictionary = new Dictionary<string, int>();
            for (int i = 0; i < bundleRoot.bundles.Count; i++)
            {
                bundleRootIndexDictionary[bundleRoot.bundles[i].bundleName] = i;
            }
        }

        public long GetStoryPublishTime(StoryType storyType, string name)
        {
            switch (storyType)
            {
                case StoryType.UnitStory: return ConstData.GamePublishedAt;
                case StoryType.EventStory: return GetStoryPublishTime_Event(name);
                case StoryType.CardStory: return GetStoryPublishTime_Card(name);
                case StoryType.MapTalk: return GetStoryPublishTime_Map(name);
                case StoryType.LiveTalk: return GetStoryPublishTime_Live(name);
                case StoryType.OtherStory: return GetStoryPublishTime_Other(name);
                case StoryType.SystemVoice: throw new NotImplementedException();
                default: return ConstData.GamePublishedAt;
            }
        }

        public long GetStoryPublishTime_Event(string name)
        {
            long publishTime = ConstData.GamePublishedAt;
            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(name);
            if (eventStoryInfo == null) return publishTime;
            foreach (var masterEvent in events)
            {
                if (masterEvent.id == eventStoryInfo.eventId)
                    return masterEvent.startAt;
            }
            return publishTime;
        }

        public long GetStoryPublishTime_Card(string name)
        {
            CardStoryInfo cardStoryInfo = ConstData.IsCardStory(name);
            if (cardStoryInfo == null) return ConstData.GamePublishedAt;

            string assetbundleName = cardStoryInfo.AssetbundleName;

            MasterCard card = null;
            foreach (var masterCard in cards)
            {
                if (masterCard.assetbundleName.Equals(assetbundleName))
                {
                    card = masterCard;
                    break;
                }
            }

            MasterEventCard eventCard = null;
            foreach (var masterEventCard in eventCards)
            {
                if (masterEventCard.cardId == cardStoryInfo.cardId)
                {
                    eventCard = masterEventCard;
                    break;
                }
            }

            if (card != null && eventCard == null)
                return card.releaseAt < ConstData.GamePublishedAt ? ConstData.GamePublishedAt : card.releaseAt;

            MasterEvent ev = null;
            foreach (var masterEvent in events)
            {
                if (masterEvent.id == eventCard.eventId)
                {
                    ev = masterEvent;
                    break;
                }
            }

            if (card != null && ev == null)
                return card.releaseAt < ConstData.GamePublishedAt ? ConstData.GamePublishedAt : card.releaseAt;

            if (ev == null) return ConstData.GamePublishedAt;

            return ev.startAt;
        }

        long GetStoryPublishTime_Map(string name)
        {
            if (string.IsNullOrEmpty(name)) return ConstData.GamePublishedAt;

            long spTime = GetStoryPublishTime_Map_Special(name);
            if (spTime!=ConstData.GamePublishedAt) return spTime;

            MasterActionSet actionSet = null;
            foreach (var aset in actionSets)
            {
                if (!string.IsNullOrEmpty(aset.scenarioId) && aset.scenarioId.Equals(name))
                {
                    actionSet = aset;
                    break;
                }
            }

            if (actionSet == null) return ConstData.GamePublishedAt;

            long startAt;
            if (actionSet.specialSeasonId != 0)
            {
                startAt = GetStoryPublishTime_Map_SpecialSeason(actionSet);
            }
            else
            {
                startAt = GetStoryPublishTime_Map_ReleaseCondition(actionSet);
                if (startAt == ConstData.GamePublishedAt && bundleRoot != null)
                {
                    startAt = GetStoryPublishTime_Map_Presume_BundleList(actionSet);
                }
            }
            return startAt;
        }

        long GetStoryPublishTime_Map_SpecialSeason(MasterActionSet actionSet)
        {
            MasterSpecialSeason specialSeason = null;
            foreach (var masterSpecialSeason in specialSeasons)
            {
                if (masterSpecialSeason.id == actionSet.specialSeasonId)
                {
                    specialSeason = masterSpecialSeason;
                    break;
                }
            }
            if (specialSeason == null) return ConstData.GamePublishedAt;
            return actionSet.archivePublishedAt < ConstData.GamePublishedAt ? ConstData.GamePublishedAt : actionSet.archivePublishedAt;
        }

        long GetStoryPublishTime_Map_ReleaseCondition(MasterActionSet actionSet)
        {
            MasterReleaseCondition releaseCondition = null;
            long asPublishedAt = actionSet.archivePublishedAt < ConstData.GamePublishedAt ? ConstData.GamePublishedAt : actionSet.archivePublishedAt;
            foreach (var masterReleaseCondition in releaseConditions)
            {
                if (masterReleaseCondition.id == actionSet.releaseConditionId)
                {
                    releaseCondition = masterReleaseCondition;
                    break;
                }
            }
            if (releaseCondition == null || releaseCondition.ReleaseConditionType != ReleaseConditionType.event_story) return asPublishedAt;

            MasterEventStory eventStory = null;
            foreach (var masterEventStory in eventStories)
            {
                bool flag = false;
                foreach (var eventStoryEpisode in masterEventStory.eventStoryEpisodes)
                {
                    if (eventStoryEpisode.id == releaseCondition.releaseConditionTypeId)
                    {
                        eventStory = masterEventStory;
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }
            if (eventStory == null) return asPublishedAt;

            MasterEvent ev = null;
            foreach (var masterEvent in events)
            {
                if (masterEvent.id == eventStory.eventId)
                {
                    ev = masterEvent;
                    break;
                }
            }
            if (ev == null) return asPublishedAt;

            return ev.startAt;
        }

        long GetStoryPublishTime_Map_Presume_BundleList(MasterActionSet actionSet)
        {
            string bundleName = $"sound/actionset/voice/{actionSet.scenarioId}";
            if (!bundleRootIndexDictionary.ContainsKey(bundleName)) return ConstData.GamePublishedAt;

            int index = bundleRootIndexDictionary[bundleName];
            while (index > 0)
            {
                index--;
                Match match = evIconBundleRegex.Match(bundleRoot.bundles[index].bundleName);
                if (match.Success)
                {
                    foreach (var ev in events)
                    {
                        if (ev.assetbundleName.Equals(match.Value))
                            return ev.startAt;
                    }
                    return ConstData.GamePublishedAt;
                }
            }

            return ConstData.GamePublishedAt;
        }

        Regex af2022TalkRegex = new Regex("areatalk_aprilfool2022_\\d\\d\\d");
        long GetStoryPublishTime_Map_Special(string name)
        {
            long startAt;
            startAt = ConstData.GamePublishedAt;
            if (af2022TalkRegex.IsMatch(name)) startAt = 1648738800000;
            return startAt;
        }

        public long GetStoryPublishTime_Live(string name)
        {
            MasterVirtualLive virtualLive = null;
            foreach (var masterVirtualLive in virtualLives)
            {
                bool flag = false;
                foreach (var masterVirtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                {
                    if (masterVirtualLiveSetlist == null || masterVirtualLiveSetlist.assetbundleName == null) continue;
                    if (masterVirtualLiveSetlist.assetbundleName.Equals(name))
                    {
                        virtualLive = masterVirtualLive;
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }
            if (virtualLive == null) return ConstData.GamePublishedAt;

            return virtualLive.startAt;
        }

        public long GetStoryPublishTime_Other(string name)
        {
            MasterSpecialStory specialStory = null;
            foreach (var masterSpecialStory in specialStories)
            {
                bool flag = false;
                foreach (var masterSpecialStoryEpisode in masterSpecialStory.episodes)
                {
                    if (masterSpecialStoryEpisode.scenarioId.Equals(name))
                    {
                        specialStory = masterSpecialStory;
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }
            if (specialStory == null) return ConstData.GamePublishedAt;
            return specialStory.startAt < ConstData.GamePublishedAt ? ConstData.GamePublishedAt : specialStory.startAt;
        }
    }
}