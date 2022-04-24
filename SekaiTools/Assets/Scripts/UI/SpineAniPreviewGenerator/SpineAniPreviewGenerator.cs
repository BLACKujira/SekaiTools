using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using SekaiTools.Spine;
using static SekaiTools.UI.SpineAniPreviewCapturer.SpineAniPreviewCapturer;

namespace SekaiTools.UI.SpineAniPreviewGenerator
{
    public class SpineAniPreviewGenerator : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public SpineAniPreviewGenerator_ModelArea modelArea;
        [Header("Settings")]
        public SkeletonDataAsset baseModel;
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public Window spineAniPreviewCapturerPrefab;

        [System.NonSerialized] public SpineControllerTypeA spineController;

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
        }

        private void OnDestroy()
        {
            Destroy(spineController);
        }

        public void Apply()
        {
            SpineAniPreviewCaptureISettings spineAniPreviewCaptureISettings = new SpineAniPreviewCaptureISettings();
            spineAniPreviewCaptureISettings.savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\16";
            spineAniPreviewCaptureISettings.spineController = spineController;
            spineAniPreviewCaptureISettings.capturerItems = new List<SpineAniPreviewCapturer.SpineAniPreviewCaptureItem>();

            foreach (var classifiedAnimation in modelArea.classifiedAnimations)
            {
                foreach (var animation in classifiedAnimation.animations)
                {
                    spineAniPreviewCaptureISettings.capturerItems.Add(new SpineAniPreviewCapturer.SpineAniPreviewCaptureItem(classifiedAnimation.atlasAssetPair,
                                                                                                                             animation.animation,
                                                                                                                             classifiedAnimation.typeName,
                                                                                                                             animation.animationName,
                                                                                                                             ConstData.characters[modelArea.spineModelSet.BelongsTo(classifiedAnimation.atlasAssetPair)].imageColor));
                }
            }

            var spineAniPreviewCapturer = window.OpenWindow<SpineAniPreviewCapturer.SpineAniPreviewCapturer>(spineAniPreviewCapturerPrefab);
            spineAniPreviewCapturer.Initialize(spineAniPreviewCaptureISettings);
            spineAniPreviewCapturer.StartCapture();
        }
    }
}