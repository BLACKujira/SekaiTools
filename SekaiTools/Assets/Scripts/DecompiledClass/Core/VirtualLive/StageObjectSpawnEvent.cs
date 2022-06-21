// Sekai.Core.VirtualLive.StageObjectSpawnEvent
using System;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class StageObjectSpawnEvent : BaseEvent
    {
        public string ObjectKey;
        public Vector3 Position;
        public float Rotation;
    }
}