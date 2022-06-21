using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using System;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public SpineSceneEditor_Main mainArea;

        public void Initialize(SpineScene spineScene,Action<SpineScene> onClose)
        {
            window.OnClose.AddListener(() => onClose(mainArea.spineScene));
            mainArea.SetScene(spineScene);
        }
    }
}