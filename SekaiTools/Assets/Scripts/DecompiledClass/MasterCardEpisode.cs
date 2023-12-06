// Sekai.MasterCardEpisode

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterCardEpisode
    {
        public int id;
        public int seq;
        public int cardId;
        public string title;
        public string scenarioId;
        public string assetbundleName;
        public int releaseConditionId;
        public int power1BonusFixed;
        public int power2BonusFixed;
        public int power3BonusFixed;
        public int[] rewardResourceBoxIds;
        public UserResource[] costs;
        public string cardEpisodePartType;

        public CardEpisodePartType CardEpisodePartType
        {
            get
            {
                return default;
            }
        }
    }
}