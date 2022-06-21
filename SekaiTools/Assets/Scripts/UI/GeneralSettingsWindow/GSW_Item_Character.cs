using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Character : GSW_Item
    {
        [Header("Components")]
        public Image iconImage;
        public Text nameText;
        public Button changeButton;
        [Header("Settings")]
        public IconSet iconSet;
        [Header("Prefabs")]
        public Window characterSelectWindowPrefab;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Void<int> configUIItem_Int = configUIItem as ConfigUIItem_Void<int>;
            if (configUIItem_Int == null) throw new ItemTypeMismatchException();
            ChangeIconAndText(configUIItem_Int.getValue());
            changeButton.onClick.AddListener(() =>
            {
                CharacterSelector.CharacterSelector characterSelector = WindowController.windowController.currentWindow.OpenWindow<CharacterSelector.CharacterSelector>(characterSelectWindowPrefab);
                characterSelector.Initialize((value) =>
                {
                    configUIItem_Int.setValue(value);
                    generalSettingsWindow.Refresh();
                });
            });
        }
        void ChangeIconAndText(int characterId)
        {
            nameText.text = ConstData.characters[characterId].Name;
            iconImage.sprite = iconSet.icons[characterId];
        }
    }
}