using SekaiTools.Count;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCSEditorInitialize
{
    public class NCSEditorInitialize_Step2 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_NCSAudio gIP_NCSAudio;
        public GIP_NCSImage gIP_NCSImage;
        public Button btnApply;
        [Header("Prefab")]
        public Window editorWindowPrefab;

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

        Settings settings;
        public void Initialize(Settings settings)
        {
            this.settings = settings;

            nicknameCountShowcase = settings.nicknameCountShowcase;
            nicknameCountData = settings.nicknameCountData;

            nicknameCountShowcase.InstantiateScenes();

            gIP_NCSAudio.Initialize(nicknameCountShowcase);
            gIP_NCSImage.Initialize(nicknameCountShowcase);

            string savePath = nicknameCountShowcase.SavePath;
            string audPath = Path.ChangeExtension(savePath, ".aud");
            string imdPath = Path.ChangeExtension(savePath, ".imd");

            if(!File.Exists(audPath)) 
            {
                AudioData audioData = new AudioData();
                audioData.SavePath = audPath;
                audioData.SaveData();
            }
            if(!File.Exists(imdPath)) 
            {
                ImageData imageData = new ImageData();
                imageData.SavePath = imdPath;
                imageData.SaveData();
            }

            gIP_NCSAudio.file_LoadData.SelectedPath = audPath;
            gIP_NCSImage.file_LoadData.SelectedPath= imdPath;
        }

        public class Settings
        {
            public Count.Showcase.NicknameCountShowcase nicknameCountShowcase;
            public NicknameCountData nicknameCountData;
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_NCSAudio, gIP_NCSImage);
            if(!string.IsNullOrEmpty(errors)) 
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

            NCSPlayerBase.Settings settings = new NCSPlayerBase.Settings();
            settings.countData = nicknameCountData;
            settings.showcase = nicknameCountShowcase;
            settings.audioData = audioData;
            settings.imageData = imageData;

            NCSEditor.NCSEditor nCSEditor
                = window.OpenWindow<NCSEditor.NCSEditor>(editorWindowPrefab);
            nCSEditor.window.OnClose.AddListener(() => window.Close());
            nCSEditor.Initialize(settings);

            btnApply.interactable = true;
        }
    }
}