using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using UnityEngine.UI;

namespace SekaiTools.UI.Live2DMotionSelect
{
    public class Live2DMotionSelect : MonoBehaviour
    {
        [Header("Components")]
        public Live2DMotionSelect_SelectArea selectArea;
        public ToggleGenerator toggleGenerator;

        L2DAnimationSet animationSet;

        public void Initialize(L2DAnimationSet animationSet,Action<string> onButtonClick)
        {
            this.animationSet = animationSet;
            selectArea.Initialize(onButtonClick);
            toggleGenerator.Generate(animationSet.motionSets.Count+1,
                (Toggle toggle, int id) =>
                {
                    toggle.GetComponentInChildren<Text>().text = id==0?"all":animationSet.motionSets[id-1].setName;
                },
                (bool value,int id)=>
                {
                    if (value) selectArea.SetButtons(animationSet,id==0? animationSet.motionPack:animationSet.motionSets[id-1].motions);
                });
        }
        public void SetDefaultPage(string currentAnimation)
        {
            int id = 0;
            AnimationClip animationClip = animationSet.GetAnimation(currentAnimation);
            L2DAnimationSet.MotionSet motionSet = animationSet.FindMotionSet(animationClip);
            if (animationClip && motionSet != null)
            {
                id = animationSet.motionSets.IndexOf(motionSet)+1;
            }
            toggleGenerator.toggles[id].isOn = true;
        }
    }
}