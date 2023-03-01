using SekaiTools.UI.BackGround;
using SekaiTools.UI.BackGroundSettings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{
    public class GIP_KizunaBGParts : MonoBehaviour
    {
        [Header("Components")]
        public UniversalGenerator decorationsButtonGenerator;
        [Header("Settings")]
        public BackGroundPartSet bGSetHDR;
        [Header("Prefabs")]
        public Button addDecorationButton;
        public Window decorationSelector;

        List<BackGroundPart> backGroundParts = new List<BackGroundPart>();
        public List<BackGroundPart> BackGroundParts => backGroundParts;

        private void Awake()
        {
            RefreshButtons();
        }

        void RefreshButtons()
        {
            decorationsButtonGenerator.ClearItems();
            decorationsButtonGenerator.Generate(backGroundParts.Count,
                (GameObject gameObject, int id) =>
                {
                    BackGroundSettings_PartItem item = gameObject.GetComponent<BackGroundSettings_PartItem>();
                    item.Label = backGroundParts[id].itemName;
                    item.Icon = backGroundParts[id].preview;

                    if (backGroundParts[id].disableRemove)
                    {
                        item.buttonRemove.interactable = false;
                    }
                    else
                    {
                        item.buttonRemove.onClick.AddListener(() =>
                        {
                            backGroundParts.RemoveAt(id);
                            RefreshButtons();
                        });
                    }

                    if (id != 0)
                    {
                        item.buttonMoveLeft.onClick.AddListener(() =>
                        {
                            BackGroundPart backGroundPart = backGroundParts[id];
                            backGroundParts[id] = backGroundParts[id - 1];
                            backGroundParts[id - 1] = backGroundPart;
                            RefreshButtons();
                        });
                    }
                    else
                    {
                        item.buttonMoveLeft.interactable = false;
                    }

                    if (id != backGroundParts.Count - 1)
                    {
                        item.buttonMoveRight.onClick.AddListener(() =>
                        {
                            BackGroundPart backGroundPart = backGroundParts[id];
                            backGroundParts[id] = backGroundParts[id + 1];
                            backGroundParts[id + 1] = backGroundPart;
                            RefreshButtons();
                        });
                    }
                    else
                    {
                        item.buttonMoveRight.interactable = false;
                    }
                });
            decorationsButtonGenerator.AddItem(addDecorationButton.gameObject, (GameObject gameObject) =>
            {
                gameObject.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    UniversalSelector universalSelector = WindowController.CurrentWindow.OpenWindow<UniversalSelector>(decorationSelector);
                    universalSelector.Title = "添加背景装饰";
                    universalSelector.Generate(bGSetHDR.backGroundParts.Count, (Button button, int id) =>
                    {
                        ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                        buttonWithIconAndText.Label = bGSetHDR.backGroundParts[id].itemName;
                        buttonWithIconAndText.Icon = bGSetHDR.backGroundParts[id].preview;
                    },
                    (int id) =>
                    {
                        backGroundParts.Add(bGSetHDR.backGroundParts[id]);
                        RefreshButtons();
                    });
                });
            });
        }
    }
}