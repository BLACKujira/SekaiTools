using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Live2dAnimation : GSW_Item
    {
        public L2DAnimationSelectButton_Open buttonFacial;
        public L2DAnimationSelectButton_Open buttonMotion;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Live2dAnimation configUIItem_Live2dAnimation = configUIItem as ConfigUIItem_Live2dAnimation;
            if (configUIItem_Live2dAnimation == null) throw new ItemTypeMismatchException();
            Live2D.L2DAnimationSet l2DAnimationSet = configUIItem_Live2dAnimation.getAnimationSet();
            buttonFacial.Initialize(
                (value)=> { configUIItem_Live2dAnimation.setFacial(value);generalSettingsWindow.Refresh(); }, 
                WindowController.windowController.currentWindow);
            buttonMotion.Initialize(
                (value) => { configUIItem_Live2dAnimation.setMotion(value); generalSettingsWindow.Refresh(); },
                WindowController.windowController.currentWindow);

            if (l2DAnimationSet != null)
            {
                buttonFacial.L2DAnimationSelectButton.SetAnimation(l2DAnimationSet, configUIItem_Live2dAnimation.getFacial());
                buttonMotion.L2DAnimationSelectButton.SetAnimation(l2DAnimationSet, configUIItem_Live2dAnimation.getMotion());
            }
            else
            {
                buttonFacial.L2DAnimationSelectButton.button.interactable = false;
                buttonMotion.L2DAnimationSelectButton.button.interactable = false;
            }
        }
    }
}