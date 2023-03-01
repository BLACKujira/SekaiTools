using UnityEngine;
using SekaiTools.UI.KizunaSceneEditorInitialize;

namespace SekaiTools.UI.KizunaScenePlayerInitialize
{
    public class KizunaScenePlayerInitialize_Step1 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_KZNSaveData gIP_KZNSaveData;
        [Header("Prefab")]
        public Window step2WindowPrefab;

        private void Awake()
        {
            gIP_KZNSaveData.SwitchMode_Load();
        }

        public void Apply()
        {
            KizunaScenePlayerInitialize_Step2 kizunaScenePlayerInitialize_Step2
                = WindowController.CurrentWindow.OpenWindow<KizunaScenePlayerInitialize_Step2>(step2WindowPrefab);
            kizunaScenePlayerInitialize_Step2.Initialize(gIP_KZNSaveData.KizunaSceneData);
        }
    }
}