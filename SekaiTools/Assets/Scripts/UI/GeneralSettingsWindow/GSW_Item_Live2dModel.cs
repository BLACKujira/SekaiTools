using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Live2dModel : GSW_Item
    {
        [Header("Components")]
        public Text nameText;
        public Button selectButton;
        [Header("Prefabs")]
        public Window modelSelectWindow;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Live2dModel configUIItem_Live2dModel = configUIItem as ConfigUIItem_Live2dModel;
            if (configUIItem_Live2dModel == null) throw new ItemTypeMismatchException();

            L2DModelSelect.SelectedModelInfo selectedModelInfo = configUIItem_Live2dModel.getValue();
            nameText.text = string.IsNullOrEmpty(selectedModelInfo.modelName) ? "无" : selectedModelInfo.modelName;

            selectButton.onClick.AddListener(() =>
            {
                L2DModelSelect.L2DModelSelect l2DModelSelect
                    = WindowController.windowController.currentWindow.OpenWindow<L2DModelSelect.L2DModelSelect>(modelSelectWindow);
                l2DModelSelect.Initialize((modelInfo) =>
                {
                    configUIItem_Live2dModel.setValue(modelInfo);
                    nameText.text = string.IsNullOrEmpty(modelInfo.modelName) ? "无" : modelInfo.modelName;
                });
            });
        }
    }
}