// Sekai.MasterCharacter3D
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class Character3ds
    {
        public MasterCharacter3D[] masterCharacter3Ds;
    }

    [Serializable]
    public class MasterCharacter3D
    {
        public int id;
        public string characterType;
        public int characterId;
        public string name;
        public int headCostume3dId;
        public int hairCostume3dId;
        public int bodyCostume3dId;
        public string unit;
    }
}