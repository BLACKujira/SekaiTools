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
    public abstract class KizunaSceneInitialize_Step2Base : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_ModelSelector gIP_ModelSelector;
        public GIP_CutinSceneAudio gIP_CutinSceneAudio;
        public GIP_KizunaSceneImage gIP_KizunaSceneImage;
        public GIP_KizunaBGParts gIP_KizunaBGParts;
        public Button btnApply;

        KizunaSceneData kizunaSceneData;

        public void Apply()
        {
            StartCoroutine(CoApply());
        }

        public IEnumerator CoApply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(
                gIP_ModelSelector,
                gIP_CutinSceneAudio,
                gIP_KizunaSceneImage);
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

            AudioData audioData = new AudioData();
            ImageData imageData = new ImageData();
            yield return audioData.LoadFile(gIP_CutinSceneAudio.SerializedAudioData);
            yield return imageData.LoadFile(gIP_KizunaSceneImage.SerializedImageData);

            KizunaSceneEditor.KizunaSceneEditor.Settings settings = new KizunaSceneEditor.KizunaSceneEditor.Settings();
            settings.audioData = audioData;
            settings.imageData = imageData;
            settings.kizunaSceneData = kizunaSceneData;
            settings.sekaiLive2DModels = models;
            settings.backGroundParts = gIP_KizunaBGParts.BackGroundParts.ToArray();

            OpenWindow(settings);
            btnApply.interactable = true;
        }

        public void Initialize(KizunaSceneData kizunaSceneData)
        {
            this.kizunaSceneData = kizunaSceneData;
            gIP_ModelSelector.Initialize(
                kizunaSceneData.AppearCharacters
                .Select((id) => new L2DModelSelectArea_ItemSettings(id))
                .ToArray());
            gIP_CutinSceneAudio.Initialize(kizunaSceneData.CutinSceneData);
            gIP_KizunaSceneImage.Initialize(kizunaSceneData);
        }

        protected abstract void OpenWindow(KizunaSceneEditor.KizunaSceneEditor.Settings settings);
    }
}