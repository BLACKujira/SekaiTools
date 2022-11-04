// Sekai.MasterBondsHonor
using System;
using System.Collections.Generic;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterBondsHonor
    {
        public int id;
        public int seq;
        public int bondsGroupId;
        public int gameCharacterUnitId1;
        public int gameCharacterUnitId2;
        public string honorRarity;
        public List<MasterBondsHonorLevel> levels;
        public string name;
        public string description;

        [IgnoreMember]
        public HonorRarity Rarity
        {
            get
            {
                return (HonorRarity)Enum.Parse(typeof(HonorRarity),honorRarity);
            }
        }

        [IgnoreMember]
        public bool ExistLevels
        {
            get
            {
                return default;
            }
        }

        public string GetLevelDescription(int level)
        {
            return null;
        }
    }
}