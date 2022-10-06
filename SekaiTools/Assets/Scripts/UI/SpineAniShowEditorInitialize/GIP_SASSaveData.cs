using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.GenericInitializationParts;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniShowEditorInitialize
{
    public class GIP_SASSaveData : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public SaveFileSelectItem file_SaveData;
        public LoadFileSelectItem file_LoadData;
        public Text txt_Template;
        public Image img_Template;
        [Header("Settings")]
        public TextAssetWithPreviewSet templateSet;
        [Header("Settings")]
        public Window templateSelectorPrefab;

        TextAsset selectedTemplate;
        bool ifNewFile = true;

        public string SelectedDataPath => ifNewFile ? file_SaveData.SelectedPath : file_LoadData.SelectedPath;
        public bool IfNewFile => ifNewFile;
        public TextAsset SelectedTemplate => selectedTemplate;

        private void Awake()
        {
            SetSelectedTemplate(0);
        }

        public string CheckIfReady()
        {
            if(ifNewFile&&string.IsNullOrEmpty(file_SaveData.SelectedPath))
            {
                return GenericInitializationCheck.GetErrorString("目录错误", "无效的目录");
            }
            else if (!ifNewFile)
            {
                if(string.IsNullOrEmpty(file_LoadData.SelectedPath))
                    return GenericInitializationCheck.GetErrorString("目录错误", "无效的目录");
                if(!File.Exists(file_LoadData.SelectedPath))
                    return GenericInitializationCheck.GetErrorString("目录错误", "文件不存在");
            }
            return null;
        }

        public void SwitchMode_Create() => ifNewFile = true;
        public void SwitchMode_Load() => ifNewFile = false;

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