using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ToggleGroupValueLoader : MonoBehaviour
    {
        public List<Toggle> toggles = new List<Toggle>();
        public int Value
        {
            get
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    Toggle toggle = toggles[i];
                    if (toggle&&toggle.isOn) return i;
                }
                return -1;
            }
        }
    }
}