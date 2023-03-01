using System;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountIntAllTypeB_Item : NCSScene_CountIntAll_ItemBase
    {
        [Header("Components")]
        public Graphic[] graphicsCharLColor;
        public Graphic[] graphicsCharRColor;
        public Image imgCharLIcon;
        public Image imgCharRIcon;
        public Text txtCount;
        public Text txtPercent;
        public RectTransform rtPercentBar;
        public RectTransform rtPercentBarFill;
        [Header("Settings")]
        public IconSet charIconSet;

        public override void Initialize(int talkerId, int nameId, int times, int total)
        {
            foreach (var graphic in graphicsCharLColor)
            {
                graphic.color = ConstData.characters[talkerId].imageColor;
            }
            foreach (var graphic in graphicsCharRColor)
            {
                graphic.color = ConstData.characters[nameId].imageColor;
            }

            if ((talkerId >= 1 && talkerId <= 20) && (nameId >= 21 && nameId <= 26))
            {
                Unit unit = ConstData.characters[talkerId].unit;
                nameId = ConstData.GetUnitVirtualSinger(nameId, unit);
            }

            imgCharLIcon.sprite = charIconSet.icons[talkerId];
            imgCharRIcon.sprite = charIconSet.icons[nameId];

            txtCount.text = times.ToString();
            float percent = (float)times / total;
            txtPercent.text = (percent * 100).ToString("00.00") + "%";

            rtPercentBarFill.sizeDelta = new Vector2(percent * rtPercentBar.sizeDelta.x, rtPercentBarFill.sizeDelta.y);
        }
    }
}