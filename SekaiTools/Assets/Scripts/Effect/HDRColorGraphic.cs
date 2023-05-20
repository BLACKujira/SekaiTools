using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.Effect
{
    public class HDRColorGraphic : MonoBehaviour, IHDRColor
    {
        public float lightOffset = 0;

        Material hDRColorMaterial;
        public Material HDRColorMaterial => hDRColorMaterial;
        public bool IfReady => HDRColorMaterial != null;

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

        private void OnDestroy()
        {
            if (HDRColorMaterial != null)
            {
                Destroy(hDRColorMaterial);
            }
        }

        private void Awake()
        {
            Graphic graphic = GetComponent<Graphic>();
            Material material = new Material(graphic.material);
            graphic.material = material;
            hDRColorMaterial = material;
        }
    }
}