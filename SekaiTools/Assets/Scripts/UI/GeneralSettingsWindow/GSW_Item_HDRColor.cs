using SekaiTools.UI.Modifier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_HDRColor : GSW_Item
    {
        public ModifierUI_ColorBase modifierUI_ColorBase;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_HDRColor configUIItem_HDRColor = configUIItem as ConfigUIItem_HDRColor;
            if (configUIItem_HDRColor == null) throw new ItemTypeMismatchException();
            modifierUI_ColorBase.Initialize(configUIItem_HDRColor.getValue, configUIItem_HDRColor.setValue);
        }
    }
}