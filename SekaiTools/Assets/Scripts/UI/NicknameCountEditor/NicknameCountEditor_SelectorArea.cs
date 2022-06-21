using System.Collections;
using System.Collections.Generic;
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
        [Header("Prefab")]
        public Window windowSelectorMutiPrefab;
        public Window windowSelectorSinglePrefab;

        public void Initialize()
        {
            for (int i = 1; i < buttonsSingle.Length; i++)
            {
                int id = i;
                Button button = buttonsSingle[i];
                button.onClick.AddListener(() =>
                {
                    NCESelector.NCESelector nCESelector = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(windowSelectorSinglePrefab);
                    nCESelector.Initialize(nicknameCountEditor.countData, nicknameCountEditor.currentCharacterId, id);
                });
            }
            buttonMuti.onClick.AddListener(()=>
            {
                NCESelector.NCESelector nCESelectorMuti = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(windowSelectorMutiPrefab);
                nCESelectorMuti.Initialize(nicknameCountEditor.countData, nicknameCountEditor.currentCharacterId);
            });
            Refresh();
        }


        public void Refresh()
        {
            for (int i = 1; i < 27; i++)
            {
                ButtonWithIconAndText buttonWithIconAndText = buttonsSingle[i].GetComponent<ButtonWithIconAndText>();
                buttonWithIconAndText.Label = nicknameCountEditor.countData[nicknameCountEditor.currentCharacterId, i].Total.ToString();
            }
        }
    }
}