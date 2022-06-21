// Sekai.Core.VirtualLive.LightEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class LightEvent : BaseEvent
    {
        public enum TargetLight
        {
            Stage,
            Character
        }

        public TargetLight Target;
        public Color LightColor;
        public Color RimColor;
        public float Intensity;
    }
}