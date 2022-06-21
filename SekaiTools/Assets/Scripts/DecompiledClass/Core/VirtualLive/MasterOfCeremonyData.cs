// Sekai.Core.VirtualLive.MasterOfCeremonyData
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.DecompiledClass.Core.VirtualLive
{
    [Serializable]
    public class MasterOfCeremonyData
    {
        public string Id;
        public CharacterSpawnEvent[] characterSpawnEvents;
        public CharacterUnspawnEvent[] characterUnspawnEvents;
        public CharacterMoveEvent[] characterMoveEvents;
        public CharacterRotateEvent[] characterRotateEvents;
        public CharacterMotionEvent[] characterMotionEvents;
        public CharacterTalkEvent[] characterTalkEvents;
        public CharacterIntaractionEvent[] characterIntaractionEvents;
        public EffectMCEvent[] effectMCEvents;
        public LightEvent[] lightEvents;
        public SoundEvent[] soundEvents;
        public BGMEvent[] bgmEvents;
        public AudienceEvent[] audienceEvents;
        public StageObjectSpawnEvent[] stageObjectSpawnEvents;
        public GlobalSpotlightEvent[] globalSpotlightEvents;
        public AisacEvent[] aisacEvents;
        public ScreenFadeEvent[] screenFadeEvents;
        public static MasterOfCeremonyData Create(string id)
        {
            return null;
        }
        public BaseEvent[] GetEventArray()
        {
            return null;
        }
        private void AddList(List<BaseEvent> list, BaseEvent[] baseEvents)
        {
        }
        public MasterOfCeremonyData()
        {
        }
    }
}