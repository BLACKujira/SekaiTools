using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ButtonWithIconAndText : MonoBehaviour
    {
        [SerializeField] Text label;
        [SerializeField] Image icon;

        public string Label { get => label.text; set => label.text = value; }
        public Sprite Icon { get => icon.sprite; set => icon.sprite = value; }
    
        public void HideIcon()
        {
            icon.enabled = false;
        }
        public void ShowIcon()
        {
            icon.enabled = true;
        }
    }
}