using SekaiTools.DecompiledClass;
using SekaiTools.UI.NicknameCountShowcase;
using SekaiTools.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;

namespace SekaiTools
{
    public class GlobalData : MonoBehaviour
    {
        [Header("Built-in data")]
        public TextAsset character3ds_Inbuilt;
        public TextAsset events_Inbuilt;
        [Header("Default data")]
        public StringSet defaultSpineModels;
        [Header("ScriptableObject")]
        public NCSSceneSet nCSSceneSet;
        public TransitionSet transitionSet;
        public InbuiltSpineModelSet inbuiltSpineModelSet;

        [System.NonSerialized] public MasterCharacter3D[] character3ds;
        [System.NonSerialized] public MasterEvent[] events;

        public static GlobalData globalData;
        private void Awake()
        {
            globalData = this;

            character3ds = JsonHelper.getJsonArray<MasterCharacter3D>(character3ds_Inbuilt.text);
            events = JsonHelper.getJsonArray<MasterEvent>(events_Inbuilt.text);
        }

        public static int Character3dIdToCharacterId(int character3dId)
        {
            return globalData.character3ds[character3dId-1].characterId;
        }
    }
}