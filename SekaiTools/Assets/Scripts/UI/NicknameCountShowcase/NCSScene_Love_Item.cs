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
        public Image imageBGTalker;
        public Image imageBGChar;
        public Text textPercent;
        public Text textDivide;
        [Header("Settings")]
        public IconSet iconSet;

        public void SetData(int talkerId,int nameId,int countTotal,int countMax)
        {
            imageTalker.sprite = iconSet.icons[talkerId];
            imageChar.sprite = iconSet.icons[nameId];

            imageBGTalker.color = ConstData.characters[talkerId].imageColor;
            imageBGChar.color = ConstData.characters[nameId].imageColor;

            textPercent.text = (((float)countMax / countTotal)*100).ToString("00.00")+"%";
            textDivide.text = $"{countMax} / {countTotal}";
        }
    }
}