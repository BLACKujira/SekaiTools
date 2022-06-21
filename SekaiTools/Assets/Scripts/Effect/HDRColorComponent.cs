using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Effect
{
    public abstract class HDRColorComponent : MonoBehaviour , IHDRColor
    {
        public float lightOffset = 0;

        Material hDRColorMaterial;

        public Color hDRColor
        {
            get
            {
                Color color = hDRColorMaterial.GetColor("hDRColor");
                float h, s, v;
                Color.RGBToHSV(color, out h, out s, out v);
                return Color.HSVToRGB(h, s, v - lightOffset);
            }

            set
            {
                float h, s, v;
                Color.RGBToHSV(value, out h, out s, out v);
                hDRColorMaterial.SetColor("hDRColor", Color.HSVToRGB(h, s, v + lightOffset));
            }
        }

        private void Awake()
        {
            hDRColorMaterial = InitializeMaterial();
        }

        public abstract Material InitializeMaterial();
    }
}