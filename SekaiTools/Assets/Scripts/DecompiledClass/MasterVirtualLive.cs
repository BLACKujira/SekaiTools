// Sekai.MasterVirtualLive
using System;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualLive : IMessagePackSerializationCallbackReceiver
    {
        public const string V2_BANNER_PREFIX = "_v2";
        public int id;
        public int seq;
        public string name;
        public int? archiveReleaseConditionId;
        public string assetbundleName;
        public long startAt;
        public long endAt;
        public string virtualLiveType;
        public string virtualLivePlatform;
        public int screenMvMusicVocalId;
        public long rankingAnnounceAt;
        public MasterVirtualLiveSetlist[] virtualLiveSetlists;
        public MasterVirtualLiveSchedule[] virtualLiveSchedules;
        public MasterVirtualLiveBeginnerSchedule[] virtualLiveBeginnerSchedules;
        public MasterVirtualLiveCharacter[] virtualLiveCharacters;
        public MasterVirtualLiveReward[] virtualLiveRewards;
        public MasterVirtualLiveCheerPointReward[] virtualLiveCheerPointRewards;
        public VirtualLiveWaitingRoom virtualLiveWaitingRoom;
        public MasterVirtualItem[] virtualItems;
        public MasterVirtualLiveAppeal[] virtualLiveAppeals;
        public MasterVirtualLiveInformation virtualLiveInformation;
        [IgnoreMember]
        public VirtualLiveType VirtualLiveType
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public VirtualLivePlatformType VirtualLivePlatformType
        {
            get
            {
                return default;
            }
            set
            {
            }
        }

        [IgnoreMember]
        public DateTime StartTime
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public DateTime EndTime
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public DateTime RankingAnnounceTime
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public MasterVirtualLiveSchedule[] SortedVirtualLiveSchedules
        {
            get
            {
                return null;
            }
            private set
            {
            }
        }

        public void UpdateVirtualLiveType()
        {
            throw new NotImplementedException();
        }

        public string GetAppealText(string state)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialize()
        {
            throw new NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }
    }
}