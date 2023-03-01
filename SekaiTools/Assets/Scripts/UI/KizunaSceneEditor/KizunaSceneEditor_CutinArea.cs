using SekaiTools.Kizuna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.KizunaSceneEditor
{
    public class KizunaSceneEditor_CutinArea : MonoBehaviour
    {
        public KizunaSceneEditor kizunaSceneEditor;
        [Header("Components")]
        public Text textCutinSceneCount;
        public Button openEdiorButton;
        [Header("Prefabs")]
        public Window editorWindow;

        public void SetData(KizunaSceneBase kizunaScene)
        {
            textCutinSceneCount.text = kizunaScene.cutinScenes.Count + " 互动语音";
            if (kizunaScene.cutinScenes.Count == 0) openEdiorButton.interactable = false;
            else openEdiorButton.interactable = true;
        }

        public void OpenCutinSceneEditor()
        {
            CutinSceneEditor.CutinSceneEditor cutinSceneEditor = kizunaSceneEditor.window.OpenWindow<CutinSceneEditor.CutinSceneEditor>(editorWindow);
            CutinSceneEditor.CutinSceneEditor.Settings settings = new CutinSceneEditor.CutinSceneEditor.Settings();
            settings.audioData = kizunaSceneEditor.audioData;
            settings.cutinSceneData = new Cutin.CutinSceneDataInKizunaData(kizunaSceneEditor.currentKizunaScene.cutinScenes, kizunaSceneEditor.kizunaSceneData);
            settings.cutinSceneData.playerType = kizunaSceneEditor.kizunaSceneData.cutinPlayerType;
            settings.sekaiLive2DModels = kizunaSceneEditor.modelArea.l2DController.live2DModels;
            cutinSceneEditor.Initialize(settings);
            kizunaSceneEditor.modelArea.l2DController.HideModelAll();
        }


    }
}