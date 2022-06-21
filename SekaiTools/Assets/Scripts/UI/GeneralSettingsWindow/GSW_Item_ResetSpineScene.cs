using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_ResetSpineScene : GSW_Item
    {
        [Header("Components")]
        public Button buttonReset;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_ResetSpineScene configUIItem_ResetSpineScene = configUIItem as ConfigUIItem_ResetSpineScene;
            if (configUIItem_ResetSpineScene == null) throw new ItemTypeMismatchException();

            buttonReset.onClick.AddListener(() => configUIItem_ResetSpineScene.resetScene());
        }
    }
}