// Sekai.MasterUnitStoryEpisode
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterUnitStoryEpisode : IMasterStoryEpisode, IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public string unit;
        public string unitEpisodeCategory;
        public int chapterNo;
        public int episodeNo;
        public string episodeNoLabel;
        public string title;
        public string assetbundleName;
        public string scenarioId;
        public int releaseConditionId;
        public int andReleaseConditionId;
        public int[] rewardResourceBoxIds;
        public long limitedReleaseStartAt;
        public long limitedReleaseEndAt;

        [IgnoreMember]
        public UnitType UnitType
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public UnitEpisodeCategoryType UnitEpisodeCategory
        {
            get
            {
                return default;
            }
            set
            {
            }
        }

        [IgnoreMember]
        public int Id
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public int ChapterNo
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public int EpisodeNo
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public string Title
        {
            get
            {
                return null;
            }
        }

        [IgnoreMember]
        public string AssetbundleName
        {
            get
            {
                return null;
            }
        }

        [IgnoreMember]
        public string ScenarioId
        {
            get
            {
                return null;
            }
        }

        [IgnoreMember]
        public int ReleaseConditionId
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public int[] RewardResourceBoxIds
        {
            get
            {
                return null;
            }
        }

        [IgnoreMember]
        public StoryType StoryType
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public long LimitedReleaseStartAt
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public long LimitedReleaseEndAt
        {
            get
            {
                return default;
            }
        }

        public void OnAfterDeserialize()
        {
            throw new System.NotImplementedException();
        }

        public void OnBeforeSerialize()
        {
            throw new System.NotImplementedException();
        }
    }
}