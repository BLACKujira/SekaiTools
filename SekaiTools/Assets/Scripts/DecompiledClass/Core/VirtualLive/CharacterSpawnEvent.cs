// Sekai.Core.VirtualLive.CharacterSpawnEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class CharacterSpawnEvent : BaseEvent, IMCEffect
    {
        public Vector3 Position;
        public int HeadCostume3dId;
        public int BodyCostume3dId;
        public float RotationY;
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