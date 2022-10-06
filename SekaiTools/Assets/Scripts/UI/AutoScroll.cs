using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class AutoScroll : MonoBehaviour
    {
        public RectTransform contentRectTransform;
        [Header("Settings")]
        public float stayTimeTop;
        public float scrollSpeed;
        public float stayTimeBottom;
        public float scrollViewHeight;

        public IEnumerator IPlay(Action onComplete = null)
        {
            contentRectTransform.anchoredPosition = new Vector2
                (contentRectTransform.anchoredPosition.x,0);
            yield return new WaitForSeconds(stayTimeTop);
            float stayOnBottonTime = 0;
            while (stayOnBottonTime < stayTimeBottom)
            {
                Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
                anchoredPosition.y += scrollSpeed * Time.deltaTime;
                float maxYPos = Mathf.Max(0, contentRectTransform.sizeDelta.y - scrollViewHeight);
                if (anchoredPosition.y >= maxYPos)
                {
                    anchoredPosition.y = maxYPos;
                    stayOnBottonTime += Time.deltaTime;
                }
                contentRectTransform.anchoredPosition = anchoredPosition;
                yield return 1;
            }

            if (onComplete != null)
                onComplete();
        }
    }
}