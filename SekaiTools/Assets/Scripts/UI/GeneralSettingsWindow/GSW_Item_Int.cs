using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{

    public class GSW_Item_Int : GSW_Item
    {
        public InputField inputField;
        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Int configUIItem_Int = configUIItem as ConfigUIItem_Int;
            if (configUIItem_Int == null) throw new ItemTypeMismatchException();
            inputField.text = configUIItem_Int.getValue().ToString();
            inputField.onValueChanged.AddListener((value) =>
            {
                int outValue;
                if (!int.TryParse(value, out outValue))
                    outValue = 0;
                configUIItem_Int.setValue(outValue);
            });
        }
    }
}