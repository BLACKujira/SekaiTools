// Sekai.Core.VirtualLive.CharacterMoveEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class CharacterMoveEvent : BaseEvent
    {
        public Vector3 Position;
        public string MoveKey;
        public float RotationY;
    }
}