// Sekai.MasterIngameCutinCharacters

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterIngameCutinCharacters
    {
        public int id;
        public int priority;
        public int characterId1;
        public int? characterId2;
        public string assetbundleName;
        public int gameCharacterUnitId1;
        public int gameCharacterUnitId2;
        public string assetbundleName1;
        public string assetbundleName2;
        public string ingameCutinCharacterType;
        public int releaseConditionId;
        public bool IsBondsCutin
        {
            get
            {
                return ingameCutinCharacterType.Equals("bonds");
            }
        }
    }
}