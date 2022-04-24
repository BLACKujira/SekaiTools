using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using SekaiTools.Spine;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.SNSIconCapturer
{
    public class SNSIconCapturer : MonoBehaviour
    {
        public enum FileNameType { modelName, animationName, both }
        public Window window;
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Canvas targetCanvas;
        public Image bgImage;
        public PerecntBar perecntBar;
        [Header("Settings")]
        public Texture2D mask;

        List<SNSIconCaptureItem> capturerItems = new List<SNSIconCaptureItem>();
        SpineControllerTypeA spineController;
        string savePath;
        FileNameType fileNameType;

        public void Initialize(SNSIconCapturerSettings sNSIconCapturerSettings)
        {
            this.capturerItems = sNSIconCapturerSettings.capturerItems;
            this.spineController = sNSIconCapturerSettings.spineController;
            this.savePath = sNSIconCapturerSettings.savePath;
            this.fileNameType = sNSIconCapturerSettings.fileNameType;
        }

        public void StartCapture()
        {
            StartCoroutine(IStartCapture());
        }
        IEnumerator IStartCapture()
        {
            for (int i = 0; i < capturerItems.Count; i++)
            {
                SNSIconCaptureItem sNSIconCaptureItem = capturerItems[i];
                bgImage.color = sNSIconCaptureItem.bgColor;
                spineController.ReplaceModel(sNSIconCaptureItem.atlasAssetPair, 0);
                SpineControllerTypeA.ModelPair modelPair = spineController.models[0];
                modelPair.Model.AnimationState.TimeScale = 0xffffffff;
                modelPair.Model.AnimationState.SetAnimation(0, sNSIconCaptureItem.animation, false);
                yield return new WaitForSeconds(.15f);
                yield return new WaitForEndOfFrame();

                Texture2D texture2D = ExtensionTools.Capture(targetRectTransform, targetCanvas);
                texture2D = ExtensionTools.ApplyMask(texture2D, mask);
                byte[] png = texture2D.EncodeToPNG();
                string fileName = "snsicon.png";
                switch (fileNameType)
                {
                    case FileNameType.modelName:
                        fileName = modelPair.Name;
                        break;
                    case FileNameType.animationName:
                        fileName = sNSIconCaptureItem.animation;
                        break;
                    case FileNameType.both:
                        fileName = $"{modelPair.Name}_{sNSIconCaptureItem.animation}";
                        break;
                }
                File.WriteAllBytes(Path.Combine(savePath, fileName + ".png"), png);

                perecntBar.priority = ((float)i) / capturerItems.Count;
            }

            window.Close();
        }

        public class SNSIconCapturerSettings
        {
            public List<SNSIconCaptureItem> capturerItems;
            public SpineControllerTypeA spineController;
            public string savePath;
            public FileNameType fileNameType;
        }
    }

    public class SNSIconCaptureItem
    {
        public AtlasAssetPair atlasAssetPair;
        public string animation;
        //public float animationProgress;
        public Color bgColor;

        public SNSIconCaptureItem(AtlasAssetPair atlasAssetPair, string animation, Color bgColor)
        {
            this.atlasAssetPair = atlasAssetPair;
            this.animation = animation;
            this.bgColor = bgColor;
        }
    }
}