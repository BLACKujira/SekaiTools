using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.BackGroundSettings
{
    public class BackGroundSettings_PartItem : ButtonWithIconAndText
    {
        public Image targetImage;
        [Header("Color")]
        public Color normalColor;
        public Color bGPartColor;
        [Header("Button")]
        public Button buttonRemove;
        public Button buttonConfig;
        public Button buttonMoveLeft;
        public Button buttonMoveRight;

        public enum Style { Normal,BGPart }
        
        public void ChangeStyle(Style style)
        {
            switch (style)
            {
                case Style.Normal:
                    targetImage.color = normalColor;
                    break;
                case Style.BGPart:
                    targetImage.color = bGPartColor;
                    break;
                default:
                    break;
            }
        }
        public void SetHideMode(bool ifHideMode)
        {
            Color color = targetImage.color;
            color.a = ifHideMode?.5f:1;
            targetImage.color = color;
        }
    }
}