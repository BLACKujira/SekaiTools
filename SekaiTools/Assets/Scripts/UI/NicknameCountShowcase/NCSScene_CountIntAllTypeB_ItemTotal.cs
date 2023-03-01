using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountIntAllTypeB_ItemTotal : MonoBehaviour
    {
        [Header("Components")]
        public Graphic[] graphicsCharLColor;
        public Image imgCharLIcon;
        public Text txtTotal;
        public Text txtPercent;
        public RectTransform rtPercentBar;
        public RectTransform rtPercentBarFill;
        [Header("Settings")]
        public IconSet charIconSet;

        public void Initialize(int talkerId, string totalText, int total, int totalSerif)
        {
            foreach (var graphic in graphicsCharLColor)
            {
                graphic.color = ConstData.characters[talkerId].imageColor;
            }

            imgCharLIcon.sprite = charIconSet.icons[talkerId];

            txtTotal.text = totalText;
            float percent = (float)total / totalSerif;
            txtPercent.text = (percent * 100).ToString("00.00") + "%";

            rtPercentBarFill.sizeDelta = new Vector2(percent * rtPercentBar.sizeDelta.x, rtPercentBarFill.sizeDelta.y);
        }
    }
}