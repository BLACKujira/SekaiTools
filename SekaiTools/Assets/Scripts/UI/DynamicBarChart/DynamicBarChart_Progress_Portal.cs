using SekaiTools.Effect;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress_Portal : MonoBehaviour
    {
        [Header("Components")]
        public HDRColorParticle[] hDRColorParticles;
        public ParticleSystem[] particlesNormal;
        public ParticleSystem[] particlesBreak;

        public Color HDRColor
        {
            get => hDRColorParticles[0].hDRColor;
            set
            {
                foreach (var particle in hDRColorParticles) 
                {
                    if(particle.IfReady)
                        particle.hDRColor = value;
                }
            }
        }

        float[] particlesNormalEmission;

        private void Awake()
        {
            particlesNormalEmission = new float[particlesNormal.Length];
            for (int i = 0; i < particlesNormal.Length; i++)
            {
                particlesNormalEmission[i] = particlesNormal[i].emission.rateOverTime.constant;
            }
        }

        public void ResetPortal()
        {
            for (int i = 0; i < particlesNormal.Length; i++) 
            {
                ParticleSystem particle = particlesNormal[i];
                ParticleSystem.EmissionModule emission = particle.emission;
                emission.rateOverTime = particlesNormalEmission[i];
            }
            foreach (var particle in particlesBreak)
            {
                particle.gameObject.SetActive(false);
            }
        }

        public void BreakPortal()
        {
            foreach (var particle in particlesNormal)
            {
                ParticleSystem.EmissionModule emission = particle.emission;
                emission.rateOverTime = 0;
            }
            foreach (var particle in particlesBreak)
            {
                particle.gameObject.SetActive(true);
            }
        }
    }
}