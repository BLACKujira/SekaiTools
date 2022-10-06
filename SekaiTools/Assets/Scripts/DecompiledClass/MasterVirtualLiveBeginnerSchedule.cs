// Sekai.MasterVirtualLiveBeginnerSchedule
using System;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualLiveBeginnerSchedule : IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public int virtualLiveId;
        public string dayOfWeek;
        public string startTime;
        public string endTime;
        [IgnoreMember]
        public DayOfWeek DayOfWeek
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public TimeSpan StartTimeSpan
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
        public TimeSpan EndTimeSpan
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
        public DateTime FirstLiveStartTime
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public DateTime FirstLiveEndTime
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public long FirstLiveStartTimeStamp
        {
            get
            {
                return default;
            }
        }

        [IgnoreMember]
        public long FirstLiveEndTimeStamp
        {
            get
            {
                return default;
            }
        }

        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialize()
        {
            throw new NotImplementedException();
        }
    }
}