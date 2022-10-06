using SekaiTools.UI.SNSIconCapturer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SekaiTools.UI.SNSIconGenerator
{
    public class SNSIconGenerator : MonoBehaviour
    {
        public enum Mode { AllModel , ThisScene }
        public enum Style { SNS , Clear }

        public Window window;
        [Header("Components")]
        public Image iconBGImage;
        public Dropdown styleDropdown;
        public Toggle toggle_AllModel;
        public Toggle toggle_ThisScene;
        [Header("Settings")]
        public InbuiltSpineModelSet spineModelSet;
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public Window sNSIconCapturerPrefab_SNS;
        public Window sNSIconCapturerPrefab_Clear;

        SpineControllerTypeA spineController;
        Mode mode;
        Style style;

        #region 由切换开关调用的方法
        void ChangeMode_AllModel()
        {
            mode = Mode.AllModel;
        }

        void ChangeMode_ThisScene()
        {
            mode = Mode.ThisScene;
        }
        #endregion

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);

            styleDropdown.options = new List<Dropdown.OptionData>(
                from string str in Enum.GetNames(typeof(Style))
                select new Dropdown.OptionData(str));
            styleDropdown.onValueChanged.AddListener((i) =>
            {
                style = (Style)i;
            });

            toggle_AllModel.onValueChanged.AddListener((value) => { if (value) ChangeMode_AllModel(); });
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
                case Mode.ThisScene:
                    sNSIconCapturerSettings.fileNameType = SNSIconCapturer.SNSIconCapturer.FileNameType.both;
                    break;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            sNSIconCapturerSettings.savePath = folderBrowserDialog.SelectedPath;
            sNSIconCapturerSettings.spineController = spineController;

            SNSIconCapturer.SNSIconCapturer sNSIconCapturer = window.OpenWindow<SNSIconCapturer.SNSIconCapturer>(sNSIconCapturerPrefab_SNS);
            sNSIconCapturer.Initialize(sNSIconCapturerSettings);
            sNSIconCapturer.StartCapture();
        }

        public List<SNSIconCaptureItem> GetCapturerItems()
        {
            List<SNSIconCaptureItem> sNSIconCaptureItems = new List<SNSIconCaptureItem>();
            switch (mode)
            {
                case Mode.AllModel:
                    for (int i = 1; i < spineModelSet.characters.Length; i++)
                    {
                        InbuiltSpineModelSet.Character character = spineModelSet.characters[i];
                        foreach (var atlasAssetPair in character.atlasAssets)
                        {
                            SNSIconCaptureItem sNSIconCaptureItem = new SNSIconCaptureItem(atlasAssetPair,spineController.models[0].Model.AnimationName,ConstData.characters[i].imageColor);
                            sNSIconCaptureItems.Add(sNSIconCaptureItem);
                        }
                    }
                    break;
                case Mode.ThisScene:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
            return sNSIconCaptureItems;
        }
    }
}