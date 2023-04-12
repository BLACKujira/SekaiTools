using SekaiTools.Effect;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress_Line_End : MonoBehaviour
    {
        [Header("Components")]
        public HDRColorParticle hDRColorParticle;
        public ParticleSystem targetParticleSystem;

        float emissionRate;

        ParticleSystem.EmissionModule emission;

        private void Awake()
        {
            emission = targetParticleSystem.emission;
            emissionRate = emission.rateOverTime.constant;
        }

        public void TurnOn()
        {
            emission.rateOverTime = emissionRate;
        }

        public void TurnOff()
        {
            emission.rateOverTime = 0;
        }
    }
}