using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class BondsHonorSub : MonoBehaviour
    {
        public IconSet iconSet;
        public Image _IconLeft, _IconRight;
        public Image _ColorLeft, _ColorRight;
        public GameObject[] levelObjects = new GameObject[5];

        public Sprite IconLeft { set => _IconLeft.sprite = value; }
        public Sprite IconRight { set => _IconRight.sprite = value; }
        public Color ColorLeft { set => _ColorLeft.color = value; }
        public Color ColorRight { set => _ColorRight.color = value; }
    
        public void SetLevel(int level)
        {
            level = Mathf.Clamp(level, 0, 5);
            int i = 0;
            for (; i < level; i++)
            {
                levelObjects[i].SetActive(true); 
            }
            for (; i < 5; i++)
            {
                levelObjects[i].SetActive(false);
            }
        }
        public void SetCharacter(int idLeft,int idRight)
        {
            ColorLeft = ConstData.characters[idLeft].imageColor;
            ColorRight = ConstData.characters[idRight].imageColor;
            IconLeft = iconSet.icons[idLeft];
            IconRight = iconSet.icons[idRight];
        }
    }
}