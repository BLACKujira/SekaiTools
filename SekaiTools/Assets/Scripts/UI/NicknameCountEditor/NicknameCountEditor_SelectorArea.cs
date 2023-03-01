using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountEditor
{
    public class NicknameCountEditor_SelectorArea : MonoBehaviour
    {
        public NicknameCountEditor nicknameCountEditor;
        [Header("Components")]
        public Button buttonMuti;
        public Button[] buttonsSingle = new Button[27];
        public ToggleGenerator toggleGenerator;
        [Header("Settings")]
        public IconSet iconSet;
        [Header("Prefab")]
        public Window windowSelectorMutiPrefab;
        public Window windowSelectorSinglePrefab;

        int currentCharacterId = 1;

        public void Initialize()
        {
            for (int i = 1; i < buttonsSingle.Length; i++)
            {
                int id = i;
                Button button = buttonsSingle[i];
                button.onClick.AddListener(() =>
                {
                    NCESelector.NCESelector nCESelector = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(windowSelectorSinglePrefab);
                    nCESelector.Initialize(nicknameCountEditor.CountData, currentCharacterId, id);
                });
            }
            buttonMuti.onClick.AddListener(()=>
            {
                NCESelector.NCESelector nCESelectorMuti = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(windowSelectorMutiPrefab);
                nCESelectorMuti.Initialize(nicknameCountEditor.CountData, currentCharacterId);
            });
            InitializeToggles();
            Refresh();
        }

        public void InitializeToggles()
        {
            toggleGenerator.Generate(26,
                (Toggle toggle, int id) =>
                {
                    toggle.transform.GetChild(0).GetComponent<Image>().sprite = iconSet.icons[id + 1];
                    toggle.GetComponent<Image>().color = ConstData.characters[id + 1].imageColor;
                },
                (bool value, int id) =>
                {
                    if (value)
                    {
                        currentCharacterId = id + 1;
                        Refresh();
                    }
                });
            toggleGenerator.toggles[0].isOn = true;
        }

        public void Refresh()
        {
            for (int i = 1; i < 27; i++)
            {
                ButtonWithIconAndText buttonWithIconAndText = buttonsSingle[i].GetComponent<ButtonWithIconAndText>();
                buttonWithIconAndText.Label = nicknameCountEditor.CountData[currentCharacterId, i].Total.ToString();
            }
        }
    }
}