using SekaiTools.Live2D;
using SekaiTools.UI.L2DModelSelect;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class L2DModelSelectArea_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBgColor;
        public Image imgCharIcon;
        public Text txtKey;
        public Text txtValue;
        public Image imgModelPreview;
        public GameObject gobjNoAniSetError;
        public Button btnSetModel;
        [Header("Settings")]
        public IconSet charIconSet;
        [Header("Prefab")]
        public Window selectModelWindowPrefab;

        L2DModelSelectArea_ItemSettings settings;
        public L2DModelSelectArea_ItemSettings Settings => settings;
        public string Key => settings.key;
        SelectedModelInfo selectedModel = new SelectedModelInfo(null,null);
        public SelectedModelInfo SelectedModel => selectedModel;

        public void Initialize(L2DModelSelectArea_ItemSettings settings)
        {
            this.settings = settings;
            if(settings.characterId>0&&settings.characterId<=57)
            {
                imgBgColor.color = ConstData.characters[ConstData.MergeVirtualSinger(settings.characterId)].imageColor;
                if (settings.characterId > 31)
                    imgCharIcon.sprite = charIconSet.icons[ConstData.MergeVirtualSinger(settings.characterId)];
                else
                    imgCharIcon.sprite = charIconSet.icons[settings.characterId];
            }
            txtKey.text = string.IsNullOrEmpty(settings.keyOverride) ? settings.key : settings.keyOverride;
            selectedModel = L2DModelLoader.GetDefaultModel(settings.characterId);
            RefreshModelInfo();
            btnSetModel.onClick.AddListener(() =>
            {
                L2DModelSelect.L2DModelSelect l2DModelSelect
                    = WindowController.windowController.currentWindow.OpenWindow<L2DModelSelect.L2DModelSelect>(selectModelWindowPrefab);
                l2DModelSelect.Initialize((modelInfo) =>
                {
                    selectedModel = modelInfo;
                    RefreshModelInfo();
                });
            });
        }

        void RefreshModelInfo()
        {
            if (string.IsNullOrEmpty(selectedModel.modelName))
            {
                txtValue.text = "请选择模型";
                gobjNoAniSetError.SetActive(false);
            }
            else
            {
                txtValue.text = selectedModel.modelName;
                imgModelPreview.sprite = L2DModelLoader.GetPreview(selectedModel.modelName);
                if (selectedModel.animationSet == null)
                    gobjNoAniSetError.SetActive(true);
            }
        }
    }
}