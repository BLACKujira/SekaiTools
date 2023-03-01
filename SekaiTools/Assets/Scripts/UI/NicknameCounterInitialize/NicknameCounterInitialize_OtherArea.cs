using SekaiTools.UI.NicknameSetting;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class NicknameCounterInitialize_OtherArea : MonoBehaviour
    {
        public NicknameCounterInitialize_Old nicknameCounterInitialize;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        [Header("Prefab")]
        public Button addItemButtonPrefab;

        [System.NonSerialized] public List<string> excludeStrings = new List<string>(new string[] { "ー", "〜", "～" });

        private void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();
            universalGenerator.Generate(excludeStrings.Count,
                (GameObject gameObject, int id) =>
                {
                    NicknameSetting_Block_Item nicknameSetting_Block_Item = gameObject.GetComponent<NicknameSetting_Block_Item>();
                    nicknameSetting_Block_Item.Initialize(excludeStrings[id], (string str) => { excludeStrings[id] = str; });
                    nicknameSetting_Block_Item.removeButton.onClick.AddListener(() =>
                    {
                        excludeStrings.RemoveAt(id);
                        Refresh();
                    });
                });
            universalGenerator.AddItem(addItemButtonPrefab.gameObject, (GameObject gameObject) =>
            {
                Button button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    excludeStrings.Add("");
                    Refresh();
                });
            });
        }
    }
}