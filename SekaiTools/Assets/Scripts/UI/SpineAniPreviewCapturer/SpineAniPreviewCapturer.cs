using SekaiTools.Spine;
using SekaiTools.UI.SNSIconCapturer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniPreviewCapturer
{
    public class SpineAniPreviewCapturer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Canvas targetCanvas;
        public Image bgImage;
        public PerecntBar perecntBar;
        public Text typeText;
        public Text nameText;
        [Header("Settings")]
        public Texture2D mask;

        List<SpineAniPreviewCaptureItem> capturerItems = new List<SpineAniPreviewCaptureItem>();
        SpineControllerTypeA spineController;
        string savePath;

        public void Initialize(SpineAniPreviewCaptureISettings sNSIconCapturerSettings)
        {
            this.capturerItems = sNSIconCapturerSettings.capturerItems;
            this.spineController = sNSIconCapturerSettings.spineController;
            this.savePath = sNSIconCapturerSettings.savePath;
        }

        public void StartCapture()
        {
            StartCoroutine(IStartCapture());
        }
        IEnumerator IStartCapture()
        {
            for (int i = 0; i < capturerItems.Count; i++)
            {
                SpineAniPreviewCaptureItem item = capturerItems[i];
                typeText.text = item.animationType;
                nameText.text = item.animationName;

                bgImage.color = item.bgColor;
                spineController.ReplaceModel(item.atlasAssetPair, 0);
                SpineControllerTypeA.ModelPair modelPair = spineController.models[0];
                modelPair.Model.AnimationState.TimeScale = 0xffffffff;
                modelPair.Model.AnimationState.SetAnimation(0, item.animation, false);
                yield return new WaitForSeconds(.15f);
                yield return new WaitForEndOfFrame();

                Texture2D texture2D = ExtensionTools.Capture(targetRectTransform, targetCanvas);
                texture2D = ExtensionTools.ApplyMask(texture2D, mask);
                byte[] png = texture2D.EncodeToPNG();
                string fileName = item.animation;
                File.WriteAllBytes(Path.Combine(savePath, fileName + ".png"), png);

                perecntBar.priority = ((float)i) / capturerItems.Count;
            }

            window.Close();
        }

        public class SpineAniPreviewCaptureISettings
        {
            public List<SpineAniPreviewCaptureItem> capturerItems;
            public SpineControllerTypeA spineController;
            public string savePath;
        }
    }

    public class SpineAniPreviewCaptureItem
    {
        public AtlasAssetPair atlasAssetPair;
        public string animation;
        public string animationType;
        public string animationName;
        public Color bgColor;

        public SpineAniPreviewCaptureItem(AtlasAssetPair atlasAssetPair, string animation, string animationType, string animationName, Color bgColor)
        {
            this.atlasAssetPair = atlasAssetPair;
            this.animation = animation;
            this.animationType = animationType;
            this.animationName = animationName;
            this.bgColor = bgColor;
        }
    }
}