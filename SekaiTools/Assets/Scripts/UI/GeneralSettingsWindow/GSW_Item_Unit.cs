using System;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Unit : GSW_Item
    {
        [Header("Components")]
        public Image imgUnitLogo;
        public Button btnSelectUnit;
        [Header("Settings")]
        public IconSet unitIconSet;
        [Header("Prefab")]
        public Window unitSelectorPrefab;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Unit configUIItem_Unit = configUIItem as ConfigUIItem_Unit;
            if (configUIItem_Unit == null) throw new ItemTypeMismatchException();

            btnSelectUnit.onClick.AddListener(() =>
            {
                UnitSelect.UnitSelect unitSelect
                    = WindowController.CurrentWindow.OpenWindow<UnitSelect.UnitSelect>(unitSelectorPrefab);
                unitSelect.Initialize((unit) =>
                {
                    configUIItem_Unit.setValue(unit);
                    Refresh(configUIItem_Unit.getValue);
                });
            });
        }

        void Refresh(Func<Unit> getValue)
        {
            Unit unit = getValue();
            Sprite sprite = unitIconSet.icons[(int)unit];
            imgUnitLogo.sprite = sprite;
            imgUnitLogo.rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
        }
    }
}