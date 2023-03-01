// Sekai.MasterEventStory

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterEventStory
    {
        public int id;
        public int eventId;
        public string assetbundleName;
        public EventStoryEpisode[] eventStoryEpisodes;

        public EventStoryEpisode GetEpisode(int episodeId) => eventStoryEpisodes[episodeId];
    }
}