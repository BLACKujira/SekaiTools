using SekaiTools.UI.DynamicBarChart;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_BarChartCharacter_Item : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform barRectTransform;
        public Text txtNumber;
        public Text txtPercent;
        public BondsHonorSub bondsHonorSub;
        public Image[] imgBarColor;
        public Image imgCharIcon;
        public HDRUIController particleController;
        [Header("Settings")]
        public float barLength = 500;
        public float minLength = 70f;
        public float deltaPerSec = 0.1f;
        public IconSet charSmallIconSet;
        public HDRColorSet hDRColorSet;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        float currentNumber = 0;
        float currentPercent = 0;

        float targetNumber = 0;
        float targetLength = 0;
        float targetPercent = 0;

        float maxNumber;
        float maxPercent;

        private void Awake()
        {
            barRectTransform.sizeDelta = new Vector2(barRectTransform.sizeDelta.x, minLength);
        }

        bool ifFirstSetData = true;
        public void SetData(int charAID, int charBID, float number, float maxNumber, float total)
        {
            if (ifFirstSetData)
            {
                bondsHonorSub.SetCharacter(charAID, charBID, true);
                foreach (var image in imgBarColor)
                {
                    image.color = ConstData.characters[charBID].imageColor;
                }
                imgCharIcon.sprite = charSmallIconSet.icons[charBID];
                ifFirstSetData = false;

                if (!particleController.Initialized) particleController.Initialize();
                DynamicBarChart_Item_Head dynamicBarChart_Item_Head = particleController.InstantiateObject.GetComponent<DynamicBarChart_Item_Head>();
                dynamicBarChart_Item_Head.HDRColor = hDRColorSet.colors[charBID];
            }

            this.maxNumber = maxNumber;
            this.maxPercent = maxNumber / total;

            targetNumber = number;
            targetLength = (number / maxNumber) * (barLength - minLength) + minLength;
            targetPercent = number / total;
        }

        private void Update()
        {
            float barLength = barRectTransform.sizeDelta.y;
            barLength = Mathf.MoveTowards(barLength, targetLength, deltaPerSec * (this.barLength - minLength) * Time.deltaTime);
            barRectTransform.sizeDelta = new Vector2(barRectTransform.sizeDelta.x, barLength);

            float number = currentNumber;
            number = Mathf.MoveTowards(currentNumber, targetNumber, deltaPerSec * maxNumber * Time.deltaTime);
            currentNumber = number;
            txtNumber.text = number.ToString("0");

            float percent = currentPercent;
            percent = Mathf.MoveTowards(percent, targetPercent, deltaPerSec * maxPercent * Time.deltaTime);
            currentPercent = percent;
            txtPercent.text = (percent * 100).ToString("00.00") + "%";
        }
    }
}