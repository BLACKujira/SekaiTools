// Sekai.MasterVirtualLiveSchedule
using System;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualLiveSchedule : IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public int virtualLiveId;
        public int seq;
        public long startAt;
        public long endAt;
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