using SekaiTools.Live2D;
using SekaiTools.UI.Live2DMotionSelect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Live2DFacialSelect
{
    public class Live2DFacialSelect : MonoBehaviour
    {
        [Header("Components")]
        public Live2DMotionSelect_SelectArea selectArea;

        public void Initialize(L2DAnimationSet animationSet, Action<string> onButtonClick)
        {
            selectArea.Initialize(onButtonClick);
            selectArea.SetButtons(animationSet, animationSet.facialPack);
        }
    }
}