// Sekai.MasterMusicVocal
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterMusicVocal
    {
        public int id;
        public int musicId;
        public string musicVocalType;
        public int seq;
        public int releaseConditionId;
        public string caption;
        public MasterMusicVocalCharacter[] characters;
        public string assetbundleName;

        [IgnoreMember]
        public MusicVocalType MusicVocalType => (MusicVocalType)Enum.Parse(typeof(MusicVocalType), musicVocalType);
    }
}