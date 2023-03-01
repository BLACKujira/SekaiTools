// Sekai.MasterReleaseCondition
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterReleaseCondition
    {
        public const int MUSIC_SHOP_EXCHANGE_ID = 5;
        public int id;
        public string sentence;
        public string releaseConditionType;
        public int releaseConditionTypeId;
        public int releaseConditionTypeLevel;
        public int releaseConditionTypeQuantity;

        [IgnoreMember]
        public ReleaseConditionType ReleaseConditionType => (ReleaseConditionType)Enum.Parse(typeof(ReleaseConditionType), releaseConditionType);
    }
}