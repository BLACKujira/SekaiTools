// Sekai.MasterUnitStoryChapter
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterUnitStoryChapter
    {
        public string unit;
        public int chapterNo;
        public string title;
        public string assetbundleName;
        public MasterUnitStoryEpisode[] episodes;

        [IgnoreMember]
        public UnitType UnitType
        {
            get
            {
                return (UnitType)Enum.Parse(typeof(UnitType), unit);
            }
        }

        public MasterUnitStoryEpisode GetEpisode(int episodeId)
        {
            return null;
        }

        public MasterUnitStoryEpisode GetEpisodeByEpisodeNo(int episodeNo)
        {
            return null;
        }
    }
}