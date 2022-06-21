// Sekai.Core.VirtualLive.BaseEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    public class BaseEvent
    {
        [SerializeField]
        public int Id;
        [SerializeField]
        public float Time;
        public float Duration;
        public int Character3dId;
        public string FaicialKey;
        public string MotionKey;
        public BaseEvent()
        {
        }
    }
}