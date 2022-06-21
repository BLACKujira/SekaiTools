// Sekai.Core.VirtualLive.BGMEvent
using System;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class BGMEvent : BaseEvent
    {
        public enum BGMPlayState
        {
            ON,
            OFF
        }
        public string BGMKey;
        public BGMPlayState Play;
    }
}