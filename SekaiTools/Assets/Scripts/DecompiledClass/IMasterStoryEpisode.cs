// Sekai.IMasterStoryEpisode

namespace SekaiTools.DecompiledClass
{
    public interface IMasterStoryEpisode
    {
        int Id
        {
            get;
        }

        int ChapterNo
        {
            get;
        }

        int EpisodeNo
        {
            get;
        }

        string Title
        {
            get;
        }

        string AssetbundleName
        {
            get;
        }

        string ScenarioId
        {
            get;
        }

        int ReleaseConditionId
        {
            get;
        }

        StoryType StoryType
        {
            get;
        }

        int[] RewardResourceBoxIds
        {
            get;
        }

        long LimitedReleaseStartAt
        {
            get;
        }

        long LimitedReleaseEndAt
        {
            get;
        }
    }
}