using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.LogWindow
{
    public class LogWindow : MessageBox.MessageBox
    {
        public RectTransform textTransform;
        public RectTransform scrollTransform;
        public float edgeSize = 25;

        public override string message
        {
            get => _message.text;
            set
            {
                _message.text = value;
            }
        }

        private void Update()
        {
            textTransform.sizeDelta = new Vector2(textTransform.sizeDelta.x, _message.preferredHeight);
            scrollTransform.sizeDelta = new Vector2(scrollTransform.sizeDelta.x, textTransform.sizeDelta.y + edgeSize);
        }
    }
}