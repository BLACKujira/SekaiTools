// Sekai.MasterMusic
using System;
using System.Collections.Generic;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterMusic
    {
        public int id;
        public int seq;
        public int releaseConditionId;
        public string title;
        public string[] categories;
        public string lyricist;
        public string composer;
        public string arranger;
        public int dancerCount;
        public int selfDancerPosition;
        public string assetbundleName;
        public string liveTalkBackgroundAssetbundleName;
        public string description;
        public long publishedAt;
        public int liveStageId;
        public float fillerSec;
        public MusicCategory[] Categories
        {
            get
            {
                MusicCategory[] musicCategories = new MusicCategory[categories.Length];
                for (int i = 0; i < categories.Length; i++)
                {
                    musicCategories[i] = (MusicCategory)Enum.Parse(typeof(MusicCategory),categories[i]);
                }
                return musicCategories;
            }
        }
    }
}