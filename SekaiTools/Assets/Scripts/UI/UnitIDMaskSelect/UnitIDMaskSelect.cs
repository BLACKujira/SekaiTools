using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.UnitIDMaskSelect
{
    public class UnitIDMaskSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Toggle[] toggles = new Toggle[27];

        Action<bool[]> onApply;

        public void Initialize(bool[] initValue, Action<bool[]> onApply)
        {
            for (int i = 0; i < Mathf.Min(initValue.Length, toggles.Length); i++)
            {
                if(toggles[i] != null)
                    toggles[i].isOn = initValue[i];
            }
            Initialize(onApply);
        }

        public void Initialize(Action<bool[]> onApply)
        {
            this.onApply = onApply;
        }

        public void SelectNone()
        {
            foreach (var toggle in toggles)
            {
                if (toggle)
                    toggle.isOn = false;
            }
        }

        public void SelectAll()
        {
            foreach (var toggle in toggles)
            {
                if (toggle)
                    toggle.isOn = true;
            }
        }

        public void Apply()
        {
            bool[] unitIDMask = new bool[toggles.Length];
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i] && toggles[i].isOn)
                    unitIDMask[i] = true;
                else
                    unitIDMask[i] = false;
            }
            onApply(unitIDMask);
            window.Close();
        }
    }
}