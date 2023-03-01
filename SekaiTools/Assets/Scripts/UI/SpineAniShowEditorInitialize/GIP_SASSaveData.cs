using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.GenericInitializationParts;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniShowEditorInitialize
{
    public class GIP_SASSaveData : GIP_CreateOrSaveData
    {
        [Header("Components")]
        public Text txt_Template;
        public Image img_Template;
        [Header("Settings")]
        public TextAssetWithPreviewSet templateSet;
        [Header("Settings")]
        public Window templateSelectorPrefab;

        TextAsset selectedTemplate;

        public TextAsset SelectedTemplate => selectedTemplate;

        private void Awake()
        {
            SetSelectedTemplate(0);
        }

        public void SelectTemplate()
        {
            UniversalSelector universalSelector = 
                WindowController.windowController.currentWindow.OpenWindow<UniversalSelector>(
                    templateSelectorPrefab);
            universalSelector.Generate(templateSet.values.Length,
                (btn, id) =>
                {
                    ButtonWithIconAndText buttonWithIconAndText = btn.GetComponent<ButtonWithIconAndText>();
                    buttonWithIconAndText.Icon = templateSet[id].sprite;
                    buttonWithIconAndText.Label = templateSet[id].name;
                },
                (id) =>
                {
                    SetSelectedTemplate(id);
                });
        }

        void SetSelectedTemplate(int templateId)
        {
            selectedTemplate = templateSet[templateId].textAsset;
            txt_Template.text = templateSet[templateId].name;
            img_Template.sprite = templateSet[templateId].sprite;
        }
    }                                              
}