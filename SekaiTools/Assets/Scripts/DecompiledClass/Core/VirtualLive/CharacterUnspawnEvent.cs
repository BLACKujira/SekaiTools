// Sekai.Core.VirtualLive.CharacterUnspawnEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class CharacterUnspawnEvent : BaseEvent, IMCEffect
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