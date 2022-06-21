// Sekai.Core.VirtualLive.GlobalSpotlightEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class GlobalSpotlightEvent : BaseEvent
    {
        public Vector3 CenterPosition;
        public float FadeStartRadius;
        public float FadeEndRadius;
    }
}