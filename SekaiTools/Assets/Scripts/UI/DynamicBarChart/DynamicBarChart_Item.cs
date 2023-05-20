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
        public HDRUIController headController;
        [Header("Settings")]
        public float barLength = 1475f;
        public float fadeTime = 0.5f;
        public float particleMinEmission = 2;
        public float particleMaxEmission = 20;
        public float particleEmissionRate = 1;

        float frameHoldTime = 0.25f;

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
        DynamicBarChart_Item_Head dynamicBarChart_Item_Head;
        public DynamicBarChart_Item_Head DynamicBarChart_Item_Head
        {
            get
            {
                if (headController == null || headController.InstantiateObject == null) return null;
                if (dynamicBarChart_Item_Head == null)
                    dynamicBarChart_Item_Head = headController.InstantiateObject.GetComponent<DynamicBarChart_Item_Head>();
                return dynamicBarChart_Item_Head;
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

        public virtual void UpdateData(DataFrame dataFrame, string key, float maxNumber,float frameHoldTime)
        {
            this.frameHoldTime = frameHoldTime;

            lastTargetLength = targetLength;
            lastTargetNumber = targetNumber;
            targetLength = barLength * (dataFrame.data[key] / maxNumber);
            targetNumber = dataFrame.data[key];
            currentTime = 0;

            float increase = targetNumber - lastTargetNumber;
            if (DynamicBarChart_Item_Head != null)
            {
                DynamicBarChart_Item_Head.EmissionRate = Mathf.Min(particleMaxEmission,
                    particleMinEmission + particleEmissionRate * increase);
            }
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