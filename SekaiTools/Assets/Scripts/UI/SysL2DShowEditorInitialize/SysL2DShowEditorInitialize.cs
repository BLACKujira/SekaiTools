using SekaiTools.Live2D;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.L2DModelSelect;
using SekaiTools.UI.MessageLayer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowEditorInitialize
{
    public class SysL2DShowEditorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_SysL2DShowData gIP_SysL2DShowData;
        public GIP_ModelSelector gIP_ModelSelector;
        public GIP_SysL2DShowAudio gIP_SysL2DShowAudio;
        public Button btnApply;
        [Header("Prefab")]
        public Window editorPrefab;

        private void Awake()
        {
            gIP_SysL2DShowData.onDataChanged += (data) =>
            {
                gIP_ModelSelector.Clear();
                gIP_ModelSelector.Initialize(data.AppearCharacters.Select(
                    (charId) =>
                    {
                        return new L2DModelSelectArea_ItemSettings(
                            charId,
                            charId.ToString(),
                            $"{charId:00} {ConstData.characters[charId].Name}");
                    }).ToArray());
                gIP_SysL2DShowAudio.SetData(data);
            };

        }

        public void Apply()
        {
            StartCoroutine(CoApply());
        }

        public IEnumerator CoApply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(
                gIP_SysL2DShowData,
                gIP_ModelSelector,
                gIP_SysL2DShowAudio);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                yield break;
            }

            btnApply.interactable = false;

            Dictionary<string, SelectedModelInfo> keyValuePairs = gIP_ModelSelector.KeyValuePairs;
            int modelArrayLength = 32;
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

            SysL2DShowEditor.SysL2DShowEditor.Settings settings = new SysL2DShowEditor.SysL2DShowEditor.Settings();
            settings.sysL2DShowData = gIP_SysL2DShowData.SysL2DShowData;
            AudioData audioData = new AudioData();
            yield return audioData.LoadFile(gIP_SysL2DShowAudio.SerializedAudioData);
            settings.playerSettings.audioData = audioData;
            settings.playerSettings.l2dSettings.models = models;

            btnApply.interactable = true;

            SysL2DShowEditor.SysL2DShowEditor sysL2DShowEditor
                = window.OpenWindow<SysL2DShowEditor.SysL2DShowEditor>(editorPrefab);
            sysL2DShowEditor.Initialize(settings);
        }
    }
}