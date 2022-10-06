// Sekai.MasterArea
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterArea
    {
        public const string AREA_TYPE_REAL = "reality_world";
        public const string AREA_TYPE_SPIRIT = "spirit_world";
        public int id;
        public int seq;
        public string assetbundleName;
        public string areaType;
        public string viewType;
        public string name;
        public int releaseConditionId;
        public string label;
        public long startAt;
        public long endAt;

        [IgnoreMember]
        public AreaViewType ViewType
        {
            get
            {
                return (AreaViewType)Enum.Parse(typeof(AreaViewType),viewType);
            }
        }
    }
}