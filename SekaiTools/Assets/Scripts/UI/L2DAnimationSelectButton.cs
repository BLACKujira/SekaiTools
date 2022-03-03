using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class L2DAnimationSelectButton : MonoBehaviour
    {
        [Header("Components")]
        public Button button;
        [SerializeField] Image targetImage;
        [SerializeField] Text textNoPreview;
        [Header("Settings")]
        [SerializeField] Sprite spriteNull;
        [SerializeField] Sprite spriteNoPreview;

        [HideInInspector] public L2DAnimationSet animationSet;
        [HideInInspector] public string animationName;

        public void RefreshImage()
        {
            if(string.IsNullOrEmpty(animationName))
            {
                targetImage.sprite = spriteNull;
                textNoPreview.text = string.Empty;
                return;
            }
            Sprite sprite = animationSet.GetPreview(animationName);
            targetImage.sprite = sprite == null ? spriteNoPreview : sprite;
            textNoPreview.text = sprite == null ? animationName : string.Empty;
        }
        public void SetAnimation(L2DAnimationSet animationSet, string animationName)
        {
            this.animationSet = animationSet;
            this.animationName = animationName;
            RefreshImage();
        }
    }
}