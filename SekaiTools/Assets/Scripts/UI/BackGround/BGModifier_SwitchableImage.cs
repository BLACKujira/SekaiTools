using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_SwitchableImage : BGModifierBase
    {
        [Header("Components")]
        public SpriteRenderer spriteRenderer;
        [Header("Prefab")]
        public SpriteRenderer spriteRendererPrefab;
        [Header("Settings")]
        public float fadeTime = 1;

        public class ChangeImageProcess : CustomYieldInstruction
        {
            bool _keepWaiting = true;

            public ChangeImageProcess()
            {
                onCompleteFadeOut += (so, sn) => _keepWaiting = false;
            }

            public override bool keepWaiting => _keepWaiting;

            /// <summary>
            /// 第一个参数为旧Sprite，第二个参数为新Sprite
            /// </summary>
            public delegate void SpriteChangeDelegate(Sprite oldSprite, Sprite newSprite);

            public event SpriteChangeDelegate onCompleteFadeOut;

            public ChangeImageProcess OnCompleteFadeOut(SpriteChangeDelegate spriteChangeDelegate)
            {
                onCompleteFadeOut += spriteChangeDelegate;
                return this;
            }

            public void Invoke(Sprite oldSprite, Sprite newSprite)
            {
                onCompleteFadeOut(oldSprite, newSprite);
            }
        }

        public ChangeImageProcess ChangeImage(Sprite sprite)
        {
            ChangeImageProcess changeImageProcess = new ChangeImageProcess();
            SpriteRenderer oldSpriteRenderer = spriteRenderer;
            SpriteRenderer newSpriteRenderer = Instantiate(spriteRendererPrefab, transform);
            newSpriteRenderer.sprite = sprite;
            newSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            spriteRenderer.DOFade(0, fadeTime).OnComplete(() =>
             {
                 newSpriteRenderer.sortingOrder = 0;
                 changeImageProcess.Invoke(oldSpriteRenderer.sprite, sprite);
                 Destroy(oldSpriteRenderer.gameObject);
             });
            spriteRenderer = newSpriteRenderer;
            return changeImageProcess;
        }

        public override void Deserialize(string serializedData)
        {
        }

        public override string Serialize()
        {
            return string.Empty;
        }
    }
}