using UnityEngine;

namespace SekaiTools.Effect
{
    public interface IHDRColor
    {
        Color hDRColor { get; set; }
    }

    public class HDRColorParticle : MonoBehaviour, IHDRColor
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
            ParticleSystemRenderer particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            Material material = new Material(particleSystemRenderer.material);
            particleSystemRenderer.material = material;
            hDRColorMaterial = material;
        }
    }
}