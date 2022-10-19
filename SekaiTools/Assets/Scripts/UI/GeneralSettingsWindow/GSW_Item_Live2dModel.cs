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

            Live2D.SekaiLive2DModel sekaiLive2DModel = configUIItem_Live2dModel.getValue();
            nameText.text = sekaiLive2DModel == null ? "无" : sekaiLive2DModel.name;

            selectButton.onClick.AddListener(() =>
            {
                //L2DModelSelect l2DModelSelect = WindowController.windowController.currentWindow.OpenWindow<L2DModelSelect>(modelSelectWindow);
                //l2DModelSelect.Generate(
                //    (SekaiLive2DModel model) => 
                //    {
                //        configUIItem_Live2dModel.setValue(model);
                //        nameText.text = sekaiLive2DModel == null ? "无" : model.name;
                //        generalSettingsWindow.Refresh();
                //    });
            });
        }
    }
}