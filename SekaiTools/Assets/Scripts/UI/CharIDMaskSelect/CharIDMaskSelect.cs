using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CharIDMaskSelect
{
    public class CharIDMaskSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Toggle[] toggles = new Toggle[27];

        Action<bool[]> onApply_Bool;
        Action<int[]> onApply_Int;

        public void Initialize(bool[] initValue, Action<bool[]> onApply)
        {
            for (int i = 0; i < Mathf.Min(initValue.Length, toggles.Length); i++)
            {
                if (toggles[i] != null)
                    toggles[i].isOn = initValue[i];
            }
            Initialize(onApply);
        }

        public void Initialize(Action<bool[]> onApply)
        {
            onApply_Bool = onApply;
        }

        public void Initialize(int[] initValue, Action<int[]> onApply)
        {
            foreach (var toggle in toggles)
            {
                if (toggle) toggle.isOn = false;
            }

            foreach (var id in initValue)
            {
                if (id < toggles.Length && toggles[id] != null)
                    toggles[id].isOn = true;
            }
            Initialize(onApply);
        }

        public void Initialize(Action<int[]> onApply)
        {
            onApply_Int = onApply;
        }

        public void Initialize(int[] initValue, Action<bool[]> onApply)
        {
            foreach (var toggle in toggles)
            {
                if (toggle != null) toggle.isOn = false;
            }

            foreach (var id in initValue)
            {
                if (id < toggles.Length && toggles[id] != null)
                    toggles[id].isOn = true;
            }
            Initialize(onApply);
        }

        public void Initialize(bool[] initValue, Action<int[]> onApply)
        {
            for (int i = 0; i < Mathf.Min(initValue.Length, toggles.Length); i++)
            {
                if (toggles[i] != null)
                    toggles[i].isOn = initValue[i];
            }
            Initialize(onApply);
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
            if (onApply_Bool != null)
            {
                bool[] charIDMask = new bool[toggles.Length];
                for (int i = 0; i < toggles.Length; i++)
                {
                    if (toggles[i] && toggles[i].isOn)
                        charIDMask[i] = true;
                    else
                        charIDMask[i] = false;
                }
                onApply_Bool(charIDMask);
            }

            if (onApply_Int != null)
            {
                List<int> selectIds = new List<int>();
                for (int i = 0; i < toggles.Length; i++)
                {
                    int id = i;
                    Toggle toggle = toggles[id];
                    if (toggle && toggle.isOn) selectIds.Add(id);
                }
                onApply_Int(selectIds.ToArray());
            }

            window.Close();
        }
    }
}