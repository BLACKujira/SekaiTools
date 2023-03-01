// Sekai.MasterSpecialStoryEpisode

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterSpecialStoryEpisode : IMasterStoryEpisode
    {
        public enum BehaviourType
        {
            none,
            area
        }

        public int id;
        public int specialStoryId;
        public int episodeNo;
        public string title;
        public string assetbundleName;
        public string scenarioId;
        public int releaseConditionId;
        public int[] rewardResourceBoxIds;
        public bool isAbleSkip;
        public string specialStoryEpisodeType;
        public int specialStoryEpisodeTypeId;

        public BehaviourType SpecialStoryEpisodeType
        {
            get
            {
                return default;
            }
        }

        public int Id
        {
            get
            {
                return default;
            }
        }

        public int ChapterNo
        {
            get
            {
                return default;
            }
        }

        public int EpisodeNo
        {
            get
            {
                return default;
            }
        }

        public string Title
        {
            get
            {
                return null;
            }
        }

        public string AssetbundleName
        {
            get
            {
                return null;
            }
        }

        public string ScenarioId
        {
            get
            {
                return null;
            }
        }

        public int ReleaseConditionId
        {
            get
            {
                return default;
            }
        }

        public int[] RewardResourceBoxIds
        {
            get
            {
                return null;
            }
        }

        public StoryType StoryType
        {
            get
            {
                return default;
            }
        }

        public long LimitedReleaseStartAt
        {
            get
            {
                return default;
            }
        }

        public long LimitedReleaseEndAt
        {
            get
            {
                return default;
            }
        }
    }
}