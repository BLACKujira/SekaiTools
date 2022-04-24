using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SekaiTools.Kizuna;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayerBase_Player_MainBase : MonoBehaviour
    {
        [Header("Components")]
        public Transforms[] resizeTransforms;
        public BondsHonorBase bondsHonorOriLv1;
        public BondsHonorBase bondsHonorOriLv2;
        public BondsHonorBase bondsHonorOriLv3;
        public BondsHonorBase bondsHonorTraLv1;
        public BondsHonorBase bondsHonorTraLv2;
        public BondsHonorBase bondsHonorTraLv3;
        public BondsHonorSub bondsHonorSubL;
        public BondsHonorSub bondsHonorSubR;
        public Image nameLabelL;
        public Image nameLabelR;
        [Header("Settings")]
        public float resizeTime;
        public float resizeDelay;
        public IconSet nameLabelSet;

        public BondsHonorBase[] bondsHonorsMain
        {
            get
            {
                return new BondsHonorBase[]
                {
                    bondsHonorOriLv1,
                    bondsHonorOriLv2,
                    bondsHonorOriLv3,
                    bondsHonorTraLv1,
                    bondsHonorTraLv2,
                    bondsHonorTraLv3,
                };
            }
        }


        IEnumerator IResize(float endValue,Action onFinish = null)
        {
            foreach (var transforms in resizeTransforms)
            {
                DOTween.To(()=>transforms.scale,(value)=>transforms.scale = value,endValue,resizeTime);
                yield return new WaitForSeconds(resizeDelay);
            }
            if (onFinish != null) onFinish();
        }

        public void Show(Action onFinish = null)
        {
            StopAllCoroutines();
            StartCoroutine(IResize(1,onFinish));
        }
        public void Hide(Action onFinish = null)
        {
            StopAllCoroutines();
            StartCoroutine(IResize(0,onFinish));
        }

        protected void SetScene(KizunaSceneBase kizunaScene)
        {
            BondsHonorBase[] bondsHonors = bondsHonorsMain;
            foreach (var bondsHonor in bondsHonors)
            {
                bondsHonor.SetCharacter(kizunaScene.charAID, kizunaScene.charBID);
            }
            bondsHonorSubL.SetCharacter(kizunaScene.charAID,kizunaScene.charBID);
            bondsHonorSubR.SetCharacter(kizunaScene.charBID,kizunaScene.charAID);

            Sprite spriteA = nameLabelSet.icons[kizunaScene.charAID];
            nameLabelL.sprite = spriteA;
            nameLabelL.rectTransform.sizeDelta = new Vector2(spriteA.texture.width, spriteA.texture.height);
            Sprite spriteB = nameLabelSet.icons[kizunaScene.charBID];
            nameLabelR.sprite = spriteB;
            nameLabelR.rectTransform.sizeDelta = new Vector2(spriteB.texture.width, spriteB.texture.height);
        }
        
        [System.Serializable]
        public class Transforms
        {
            [SerializeField] List<Transform> transforms;
            [System.NonSerialized] List<Vector3> scaleMagnification;
            [System.NonSerialized]  float _scale = 1;

            public float scale
            {
                get
                {
                    return _scale;
                }

                set
                {
                    if (scaleMagnification == null)
                    {
                        scaleMagnification = new List<Vector3>();
                        foreach (var transform in transforms)
                        {
                            scaleMagnification.Add(transform.localScale);
                        }
                    }
                    for (int i = 0; i < transforms.Count; i++)
                    {
                        transforms[i].localScale = scaleMagnification[i] * value;
                    }
                    _scale = value;
                }
            }
        }
    }
}