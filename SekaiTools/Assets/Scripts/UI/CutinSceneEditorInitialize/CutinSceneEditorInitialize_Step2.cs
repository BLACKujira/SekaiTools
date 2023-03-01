using SekaiTools.Cutin;
using SekaiTools.Live2D;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.L2DModelSelect;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CutinSceneEditorInitialize
{
    public class CutinSceneEditorInitialize_Step2 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_ModelSelector gIP_ModelSelector;
        public GIP_CutinSceneAudio gIP_CutinSceneAudio;
        public Button btnApply;
        [Header("Prefab")]
        public Window editorPrefab;

        CutinSceneData cutinSceneData;

        public void Initialize(CutinSceneData cutinSceneData)
        {
            this.cutinSceneData = cutinSceneData;
            gIP_ModelSelector.Initialize(
                cutinSceneData.CountAppearCharacters()
                .Select((id)=>new L2DModelSelectArea_ItemSettings(
                    id,
                    id.ToString(),
                    $"{id:00} {ConstData.characters[id].Name}"))
                .ToArray()
                );
            gIP_CutinSceneAudio.Initialize(cutinSceneData);
        }

        public void Apply()
        {
            StartCoroutine(CoApply());
        }

        public IEnumerator CoApply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(
                gIP_ModelSelector,
                gIP_CutinSceneAudio);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                yield break;
            }

            btnApply.interactable = false;

            Dictionary<string, SelectedModelInfo> keyValuePairs = gIP_ModelSelector.KeyValuePairs;
            int modelArrayLength = 57;
            SekaiLive2DModel[] models = new SekaiLive2DModel[modelArrayLength];

            for (int i = 0; i < modelArrayLength; i++)
            {
                string key = i.ToString();
                if (keyValuePairs.ContainsKey(key))
                {
                    SelectedModelInfo selectedModelInfo = keyValuePairs[key];
                    L2DModelLoaderObjectBase l2DModelLoaderObjectBase = L2DModelLoader.LoadModel(selectedModelInfo.modelName);
                    yield return l2DModelLoaderObjectBase;
                    SekaiLive2DModel model = l2DModelLoaderObjectBase.Model;
                    model.AnimationSet = L2DModelLoader.InbuiltAnimationSet.GetAnimationSet(selectedModelInfo.animationSet);
                    models[i] = model;
                }
            }

            CutinSceneEditor.CutinSceneEditor.Settings settings = new CutinSceneEditor.CutinSceneEditor.Settings();
            AudioData audioData = new AudioData();
            yield return audioData.LoadFile(gIP_CutinSceneAudio.SerializedAudioData);
            settings.audioData = audioData;
            settings.cutinSceneData = cutinSceneData;
            settings.sekaiLive2DModels = models;

            btnApply.interactable = true;

            CutinSceneEditor.CutinSceneEditor cutinSceneEditor = window.OpenWindow<CutinSceneEditor.CutinSceneEditor>(editorPrefab);
            cutinSceneEditor.window.OnClose.AddListener(() => window.Close());
            cutinSceneEditor.Initialize(settings);
        }
    }
}