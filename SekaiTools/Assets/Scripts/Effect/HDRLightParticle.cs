using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Effect
{
    public interface IHDRLight
    {
        float lightStrength { get;set; }
    }

    public class HDRLightParticle : MonoBehaviour, IHDRLight
    {
        public float startStrength = 1;
        public float lightStrength { get=>hDRLightMaterial.GetFloat("strength"); set { hDRLightMaterial.SetFloat("strength", value); } }

        Material hDRLightMaterial;

        private void Awake()
        {
            ParticleSystemRenderer particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            Material material = new Material(particleSystemRenderer.material);
            particleSystemRenderer.material = material;
            hDRLightMaterial = material;
            lightStrength = startStrength;
        }
    }
}