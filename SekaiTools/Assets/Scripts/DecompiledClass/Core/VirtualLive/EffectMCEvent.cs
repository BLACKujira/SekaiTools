// Sekai.Core.VirtualLive.EffectMCEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class EffectMCEvent : BaseEvent, IMCEffect
    {
        [SerializeField]
        private string effectKey;
        public string EffectKey
        {
            get
            {
                return effectKey;
            }
            set
            {
                effectKey = value;
            }
        }
    }
}