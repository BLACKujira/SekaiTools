using System;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_DateTime : GSW_Item
    {
        public InputField inputField;
        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_DateTime configUIItem_DateTime = configUIItem as ConfigUIItem_DateTime;
            if (configUIItem_DateTime == null) throw new ItemTypeMismatchException();
            inputField.text = configUIItem_DateTime.getValue().ToString("d");
            inputField.onValueChanged.AddListener((value) =>
            {
                DateTime outValue;
                if (!DateTime.TryParse(value, out outValue))
                    outValue = DateTime.Now;
                configUIItem_DateTime.setValue(outValue);
            });
        }

    }
}