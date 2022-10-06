// Sekai.MasterVirtualItem
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualItem
    {
        public int id;
        public string virtualItemCategory;
        public int seq;
        public int priority;
        public string name;
        public string assetbundleName;
        public int costVirtualCoin;
        public int costJewel;
        public int cheerPoint;
        public string effectAssetbundleName;
        public string effectExpressionType;
        public string virtualItemLabelType;
        public long startAt;
        public long? endAt;
        [IgnoreMember]
        public VirtualItemCategory VirtualItemCategory
        {
            get
            {
                return default;
            }
        }
        [IgnoreMember]
        public EffectExpressionType EffectExpressionType
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public VirtualItemLabelType VirtualItemLabelType
        {
            get
            {
                return default;
            }
        }
    }
}