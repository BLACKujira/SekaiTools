using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CutinSceneEditor
{
    public class CutinSelectButton : MonoBehaviour
    {
        public IconSet iconSet;
        public Image _IconLeft, _IconRight;
        public Image _ColorLeft, _ColorRight;
        public Text _Level;
        public Image _backGroundColor;
        public Color selectColor;


        public Sprite IconLeft { set => _IconLeft.sprite = value; }
        public Sprite IconRight { set => _IconRight.sprite = value; }
        public Color ColorLeft { set => _ColorLeft.color = value; }
        public Color ColorRight { set => _ColorRight.color = value; }
        public int Level { set => _Level.text = value.ToString(); }
        public Color BackGroundColor { set => _backGroundColor.color = value; }

        Color normalColor;

        private void Awake()
        {
            normalColor = _backGroundColor.color;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
        public void SetCharacter(int idLeft, int idRight)
        {
            ColorLeft = ConstData.characters[idLeft].imageColor;
            ColorRight = ConstData.characters[idRight].imageColor;
            IconLeft = iconSet.icons[idLeft];
            IconRight = iconSet.icons[idRight];
        }
        public void Select()
        {
            BackGroundColor = selectColor;
        }
        public void Unselect()
        {
            BackGroundColor = normalColor;
        }
    }
}