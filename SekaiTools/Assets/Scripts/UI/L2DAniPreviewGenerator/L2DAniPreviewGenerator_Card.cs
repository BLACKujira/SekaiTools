using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DAniPreviewGenerator
{
    public class L2DAniPreviewGenerator_Card : MonoBehaviour
    {
        public Image imageEdge;
        public Text textType;
        public Text textName;

        string textTypeOrigin;
        string textNameOrigin;
        Color colorOrigin;

        private void Awake()
        {
            textTypeOrigin = textType.text;
            textNameOrigin = textName.text;
            colorOrigin = imageEdge.color;
        }

        public void ResetTextAndColor()
        {
            textType.text = textNameOrigin;
            textName.text = textNameOrigin;
            imageEdge.color = colorOrigin;
        }

        public void SetData(string type,string name,bool useNameStringColor = false)
        {
            textType.text = type;
            textName.text = name;
            byte[] hashCode = BitConverter.GetBytes(useNameStringColor?name.GetHashCode():type.GetHashCode());
            Color color = Color.HSVToRGB((float)hashCode[3] / 255, (float)hashCode[2] / 255, 0.6f);
            if(imageEdge) imageEdge.color = color;
        }
    }
}