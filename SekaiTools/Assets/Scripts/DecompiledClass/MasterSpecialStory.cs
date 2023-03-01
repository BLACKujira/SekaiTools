// Sekai.MasterSpecialStory

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterSpecialStory
    {
        public int id;
        public int seq;
        public string assetbundleName;
        public MasterSpecialStoryEpisode[] episodes;
        public long startAt;
        public long endAt;

        public MasterSpecialStoryEpisode GetEpisode(int episodeId)
        {
            return null;
        }

        public MasterSpecialStoryEpisode GetEpisodeByEpisodeNo(int episodeNo)
        {
            return null;
        }
    }
}