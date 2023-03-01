using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Increase_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imageTalker;
        public Image imageChar;
        public Image[] imageBGTalker;
        public Image[] imageBGChar;
        public Text textIncrease;
        public Text textSub;
        public Image percentBarFillMask;
        [Header("Settings")]
        public IconSet iconSet;

        public void SetData(int talkerId, int nameId, int countNow, int countLastTime)
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

            textIncrease.text = $"+ {countNow - countLastTime}";
            if (textSub) textSub.text = $"{countLastTime} => {countNow}";
            if (percentBarFillMask) percentBarFillMask.fillAmount = (float)(countNow-countLastTime) / countNow;
        }
    }
}