// Sekai.EventStoryEpisode

using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class EventStoryEpisode : IMasterStoryEpisode, IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public int eventStoryId;
        public int episodeNo;
        public string title;
        public string assetbundleName;
        public string scenarioId;
        public int releaseConditionId;

        public EpisodeReward[] episodeRewards;
        public int Id
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

        public int ChapterNo
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