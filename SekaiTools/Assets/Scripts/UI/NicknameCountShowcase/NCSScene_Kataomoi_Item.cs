using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Kataomoi_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imageLeft;
        public Image imageRight;
        public Image imageBGLeft;
        public Image imageBGRight;
        public Text textTotal;
        public Text textLeftCount;
        public Text textRightCount;
        [Header("Settings")]
        public IconSet iconSet;

        public void SetData(int leftId, int rightId, float percentLeft, float percentRight)
        {
            imageLeft.sprite = iconSet.icons[leftId];
            imageRight.sprite = iconSet.icons[rightId];

            imageBGLeft.color = ConstData.characters[leftId].imageColor;
            imageBGRight.color = ConstData.characters[rightId].imageColor;

            percentLeft *= 100;
            percentRight *= 100;

            textLeftCount.text = percentLeft.ToString("00.00") + "%";
            textRightCount.text = percentRight.ToString("00.00") + "%";
            textTotal.text = (percentLeft-percentRight).ToString("00.00") + "%";
        }
    }
}