using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.UnitSelect
{
    public class UnitSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Button[] unitButtons = new Button[7];

        public void Initialize(Action<Unit> onApply)
        {
            for (int i = 0; i < unitButtons.Length; i++) 
            {
                int id = i;
                Button button = unitButtons[id];
                if (button == null) continue;
                button.onClick.AddListener(() =>
                {
                    onApply((Unit)id);
                    window.Close();
                });
            }
        }
    }
}