using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioCMDInput : MonoBehaviour
    {
        [Header("Components")]
        public UniversalGeneratorV2 universalGenerator;
        [Header("Settings")]
        public RadioCMDInputSet radioCMDInputSet;
        [Header("Prefab")]
        public Button addItemButtonPrefab;
        public Window cMDInputItemSelectorWindowPrefab;

        public RadioCMDInputInitializePart[] radioCMDInputInitializeParts
        {
            get
            {
                List<RadioCMDInputInitializePart> radioCMDInputInitializeParts = new List<RadioCMDInputInitializePart>();
                foreach (var item in gIP_RadioCMDInput_Items)
                {
                    radioCMDInputInitializeParts.Add(new RadioCMDInputInitializePart
                        (item.ManagerObjectPrefab, item.Settings));
                }
                return radioCMDInputInitializeParts.ToArray();
            }
        }

        List<GIP_RadioCMDInput_ItemBase> gIP_RadioCMDInput_Items = new List<GIP_RadioCMDInput_ItemBase>();

        GameObject addItemButton;
        private void Awake()
        {
            AddAddItemButton();
        }

        private void AddAddItemButton()
        {
            addItemButton = universalGenerator.AddItem(addItemButtonPrefab.gameObject,
                            (gobj) =>
                            {
                                Button button = gobj.GetComponent<Button>();
                                button.onClick.AddListener(() =>
                                {
                                    AddItem();
                                });
                            });
        }

        public void AddItem()
        {
            UniversalSelector universalSelector 
                = WindowController.windowController.currentWindow.OpenWindow<UniversalSelector>
                (cMDInputItemSelectorWindowPrefab);
            universalSelector.Generate(radioCMDInputSet.radioCMDInputItems.Count,
                (button, id) =>
                {
                    ItemWithTitleAndContent itemWithTitleAndContent = button.GetComponent<ItemWithTitleAndContent>();
                    itemWithTitleAndContent.text_Title.text = radioCMDInputSet[id].managerObjectPrefab.itemName;
                    itemWithTitleAndContent.text_Content.text = radioCMDInputSet[id].managerObjectPrefab.description;
                },
                (id) =>
                {
                    GameObject gameObject = universalGenerator.AddItem(radioCMDInputSet[id].configItemPrefab.gameObject);
                    GIP_RadioCMDInput_ItemBase gIP_RadioCMDInput_ItemBase = gameObject.GetComponent<GIP_RadioCMDInput_ItemBase>();
                    universalGenerator.RemoveItem(addItemButton);
                    gIP_RadioCMDInput_Items.Add(gIP_RadioCMDInput_ItemBase);
                    AddAddItemButton();
                    gIP_RadioCMDInput_ItemBase.Initialize(radioCMDInputSet[id].managerObjectPrefab,
                        () =>
                        {
                            universalGenerator.RemoveItem(gameObject);
                            gIP_RadioCMDInput_Items.Remove(gIP_RadioCMDInput_ItemBase);
                        });
                });
        }
    }
}