// Sekai.VirtualLiveWaitingRoom
using System;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class VirtualLiveWaitingRoom : IMessagePackSerializationCallbackReceiver
    {
        public int id;
        public int virtualLiveId;
        public string assetbundleName;
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