using SekaiTools.Count;
using SekaiTools.Live2D;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.L2DModelSelect;
using SekaiTools.UI.NCSEditorInitialize;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCSPlayerInitialize
{
    public class NCSPlayerInitialize_Step2 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_NCSAudio gIP_NCSAudio;
        public GIP_NCSImage gIP_NCSImage;
        public GIP_ModelSelector gIP_ModelSelector;
        public Button btnApply;
        [Header("Prefab")]
        public Window playerWindowPrefab;

        public static HashSet<string> RequireMasterTables => NCSPlayerBase.RequireMasterTables;

        Count.Showcase.NicknameCountShowcase nicknameCountShowcase;
        NicknameCountData nicknameCountData;

        private void Awake()
        {
            window.OnClose.AddListener(() =>
            {
                nicknameCountShowcase.DestroyScenes();
            });
        }

        public void Initialize(Settings settings)
        {
            nicknameCountShowcase = settings.nicknameCountShowcase;
            nicknameCountData = settings.nicknameCountData;

            nicknameCountShowcase.InstantiateScenes();

            gIP_NCSAudio.Initialize(nicknameCountShowcase);
            gIP_NCSImage.Initialize(nicknameCountShowcase);

            gIP_ModelSelector.Initialize(
                nicknameCountShowcase.charactersRequireL2d
                .Select(value=>new L2DModelSelectArea_ItemSettings(value))
                .ToArray());

            string savePath = nicknameCountShowcase.SavePath;
            string audPath = Path.ChangeExtension(savePath, ".aud");
            string imdPath = Path.ChangeExtension(savePath, ".imd");

            if (!File.Exists(audPath))
            {
                AudioData audioData = new AudioData();
                audioData.SavePath = audPath;
                audioData.SaveData();
            }
            if (!File.Exists(imdPath))
            {
                ImageData imageData = new ImageData();
                imageData.SavePath = imdPath;
                imageData.SaveData();
            }

            gIP_NCSAudio.file_LoadData.SelectedPath = audPath;
            gIP_NCSImage.file_LoadData.SelectedPath = imdPath;
        }

        public class Settings
        {
            public Count.Showcase.NicknameCountShowcase nicknameCountShowcase;
            public NicknameCountData nicknameCountData;
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_NCSAudio, gIP_NCSImage,gIP_ModelSelector);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            StartCoroutine(CoApply());
        }

        IEnumerator CoApply()
        {
            btnApply.interactable = false;

            SerializedAudioData serializedAudioData = gIP_NCSAudio.SerializedAudioData;
            SerializedImageData serializedImageData = gIP_NCSImage.SerializedImageData;

            AudioData audioData = new AudioData();
            ImageData imageData = new ImageData();

            yield return audioData.LoadFile(serializedAudioData);
            yield return imageData.LoadFile(serializedImageData);

            audioData.SavePath = gIP_NCSAudio.SavePath;
            imageData.SavePath = gIP_NCSImage.SavePath;

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

            NCSPlayerBase.Settings settings = new NCSPlayerBase.Settings();
            settings.countData = nicknameCountData;
            settings.showcase = nicknameCountShowcase;
            settings.audioData = audioData;
            settings.imageData = imageData;
            settings.live2DModels = models;

            NCSPlayer.NCSPlayer nCSPlayer
                = window.OpenWindow<NCSPlayer.NCSPlayer>(playerWindowPrefab);
            nCSPlayer.window.OnClose.AddListener(() => window.Close());
            nCSPlayer.Initialize(settings);

            btnApply.interactable = true;
        }
    }
}