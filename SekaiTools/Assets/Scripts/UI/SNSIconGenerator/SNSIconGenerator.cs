using SekaiTools.UI.SNSIconCapturer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using UnityEngine.UI;

namespace SekaiTools.UI.SNSIconGenerator
{
    public class SNSIconGenerator : MonoBehaviour
    {
        public enum Mode {AllModel,AllAnimation,ThisScene }

        public Window window;
        [Header("Components")]
        public Image iconBGImage;
        public Toggle toggle_AllModel;
        public Toggle toggle_AllAnimation;
        public Toggle toggle_ThisScene;
        [Header("Settings")]
        public InbuiltSpineModelSet spineModelSet;
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public Window sNSIconCapturerPrefab;

        SpineControllerTypeA spineController;
        Mode mode;

        #region 由切换开关调用的方法
        public void ChangeMode_AllModel()
        {
            mode = Mode.AllModel;
        }

        public void ChangeMode_AllAnimation()
        {
            mode = Mode.AllAnimation;
        }

        public void ChangeMode_ThisScene()
        {
            mode = Mode.ThisScene;
        }
        #endregion

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
            toggle_AllModel.onValueChanged.AddListener((value) => { if (value) ChangeMode_AllModel(); });
            toggle_AllAnimation.onValueChanged.AddListener((value) => { if (value) ChangeMode_AllAnimation(); });
            toggle_ThisScene.onValueChanged.AddListener((value) => { if (value) ChangeMode_ThisScene(); });
        }

        private void OnDestroy()
        {
            Destroy(spineController);
        }

        public void Apply()
        {
            var sNSIconCapturerSettings = new SNSIconCapturer.SNSIconCapturer.SNSIconCapturerSettings();
            sNSIconCapturerSettings.capturerItems = GetCapturerItems();
            switch (mode)
            {
                case Mode.AllModel:
                    sNSIconCapturerSettings.fileNameType = SNSIconCapturer.SNSIconCapturer.FileNameType.modelName;
                    break;
                case Mode.AllAnimation:
                    sNSIconCapturerSettings.fileNameType = SNSIconCapturer.SNSIconCapturer.FileNameType.animationName;
                    break;
                case Mode.ThisScene:
                    sNSIconCapturerSettings.fileNameType = SNSIconCapturer.SNSIconCapturer.FileNameType.both;
                    break;
            }
            sNSIconCapturerSettings.savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\25";
            sNSIconCapturerSettings.spineController = spineController;

            SNSIconCapturer.SNSIconCapturer sNSIconCapturer = window.OpenWindow<SNSIconCapturer.SNSIconCapturer>(sNSIconCapturerPrefab);
            sNSIconCapturer.Initialize(sNSIconCapturerSettings);
            sNSIconCapturer.StartCapture();
        }

        public List<SNSIconCaptureItem> GetCapturerItems()
        {
            List<SNSIconCaptureItem> sNSIconCaptureItems = new List<SNSIconCaptureItem>();
            switch (mode)
            {
                case Mode.AllModel:
                    for (int i = 0; i < spineModelSet.characters.Length; i++)
                    {
                        InbuiltSpineModelSet.Character character = spineModelSet.characters[i];
                        foreach (var atlasAssetPair in character.atlasAssets)
                        {
                            SNSIconCaptureItem sNSIconCaptureItem = new SNSIconCaptureItem(atlasAssetPair,spineController.models[0].Model.AnimationName,ConstData.characters[i].imageColor);
                            sNSIconCaptureItems.Add(sNSIconCaptureItem);
                        }
                    }
                    break;
                case Mode.AllAnimation:
                    global::Spine.SkeletonData skeletonData = spineController.models[0].Model.SkeletonDataAsset.GetSkeletonData(false);
                    AtlasAssetPair thisAtlasAssetPair = spineController.models[0].AtlasAssetPair;
                    foreach (var animation in skeletonData.Animations)
                    {
                        SNSIconCaptureItem sNSIconCaptureItem = new SNSIconCaptureItem(thisAtlasAssetPair, animation.name, iconBGImage.color);
                        sNSIconCaptureItems.Add(sNSIconCaptureItem);
                    }
                    break;
                case Mode.ThisScene:
                    throw new System.NotImplementedException();
                    break;
                default:
                    break;
            }
            return sNSIconCaptureItems;
        }
    }
}