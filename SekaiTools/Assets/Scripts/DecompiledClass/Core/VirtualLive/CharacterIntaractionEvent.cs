// Sekai.Core.VirtualLive.CharacterIntaractionEvent
using System;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class CharacterIntaractionEvent : BaseEvent
    {
        public enum InteractionActionType
        {
            PenlightSwing
        }

        public InteractionActionType InteractionType;
    }
}