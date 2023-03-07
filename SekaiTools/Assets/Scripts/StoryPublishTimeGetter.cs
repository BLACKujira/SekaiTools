using SekaiTools.DecompiledClass;
using System;
using System.Collections.Generic;

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

        public long GetStoryPublishTime_Map(string name)
        {
            MasterActionSet actionSet = null;
            foreach (var masterActionSet in actionSets)
            {
                if (name.Equals(masterActionSet.scenarioId))
                {
                    actionSet = masterActionSet;
                    break;
                }
            }
            if (actionSet == null) return ConstData.GamePublishedAt;

            if (actionSet.specialSeasonId != 0)
            {
                return GetStoryPublishTime_Map_SpecialSeason(name, actionSet);
            }
            else
            {
                return GetStoryPublishTime_Map_ReleaseCondition(name, actionSet);
            }
        }

        long GetStoryPublishTime_Map_SpecialSeason(string name, MasterActionSet actionSet)
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

        long GetStoryPublishTime_Map_ReleaseCondition(string name, MasterActionSet actionSet)
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
                if (masterEventStory.id == releaseCondition.releaseConditionTypeId)
                {
                    eventStory = masterEventStory;
                    break;
                }
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