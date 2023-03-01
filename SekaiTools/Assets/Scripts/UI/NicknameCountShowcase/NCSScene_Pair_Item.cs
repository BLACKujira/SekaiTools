using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Pair_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imageLeft;
        public Image imageRight;
        public Image[] imageBGLeft;
        public Image[] imageBGRight;
        public Text textTotal;
        public Text textLeftCount;
        public Text textRightCount;
        [Header("Settings")]
        public IconSet iconSet;

        public void SetData(int leftId, int rightId, int countLeft, int countRight)
        {
            imageLeft.sprite = iconSet.icons[leftId];
            imageRight.sprite = iconSet.icons[rightId];

            foreach (Image image in imageBGLeft) 
            {
                image.color = ConstData.characters[leftId].imageColor;
            }
            foreach (Image image in imageBGRight)
            {
                image.color = ConstData.characters[rightId].imageColor;
            }

            textLeftCount.text = countLeft.ToString("000");
            textRightCount.text = countRight.ToString("000");
            textTotal.text = (countLeft+countRight).ToString("000");
        }
    }
}