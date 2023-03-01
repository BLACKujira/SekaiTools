using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Love_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imageTalker;
        public Image imageChar;
        public Image[] imageBGTalker;
        public Image[] imageBGChar;
        public Text textPercent;
        public Text textDivide;
        public Image percentBarFillMask;
        [Header("Settings")]
        public IconSet iconSet;

        public void SetData(int talkerId,int nameId,int countTotal,int countMax)
        {
            imageTalker.sprite = iconSet.icons[talkerId];
            imageChar.sprite = iconSet.icons[nameId];

            foreach (var image in imageBGTalker)
            {
                image.color = ConstData.characters[talkerId].imageColor;
            }
            foreach (var image in imageBGChar)
            {
                image.color = ConstData.characters[nameId].imageColor;
            }
            float percent = (float)countMax / countTotal;
            textPercent.text = (percent * 100).ToString("00.00")+"%";
            textDivide.text = $"{countMax} / {countTotal}";
            if(percentBarFillMask != null) percentBarFillMask.fillAmount = percent;
        }
    }
}