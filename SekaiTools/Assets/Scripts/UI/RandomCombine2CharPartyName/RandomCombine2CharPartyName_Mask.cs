using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RandomCombine2CharPartyName
{
    public class RandomCombine2CharPartyName_Mask : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharL;
        public Image imgCharR;
        public Image colorL;
        public Image colorR;
        [Header("Settings")]
        public float fadeTime = 0.5f;
        public IconSet iconSetChar;

        int charLId;
        int charRId;

        public void SetData(int charLId, int charRId)
        {
            this.charLId = charLId;
            this.charRId = charRId;
            imgCharL.sprite = iconSetChar.icons[charLId];
            imgCharR.sprite = iconSetChar.icons[charRId];
        }

        public void Play()
        {
            Color imageColorL = ConstData.characters[charLId].imageColor;
            Color imageColorR = ConstData.characters[charRId].imageColor;

            imageColorL.a = 0;
            imageColorR.a = 0;

            imgCharL.color = Color.white;
            imgCharR.color = Color.white;
            colorL.color = Color.white;
            colorL.color = Color.white;

            imgCharL.DOColor(imageColorL, fadeTime);
            imgCharR.DOColor(imageColorR, fadeTime);
            colorL.DOColor(imageColorL, fadeTime);
            colorR.DOColor(imageColorR, fadeTime);
        }

        public void TurnOff()
        {
            imgCharL.color = Color.clear;
            imgCharR.color = Color.clear;
            colorL.color = Color.clear;
            colorR.color = Color.clear;
        }

        public void ResetColor()
        {
            imgCharL.color = Color.white;
            imgCharR.color = Color.white;
            colorL.color = Color.white;
            colorR.color = Color.white;
        }
    }
}