// Sekai.MasterSystemLive2D
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterSystemLive2D
    {
        public int id;
        public int characterId;
        public string unit;
        public string serif;
        public string assetbundleName;
        public string voice;
        public string motion;
        public string expression;
        public long publishedAt;
        public long closedAt;
        public int weight;

        public UnitType UnitType
        {
            get
            {
                return (UnitType)Enum.Parse(typeof(UnitType),unit);
            }
        }
    }
}