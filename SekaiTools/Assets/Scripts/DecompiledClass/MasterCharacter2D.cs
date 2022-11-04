// Sekai.MasterCharacter2D
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterCharacter2D
    {
        public const string CHARATER_TYPE_GAME_CHARACTER = "game_character";

        public const string CHARATER_TYPE_MOB = "mob";

        public int id;

        public string characterType;

        public int characterId;

        public string unit;

        public string assetName;

        [IgnoreMember]
        public UnitType UnitType => (UnitType)Enum.Parse(typeof(UnitType), unit);
    }
}