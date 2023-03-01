using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.UI.GenericInitializationParts;

namespace SekaiTools.UI.CutinSceneEditorInitialize
{
    public class CutinSceneEditorInitialize_Step1 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_CSDSaveData gIP_CSDSaveData;
        [Header("Prefab")]
        public Window step2WindowPrefab;

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_CSDSaveData);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            CutinSceneEditorInitialize_Step2 cutinSceneEditorInitialize_Step2
                = window.OpenWindow<CutinSceneEditorInitialize_Step2>(step2WindowPrefab);
            cutinSceneEditorInitialize_Step2.Initialize(gIP_CSDSaveData.CutinSceneData);
        }
    }
}