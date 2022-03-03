using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ToggleWithIconAndColor : MonoBehaviour
    {
        [HideInInspector] public Toggle toggle;
        public Image icon;
        public Text label;
        public Color selectColor;

        Color normalColor;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            normalColor = toggle.targetGraphic.color;
            toggle.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                    toggle.targetGraphic.color = selectColor;
                else
                    toggle.targetGraphic.color = normalColor;
            });
        }
        public void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }
        public void SetLabel(string text)
        {
            label.text = text;
        }
    }
}