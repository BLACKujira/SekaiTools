using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CharacterSelector
{
    public class CharacterSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Button[] buttons = new Button[27];

        public void Initialize(Action<int> setCharacter)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int id = i;
                if(buttons[i])
                {
                    buttons[i].onClick.AddListener(() =>
                    {
                        setCharacter(id);
                        window.Close();
                    });
                }
            }
        }
    }
}