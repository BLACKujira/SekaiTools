using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using System;

namespace SekaiTools.UI.Live2DMotionSelect
{
    public class Live2DMotionSelect_SelectArea : MonoBehaviour
    {
        List<L2DAnimationSelectButton> l2DAnimationSelectButtons = new List<L2DAnimationSelectButton>();
        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public L2DAnimationSelectButton buttonPrefab;
        [Header("Settings")]
        public int numberPerLine;
        public float distanceX;
        public float distanceY;

        Action<string> onButtonClick;

        public void Initialize(Action<string> onButtonClick)
        {
            this.onButtonClick = onButtonClick;
        }

        public void SetButtons(L2DAnimationSet animationSet, List<AnimationClip> animations)
        {
            //生成足够多的按钮,并排序按钮
            while (l2DAnimationSelectButtons.Count<animations.Count+1)
            {
                L2DAnimationSelectButton button = Instantiate(buttonPrefab, scorllContent);
                button.button.onClick.AddListener(()=>onButtonClick(button.animationName));
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    distanceX * (l2DAnimationSelectButtons.Count % numberPerLine),
                    -distanceY * (l2DAnimationSelectButtons.Count / numberPerLine));
                l2DAnimationSelectButtons.Add(button);
            }
            scorllContent.sizeDelta = new Vector2(
                scorllContent.sizeDelta.x,
                distanceY * (((animations.Count+1) / numberPerLine) + (((animations.Count + 1) % numberPerLine) == 0 ? 0: 1))) ;

            //设置按钮图像
            for (int i = 1; i < animations.Count+1; i++)
            {
                l2DAnimationSelectButtons[i].SetAnimation(animationSet, animations[i-1].name);
            }
            l2DAnimationSelectButtons[0].SetAnimation(animationSet, null);

            //关闭多余的按钮
            int id = 0;
            for (; id < animations.Count+1; id++)
            {
                l2DAnimationSelectButtons[id].gameObject.SetActive(true);
            }
            for (; id < l2DAnimationSelectButtons.Count; id++)
            {
                l2DAnimationSelectButtons[id].gameObject.SetActive(false);
            }
        }
    }
}