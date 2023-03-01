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
        [Header("Default data")]
        public StringSet defaultSpineModels;
        [Header("ScriptableObject")]
        public NCSSceneSet nCSSceneSet;
        public TransitionSet transitionSet;
        public InbuiltSpineModelSet inbuiltSpineModelSet;

        public static GlobalData globalData;
        private void Awake()
        {
            globalData = this;
        }
    }
}