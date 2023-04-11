using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Item : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform barRectTransform;
        public Text txtNumber;
        [Header("Settings")]
        public float barLength = 1475f;
        public float fadeTime = 0.5f;
        public float frameHoldTime = 0.25f;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        Graphic[] graphics;

        bool isFadingOut = false;
        public bool IsFadingOut => isFadingOut;

        protected float lastTargetLength;
        protected float lastTargetNumber;
        protected float targetLength;
        protected float targetNumber;
        float currentTime = 0;
        protected float FrameTimePercent => currentTime / frameHoldTime;

        private void Awake()
        {
            graphics = GetComponentsInChildren<Graphic>();
            foreach (Graphic graphic in graphics)
            {
                Color color = graphic.color;
                color.a = 0;
                graphic.color = color;
            }
            barRectTransform.sizeDelta = new Vector2(0, barRectTransform.sizeDelta.y);
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            if (barRectTransform != null)
            {
                barRectTransform.sizeDelta = new Vector2(
                     Mathf.Lerp(lastTargetLength, targetLength, Mathf.Min(FrameTimePercent, 1)),
                     barRectTransform.sizeDelta.y);
            }
            if (txtNumber != null)
            {
                txtNumber.text = GetNumberString(Mathf.Lerp(lastTargetNumber, targetNumber, Mathf.Min(FrameTimePercent, 1)));
            }
        }

        protected virtual string GetNumberString(float number)
        {
            return number.ToString("00.00");
        }

        public virtual void UpdateData(DataFrame dataFrame, string key, float maxNumber)
        {
            lastTargetLength = targetLength;
            lastTargetNumber = targetNumber;
            targetLength = barLength * (dataFrame.data[key] / maxNumber);
            targetNumber = dataFrame.data[key];
            currentTime = 0;
        }

        public void FadeIn()
        {
            foreach (Graphic graphic in graphics)
            {
                graphic.DOFade(1, fadeTime);
            }
        }

        public void FadeOut()
        {
            isFadingOut = true;
            foreach (Graphic graphic in graphics)
            {
                graphic.DOFade(0, fadeTime);
            }
        }
    }
}