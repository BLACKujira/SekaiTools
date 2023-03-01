using SekaiTools.Cutin;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_CutinScenePlayer : GSW_Item
    {
        [Header("Components")]
        public Text txtDisplayName;
        public Image imgPreview;
        public Button btnChange;
        [Header("Settings")]
        public CutinScenePlayerSet playerSet;
        [Header("Prefab")]
        public Window selectorPrefab;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_CutinScenePlayer configUIItem_CutinScenePlayer = configUIItem as ConfigUIItem_CutinScenePlayer;
            if (configUIItem_CutinScenePlayer == null) throw new ItemTypeMismatchException();

            btnChange.onClick.AddListener(() =>
            {
                UniversalSelector universalSelector = 
                    WindowController.CurrentWindow.OpenWindow<UniversalSelector>(selectorPrefab);
                universalSelector.Generate(playerSet.items.Length,
                    (btn, id) =>
                    {
                        ButtonWithIconAndText buttonWithIconAndText = btn.GetComponent<ButtonWithIconAndText>();
                        buttonWithIconAndText.Icon = playerSet.items[id].preview;
                        buttonWithIconAndText.Label = playerSet.items[id].displayName;
                    },
                    (id) =>
                    {
                        configUIItem_CutinScenePlayer.setValue(playerSet.items[id].key);
                        RefreshInfo(configUIItem_CutinScenePlayer.getValue());
                    });
            });

            RefreshInfo(configUIItem_CutinScenePlayer.getValue());
        }

        void RefreshInfo(string playerKey)
        {
            CutinScenePlayerSet_Item cutinScenePlayerSet_Item = playerSet.GetItem(playerKey);
            if(cutinScenePlayerSet_Item==null)
            {
                txtDisplayName.text = Message.Error.STR_ERROR;
                imgPreview.sprite = null;
            }
            else
            {
                txtDisplayName.text = cutinScenePlayerSet_Item.displayName;
                imgPreview.sprite = cutinScenePlayerSet_Item.preview;
            }
        }
    }
}