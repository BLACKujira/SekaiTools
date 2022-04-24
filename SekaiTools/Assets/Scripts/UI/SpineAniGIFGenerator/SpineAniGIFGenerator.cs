using SekaiTools.Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SekaiTools.UI.SpineAniGIFCapturer.SpineAniGIFCapturer;

namespace SekaiTools.UI.SpineAniGIFGenerator
{
    public class SpineAniGIFGenerator : MonoBehaviour
    {
        public enum GenerateMode { GIF,PNGWITHBG,PNGNOBG }

        public Window window;
        [Header("Components")]
        public SpineAniGIFGenerator_ModelArea modelArea;
        public SpineAniGIFGenerator_ModeArea modeArea;
        public SpineAniGIFGenerator_CaptureArea captureArea;
        public RectTransform captureBox;
        [Header("Settings")]
        public string defaultModelName = "sd_21miku_normal";
        public InbuiltSpineModelSet spineModelSet;
        public Vector2 modelPosition = new Vector2(-0.04f, -1.1f);
        public Vector3 modelScale = new Vector3(0.4f, 0.4f, 1);
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public Window capturerWindowPrefab;

        [System.NonSerialized] public SpineControllerTypeA spineController;

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
            SpineControllerTypeA.ModelPair modelPair = spineController.AddModel(spineModelSet.GetValue(defaultModelName));
            modelPair.Model.transform.position = modelPosition;
            modelPair.Model.transform.localScale = modelScale;
        }

        private void Start()
        {
            modelArea.Refresh();
        }

        private void OnDestroy()
        {
            Destroy(spineController);
        }

        public void Apply()
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog;
            GenerateMode generateMode = modeArea.generateMode;
            if (generateMode == GenerateMode.GIF)
                saveFileDialog = FileDialogFactory.GetSaveFileDialog_GIF();
            else
                saveFileDialog = FileDialogFactory.GetSaveFileDialog_PNG();

            System.Windows.Forms.DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK) return;

            SpineAniGIFCapturerSettings settings = new SpineAniGIFCapturerSettings();
            settings.spineController = spineController;
            switch (generateMode)
            {
                case GenerateMode.GIF:
                    settings.captureMode = CaptureMode.Screen;
                    settings.encodeMode = EncodeMode.gif;
                    break;
                case GenerateMode.PNGWITHBG:
                    settings.captureMode = CaptureMode.Screen;
                    settings.encodeMode = EncodeMode.png;
                    break;
                case GenerateMode.PNGNOBG:
                    settings.captureMode = CaptureMode.RenderTexture;
                    settings.encodeMode = EncodeMode.png;
                    break;
            }
            settings.position = captureArea.Position;
            settings.size = captureArea.Size;
            settings.saveFilePath = saveFileDialog.FileName;
            settings.frameRate = captureArea.Frame;

            SpineAniGIFCapturer.SpineAniGIFCapturer spineAniGIFCapturer = window.OpenWindow<SpineAniGIFCapturer.SpineAniGIFCapturer>(capturerWindowPrefab);
            spineAniGIFCapturer.Initialize(settings);
            spineAniGIFCapturer.StartCapture();
        }
    }
}