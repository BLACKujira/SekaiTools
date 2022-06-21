using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_SpineScene : GSW_Item
    {
        [Header("Components")]
        public Button buttonEdit;
        [Header("Prefabs")]
        public Window spineSceneEditorPrefab;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_SpineScene configUIItem_SpineScene = configUIItem as ConfigUIItem_SpineScene;
            if (configUIItem_SpineScene == null) throw new ItemTypeMismatchException();

            buttonEdit.onClick.AddListener(() =>
            {
                SpineSceneEditor.SpineSceneEditor spineSceneEditor = WindowController.windowController.currentWindow.OpenWindow<SpineSceneEditor.SpineSceneEditor>(spineSceneEditorPrefab);
                spineSceneEditor.Initialize(configUIItem_SpineScene.getValue(), configUIItem_SpineScene.setValue);
            });
        }
    }
}