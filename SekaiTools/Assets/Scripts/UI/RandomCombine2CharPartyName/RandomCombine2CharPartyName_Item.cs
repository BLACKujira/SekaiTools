using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RandomCombine2CharPartyName
{

    public class RandomCombine2CharPartyName_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharL;
        public Image imgCharR;
        public Image colorL;
        public Image colorR;
        public Text groupName;
        [Header("Settings")]
        public IconSet iconSetChar;

        public void SetData(int charLId,int charRId,string groupName)
        {
            imgCharL.sprite = iconSetChar.icons[charLId];
            imgCharR.sprite = iconSetChar.icons[charRId];
            colorL.color = ConstData.characters[charLId].imageColor;
            colorR.color = ConstData.characters[charRId].imageColor;
            this.groupName.text = groupName;
        }
    }
}