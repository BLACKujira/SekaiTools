using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Float : GSW_Item
    {
        public InputField inputField;
        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Float configUIItem_Float = configUIItem as ConfigUIItem_Float;
            if (configUIItem_Float == null) throw new ItemTypeMismatchException();
            inputField.text = configUIItem_Float.getValue().ToString("0.00");
            inputField.onValueChanged.AddListener((value) =>
            {
                float outValue;
                if (!float.TryParse(value, out outValue))
                    outValue = 0;
                configUIItem_Float.setValue(outValue);
            });
        }
    }
}