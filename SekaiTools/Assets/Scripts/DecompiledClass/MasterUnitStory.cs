// Sekai.MasterUnitStory
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterUnitStory
    {
        public string unit;
        public int seq;
        public string assetbundleName;
        public MasterUnitStoryChapter[] chapters;

        [IgnoreMember]
        public UnitType UnitType
        {
            get
            {
                return (UnitType)Enum.Parse(typeof(UnitType), unit);
            }
        }

        public MasterUnitStoryChapter GetChapter(int chapterId)
        {
            return null;
        }
    }
}