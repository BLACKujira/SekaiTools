using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class BondsHonorBase : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image _IconLeft;
        [SerializeField] Image _IconRight;
        [SerializeField] Image _ColorLeft, _ColorRight;

        [Header("Settings")]
        public IconSet iconSet;

        public Sprite IconLeft { set => _IconLeft.sprite = value; }
        public Sprite IconRight { set => _IconRight.sprite = value; }
        public Color ColorLeft { set => _ColorLeft.color = value; }
        public Color ColorRight { set => _ColorRight.color = value; }
        
        public void SetCharacter(int idLeft, int idRight)
        {
            ColorLeft = ConstData.characters[idLeft].imageColor;
            ColorRight = ConstData.characters[idRight].imageColor;
            IconLeft = iconSet.icons[idLeft];
            IconRight = iconSet.icons[idRight];
        }
    }
}