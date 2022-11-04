using SekaiTools.Effect;
using UnityEngine;

namespace SekaiTools.UI.RandomCombine2CharPartyName
{
    public class RandomCombine2CharPartyName_Particle : MonoBehaviour
    {
        [Header("Components")]
        public ParticleSystem psCharL;
        public ParticleSystem psCharR;
        public ParticleSystem psTextL;
        public ParticleSystem psTextR;
        [Header("Settings")]
        public IconSet charIconSet;
        public HDRColorSet colorSet;


        ParticleSystem.ShapeModule psShapeCharL;
        ParticleSystem.ShapeModule psShapeCharR;

        HDRColorParticle hdrcpCharL;
        HDRColorParticle hdrcpCharR;
        HDRColorParticle hdrcpTextL;
        HDRColorParticle hdrcpTextR;

        private void Awake()
        {
            psShapeCharL = psCharL.shape;
            psShapeCharR = psCharR.shape;

            hdrcpCharL = psCharL.GetComponent<HDRColorParticle>();
            hdrcpCharR = psCharR.GetComponent<HDRColorParticle>();
            hdrcpTextL = psTextL.GetComponent<HDRColorParticle>();
            hdrcpTextR = psTextR.GetComponent<HDRColorParticle>();
        }


        public void SetData(int charLId, int charRId)
        {
            psShapeCharL.sprite = charIconSet.icons[charLId];
            psShapeCharR.sprite = charIconSet.icons[charRId];

            hdrcpCharL.hDRColor = colorSet.colors[charLId];
            hdrcpCharR.hDRColor = colorSet.colors[charRId];
            hdrcpTextL.hDRColor = colorSet.colors[charLId];
            hdrcpTextR.hDRColor = colorSet.colors[charRId];
        }

        public void Play()
        {
            psCharL.Play();
            psCharR.Play();
            psTextL.Play();
            psTextR.Play();
        }
    }
}