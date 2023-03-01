using SekaiTools.Kizuna;
using SekaiTools.Live2D;
using SekaiTools.UI.CutinSceneEditorInitialize;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.L2DModelSelect;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{
    public class KizunaSceneEditorInitialize_Step2 : KizunaSceneInitialize_Step2Base
    {
        [Header("Prefab")]
        public Window editorPrefab;

        protected override void OpenWindow(KizunaSceneEditor.KizunaSceneEditor.Settings settings)
        {
            KizunaSceneEditor.KizunaSceneEditor kizunaSceneEditor
                = window.OpenWindow<KizunaSceneEditor.KizunaSceneEditor>(editorPrefab);
            kizunaSceneEditor.window.OnClose.AddListener(() => window.Close());
            kizunaSceneEditor.Initialize(settings);
        }
    }
}