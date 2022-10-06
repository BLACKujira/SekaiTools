// Sekai.MasterVirtualLiveReward
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualLiveReward
    {
        public int id;
        public int virtualLiveId;
        public int resourceBoxId;
        public string virtualLiveType;
        [IgnoreMember]
        public VirtualLiveType VirtualLiveType
        {
            get
            {
                return default;
            }
        }
    }
}