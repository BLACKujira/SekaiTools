using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using SekaiTools.Spine;
using System.IO;
using System;

namespace SekaiTools.UI.SNSIconCapturer
{
    public abstract class SNSIconCapturer : MonoBehaviour
    {
        public enum FileNameType { modelName, animationName, both }
        public Window window;
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Canvas targetCanvas;
        public PerecntBar perecntBar;

        List<SNSIconCaptureItem> capturerItems = new List<SNSIconCaptureItem>();
        SpineControllerTypeA spineController;
        string savePath;
        FileNameType fileNameType;

        public virtual void Initialize(SNSIconCapturerSettings sNSIconCapturerSettings)
        {
            capturerItems = sNSIconCapturerSettings.capturerItems;
            spineController = sNSIconCapturerSettings.spineController;
            savePath = sNSIconCapturerSettings.savePath;
            fileNameType = sNSIconCapturerSettings.fileNameType;
        }

        public event Action<SNSIconCaptureItem> onItemChange;
        public Func<Texture2D, Texture2D> postProcessing = null;
        public void StartCapture()
        {
            StartCoroutine(IStartCapture());
        }
        IEnumerator IStartCapture()
        {
            for (int i = 0; i < capturerItems.Count; i++)
            {
                SNSIconCaptureItem sNSIconCaptureItem = capturerItems[i];
                onItemChange(sNSIconCaptureItem);
                spineController.ReplaceModel(sNSIconCaptureItem.atlasAssetPair, 0);
                SpineControllerTypeA.ModelPair modelPair = spineController.models[0];
                modelPair.Model.AnimationState.TimeScale = 0xffffffff;
                modelPair.Model.AnimationState.SetAnimation(0, sNSIconCaptureItem.animation, false);
                yield return new WaitForSeconds(.15f);
                yield return new WaitForEndOfFrame();

                Texture2D texture2D = ExtensionTools.Capture(targetRectTransform, targetCanvas);
                if(postProcessing != null)
                    texture2D = postProcessing(texture2D);
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
}