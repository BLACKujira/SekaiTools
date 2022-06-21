// Sekai.Core.VirtualLive.AisacEvent
using SekaiTools.DecompiledClass;
using System;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class AisacEvent : BaseEvent
    {
        public AisacKey AisacKey;

        public float Volume;

    }
}