using SekaiTools.Effect;
using UnityEngine;
using UnityEngine.Rendering;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Item_Head : MonoBehaviour
    {
        public HDRColorParticle[] hDRColorParticles;
        public ParticleSystem targetParticleSystem;

        ParticleSystem.EmissionModule emission;
        public float EmissionRate 
        {
            get => emission.rateOverTime.constant;
            set
            {
                emission.rateOverTime = value;
            }
        }

        public Color HDRColor
        {
            get => hDRColorParticles[0].hDRColor;
            set
            {
                foreach (var particle in hDRColorParticles)
                {
                    particle.hDRColor = value;
                }
            }
        }

        private void Awake()
        {
            emission = targetParticleSystem.emission;
        }
    }
}