using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    [RequireComponent(typeof(L2DAnimationSelectButton))]
    public class L2DAnimationSelectButton_Open : MonoBehaviour
    {
        private L2DAnimationSelectButton l2DAnimationSelectButton;
        public Window openWindow;

        public L2DAnimationSelectButton L2DAnimationSelectButton { 
            get { if (l2DAnimationSelectButton == null) l2DAnimationSelectButton = GetComponent<L2DAnimationSelectButton>();return l2DAnimationSelectButton; }
            set => l2DAnimationSelectButton = value; }

        public void Initialize(Action<string> setString,Window inWindow)
        {
            L2DAnimationSelectButton.button.onClick.AddListener(() => 
            {
                Window window = Instantiate(openWindow);
                if (window.controlScript is Live2DMotionSelect.Live2DMotionSelect)
                {
                    Live2DMotionSelect.Live2DMotionSelect motionSelect = (Live2DMotionSelect.Live2DMotionSelect)window.controlScript;
                    motionSelect.Initialize(L2DAnimationSelectButton.animationSet, (str) => { setString(str); window.Close(); });
                    motionSelect.SetDefaultPage(null);
                }
                else
                {
                    Live2DFacialSelect.Live2DFacialSelect facialSelect = (Live2DFacialSelect.Live2DFacialSelect)window.controlScript;
                    facialSelect.Initialize(L2DAnimationSelectButton.animationSet, (str) => { setString(str); window.Close(); });
                }
                //motionSelect.SetDefaultPage(button.animationName);//这个功能暂且有bug
                window.Initialize(inWindow);
                window.Show();
            });   
        }
    }
}