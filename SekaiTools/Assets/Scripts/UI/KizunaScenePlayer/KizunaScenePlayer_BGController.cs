using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.BackGround;
using SekaiTools.UI.BackGroundSettings;
using SekaiTools.Kizuna;
using SekaiTools.Effect;
using DG.Tweening;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayer_BGController : MonoBehaviour
    {
        public BackGroundPart gridPrefab;
        public HDRColorSet hDRColorSet;
        public float fadeTime;

        BackGroundPart gridPart;
        List<BackGroundPart> particleLPart;
        List<BackGroundPart> particleRPart;

        public void Initialize(BackGroundPart[] backGroundParts)
        {
            gridPart = BackGroundController.backGroundController.AddDecoration(gridPrefab);
            particleLPart = new List<BackGroundPart>();
            particleRPart = new List<BackGroundPart>();
            foreach (var backGroundPart in backGroundParts)
            {
                BackGroundPart partL = BackGroundController.backGroundController.AddDecoration(backGroundPart);
                particleLPart.Add(partL);
                BackGroundPart partR = BackGroundController.backGroundController.AddDecoration(backGroundPart);
                particleRPart.Add(partR);
                partL.disableRemove = true;
                partR.disableRemove = true;
            }
            gridPart.disableRemove = true;
        }

        public void SetScene(KizunaSceneBase kizunaScene)
        {
            foreach (var part in particleLPart)
            {
                IHDRColor iHDRColorL = part.GetModifier<IHDRColor>();
                DOTween.To(() => iHDRColorL.hDRColor, color => iHDRColorL.hDRColor = color, hDRColorSet.colors[kizunaScene.charAID], fadeTime);
            }
            foreach (var part in particleRPart)
            {
                IHDRColor iHDRColorR = part.GetModifier<IHDRColor>();
                DOTween.To(() => iHDRColorR.hDRColor, color => iHDRColorR.hDRColor = color, hDRColorSet.colors[kizunaScene.charBID], fadeTime);
            }
        }

        public void FadeInGrid(float time)
        {
            gridPart.GetModifier<BGModifier_SpriteColor>().targetSpriteRenderer.DOFade(1, time);
        }

        public void FadeOutGrid(float time)
        {
            gridPart.GetModifier<BGModifier_SpriteColor>().targetSpriteRenderer.DOFade(0, time);
        }

        private void OnDestroy()
        {
            BackGroundController.backGroundController.RemoveDecoration(gridPart);
            foreach (var part in particleLPart)
            {
                BackGroundController.backGroundController.RemoveDecoration(part);
            }
            foreach (var part in particleRPart)
            {
                BackGroundController.backGroundController.RemoveDecoration(part);
            }
        }
    }
}