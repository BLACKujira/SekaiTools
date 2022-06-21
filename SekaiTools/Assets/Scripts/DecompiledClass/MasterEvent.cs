// Sekai.MasterEvent
using System;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    [MessagePackObject]
    public class MasterEvent : IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public string eventType;
        public string name;
        public string assetbundleName;
        public string bgmAssetbundleName;
        public string eventPointAssetbundleName;
        public long startAt;
        public long aggregateAt;
        public long rankingAnnounceAt;
        public long distributionStartAt;
        public long closedAt;
        public long distributionEndAt;
        public EventRankingRewardRange[] eventRankingRewardRanges;
        public int? virtualLiveId;
        public string unit;
        [IgnoreMember]
        public GameEventType EventType;
        [IgnoreMember]
        public DateTime StartTime;
        [IgnoreMember]
        public DateTime AggregateTime;
        [IgnoreMember]
        public DateTime EndTime;
        [IgnoreMember]
        public UnitType BonusUnitType
        {
            get
            {
                return default;
            }
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
        public MasterEvent()
        {
        }
    }
}