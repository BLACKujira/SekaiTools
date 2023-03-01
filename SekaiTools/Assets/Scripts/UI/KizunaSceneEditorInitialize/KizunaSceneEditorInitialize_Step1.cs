using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{
    public class KizunaSceneEditorInitialize_Step1 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_KZNSaveData gIP_KZNSaveData;
        [Header("Prefab")]
        public Window step2WindowPrefab;

        public void Apply()
        {
            KizunaSceneEditorInitialize_Step2 kizunaSceneEditorInitialize_Step2
                = WindowController.CurrentWindow.OpenWindow<KizunaSceneEditorInitialize_Step2>(step2WindowPrefab);
            kizunaSceneEditorInitialize_Step2.Initialize(gIP_KZNSaveData.KizunaSceneData);
        }
    }
}