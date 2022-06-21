using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameSetting
{
    public class NicknameSetting_Block : MonoBehaviour
    {
        public NicknameSetting nicknameSetting;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        [Header("Prefab")]
        public Button addItemButtonPrefab;

        NicknameSet.NicknameItem nicknameGlobal;
        NicknameSet.NicknameItem nicknameCharacter;

        public enum Mode { global,character }
        [System.NonSerialized] public Mode mode;

        public void Initialize(NicknameSet.NicknameItem nicknameGlobal)
        {
            mode = Mode.global;
            this.nicknameGlobal = nicknameGlobal;
            Refresh();
        }

        public void Initialize(NicknameSet.NicknameItem nicknameGlobal, NicknameSet.NicknameItem nicknameCharacter)
        {
            mode = Mode.character;
            this.nicknameGlobal = nicknameGlobal;
            this.nicknameCharacter = nicknameCharacter;
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();
            if(mode == Mode.global)
            {
                universalGenerator.Generate(nicknameGlobal.nickNames.Count,
                    (GameObject gameObject,int id)=>
                    {
                        NicknameSetting_Block_Item nicknameSetting_Block_Item = gameObject.GetComponent<NicknameSetting_Block_Item>();
                        nicknameSetting_Block_Item.Initialize(nicknameGlobal.nickNames[id], (string str) => { nicknameGlobal.nickNames[id] = str; });
                        nicknameSetting_Block_Item.removeButton.onClick.AddListener(() =>
                        {
                            nicknameGlobal.nickNames.RemoveAt(id);
                            Refresh();
                        });
                    });
                universalGenerator.AddItem(addItemButtonPrefab.gameObject, (GameObject gameObject) =>
                {
                    Button button = gameObject.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        nicknameGlobal.nickNames.Add("");
                        Refresh();
                    });
                });
            }
            else
            {
                universalGenerator.Generate(nicknameGlobal.nickNames.Count+ nicknameCharacter.nickNames.Count,
                    (GameObject gameObject, int id) =>
                    {
                        if (id < nicknameGlobal.nickNames.Count)
                        {
                            NicknameSetting_Block_Item nicknameSetting_Block_Item = gameObject.GetComponent<NicknameSetting_Block_Item>();
                            nicknameSetting_Block_Item.Initialize(nicknameGlobal.nickNames[id], null);
                            nicknameSetting_Block_Item.SetGlobalMode();
                        }
                        else
                        {
                            NicknameSetting_Block_Item nicknameSetting_Block_Item = gameObject.GetComponent<NicknameSetting_Block_Item>();
                            int newId = id - nicknameGlobal.nickNames.Count;
                            nicknameSetting_Block_Item.Initialize(nicknameCharacter.nickNames[newId], (string str) => { nicknameCharacter.nickNames[newId] = str; });
                            nicknameSetting_Block_Item.removeButton.onClick.AddListener(() =>
                            {
                                nicknameGlobal.nickNames.RemoveAt(newId);
                                Refresh();
                            });
                        }
                    });
                universalGenerator.AddItem(addItemButtonPrefab.gameObject, (GameObject gameObject) =>
                {
                    Button button = gameObject.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        nicknameCharacter.nickNames.Add("");
                        Refresh();
                    });
                });
            }
        }
    }
}