using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.KizunaSceneCreate;
using SekaiTools.Kizuna;
using SekaiTools.UI.CutinScenePlayerInitialize;
using UnityEngine.UI;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using System.IO;

namespace SekaiTools.UI.KizunaScenePlayerInitialize
{
    public class KizunaScenePlayerInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public KizunaScenePlayerInitialize_Audio audioArea;
        public KizunaScenePlayerInitialize_Graphics graphicArea;
        public KizunaScenePlayerInitialize_Model modelArea;
        public KizunaScenePlayerInitialize_BGPart bGPart;
        public InputField pathInputField;
        public Button applyButton;
        [Header("Prefab")]
        public Window newDataWindowPrefab;
        public Window openWindowPrefab;
        public Window nowLoadingWindowPrefab;
        [Header("Settings")]
        public KizunaSceneCreate.KizunaSceneCreate.Mode mode;

        public bool IfGetReady
        {
            get
            {
                if (kizunaSceneData == null) return false;
                if (graphicArea!=null&&!graphicArea.IfGetReady) return false;
                if (!modelArea.IfGetReady()) return false;
                return true;
            }
        }

        [System.NonSerialized] public KizunaSceneDataBase kizunaSceneData;

        OpenFileDialog openFileDialog;

        private void Awake()
        {
            Refresh();
            openFileDialog = FileDialogFactory.GetOpenFileDialog_KizunaSceneData();
        }

        public void NewData()
        {
            KizunaSceneCreate.KizunaSceneCreate kizunaSceneCreate = window.OpenWindow<KizunaSceneCreate.KizunaSceneCreate>(newDataWindowPrefab);
            kizunaSceneCreate.Initialize(
                (KizunaSceneDataBase kizunaSceneData, AudioData audioData) =>
                {
                    this.kizunaSceneData = kizunaSceneData;
                    audioArea.audioData = audioData;
                    Refresh();
                });
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = openFileDialog.FileName;
            if(mode == KizunaSceneCreate.KizunaSceneCreate.Mode.Normal)
                kizunaSceneData = KizunaSceneData.LoadData(File.ReadAllText(fileName));
            else
                kizunaSceneData = CustomKizunaData.LoadData(File.ReadAllText(fileName));

            kizunaSceneData.savePath = fileName;
            string audioFileName = Path.ChangeExtension(fileName, ".aud");
            if (File.Exists(audioFileName))
            {
                audioArea.LoadData(Refresh, audioFileName);
            }
            string imageFileName = Path.ChangeExtension(fileName, ".imd");
            if (File.Exists(imageFileName)&& kizunaSceneData is KizunaSceneData)
            {
                ImageData imageData = new ImageData(imageFileName);
                graphicArea.imageData = imageData;
                NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
                nowLoadingTypeA.TitleText = "读取文件中";
                nowLoadingTypeA.OnFinish += () =>
                {
                    graphicArea.imageMatchingCount = ((KizunaSceneData)kizunaSceneData).CountImageMatching(imageData);
                    Refresh();
                };
                nowLoadingTypeA.StartProcess(imageData.LoadData(File.ReadAllText(imageFileName)));
            }
        }

        public void Refresh()
        {
            pathInputField.text = kizunaSceneData == null ? string.Empty : kizunaSceneData.savePath;
            if(graphicArea!=null) graphicArea.Refresh();
            audioArea.Initialize(kizunaSceneData);
            if (kizunaSceneData == null) return;

            modelArea.Initialize(kizunaSceneData.CountAppearCharacters());
        }

        private void Update()
        {
            applyButton.interactable = IfGetReady;
        }

        public void Apply()
        {
            if (openWindowPrefab.controlScript is KizunaSceneEditor.KizunaSceneEditor)
            {
                KizunaSceneEditor.KizunaSceneEditor.KizunaSceneEditorSettings kizunaSceneEditorSettings = new KizunaSceneEditor.KizunaSceneEditor.KizunaSceneEditorSettings();
                kizunaSceneEditorSettings.imageData = graphicArea != null ? graphicArea.imageData : null;
                kizunaSceneEditorSettings.kizunaSceneData = kizunaSceneData;
                kizunaSceneEditorSettings.audioData = audioArea.audioData;
                kizunaSceneEditorSettings.sekaiLive2DModels = modelArea.sekaiLive2DModels;
                kizunaSceneEditorSettings.backGroundParts = bGPart.backGroundParts.ToArray();

                KizunaSceneEditor.KizunaSceneEditor kizunaSceneEditor = window.OpenWindow<KizunaSceneEditor.KizunaSceneEditor>(openWindowPrefab);
                kizunaSceneEditor.Initialize(kizunaSceneEditorSettings);
            }
            else
            {
                KizunaScenePlayer.KizunaScenePlayer.KizunaScenePlayerSettings kizunaScenePlayerSettings = new KizunaScenePlayer.KizunaScenePlayer.KizunaScenePlayerSettings();
                kizunaScenePlayerSettings.imageData = graphicArea != null ? graphicArea.imageData : null;
                kizunaScenePlayerSettings.kizunaSceneData = kizunaSceneData;
                kizunaScenePlayerSettings.audioData = audioArea.audioData;
                kizunaScenePlayerSettings.sekaiLive2DModels = modelArea.sekaiLive2DModels;
                kizunaScenePlayerSettings.backGroundParts = bGPart.backGroundParts.ToArray();

                KizunaScenePlayer.KizunaScenePlayer kizunaScenePlayer = window.OpenWindow<KizunaScenePlayer.KizunaScenePlayer>(openWindowPrefab);
                kizunaScenePlayer.Initialize(kizunaScenePlayerSettings);
            }
        }
    }
}