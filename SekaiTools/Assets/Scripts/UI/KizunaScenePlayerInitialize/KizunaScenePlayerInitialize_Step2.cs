using UnityEngine;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{
    public class KizunaScenePlayerInitialize_Step2 : KizunaSceneInitialize_Step2Base
    {
        [Header("Prefab")]
        public Window playerPrefab;

        protected override void OpenWindow(KizunaSceneEditor.KizunaSceneEditor.Settings settings)
        {
            KizunaScenePlayer.KizunaScenePlayer kizunaScenePlayer
                = window.OpenWindow<KizunaScenePlayer.KizunaScenePlayer>(playerPrefab);
            kizunaScenePlayer.window.OnClose.AddListener(() => kizunaScenePlayer.DestroyLive2DModels());
            kizunaScenePlayer.Initialize(settings);
        }
    }
}