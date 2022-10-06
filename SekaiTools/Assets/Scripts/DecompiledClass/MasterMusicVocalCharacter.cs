// Sekai.MasterMusicVocalCharacter
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterMusicVocalCharacter
    {
        public int id;
        public int musicId;
        public int musicVocalId;
        public string characterType;
        public int characterId;
        public int seq;

        [IgnoreMember]
        public CharacterType CharacterType => (CharacterType)Enum.Parse(typeof(CharacterType),characterType);
    }
}