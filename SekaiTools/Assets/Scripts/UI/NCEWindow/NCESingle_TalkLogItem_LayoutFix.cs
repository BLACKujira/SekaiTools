using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCESingle_TalkLogItem_LayoutFix :  MonoBehaviour , ILayoutElement
    {
        public RectTransform serifWindow;
        public float offset;

        public float width => LayoutUtility.GetPreferredSize(serifWindow, 0);
        public float height => LayoutUtility.GetPreferredSize(serifWindow, 1) + offset;

        public float minWidth => width;
        public float preferredWidth => width;
        public float flexibleWidth => width;
        public float minHeight => height;
        public float preferredHeight => height;
        public float flexibleHeight => height;
        public int layoutPriority => 1;

        public void CalculateLayoutInputHorizontal()
        {
        }

        public void CalculateLayoutInputVertical()
        {
        }
    }
}