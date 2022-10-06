using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class GraphicColorMapping : MonoBehaviour
    {
        [SerializeField] Graphic graphic;
        Color color;
        public Graphic Graphic => graphic;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                SetColor();
            }
        }

        float alpha;
        public float Alpha 
        { 
            get => alpha; 
            set
            {
                alpha = value;
                SetColor();
            }
        }

        private void SetColor()
        {
            Color color = this.color;
            color.a = Mathf.Lerp(0, this.color.a, alpha);
            graphic.color = color;
        }

        private void Awake()
        {
            color = graphic.color;
        }
    }
}