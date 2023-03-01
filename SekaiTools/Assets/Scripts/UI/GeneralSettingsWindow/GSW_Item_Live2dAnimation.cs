using SekaiTools.Live2D;
using System;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_Live2dAnimation : GSW_Item
    {
        public L2DAnimationSelectButton_Open buttonFacial;
        public L2DAnimationSelectButton_Open buttonMotion;

        Func<L2DAnimationSet> getAnimationSet;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_Live2dAnimation configUIItem_Live2dAnimation = configUIItem as ConfigUIItem_Live2dAnimation;
            if (configUIItem_Live2dAnimation == null) throw new ItemTypeMismatchException();

            getAnimationSet = configUIItem_Live2dAnimation.getAnimationSet;

            Live2D.L2DAnimationSet l2DAnimationSet = configUIItem_Live2dAnimation.getAnimationSet();
            buttonFacial.Initialize(
                (value) => { configUIItem_Live2dAnimation.setFacial(value); generalSettingsWindow.Refresh(); }
                );
            buttonMotion.Initialize(
                (value) => { configUIItem_Live2dAnimation.setMotion(value); generalSettingsWindow.Refresh(); }
                );

            if (l2DAnimationSet != null)
            {
                buttonFacial.L2DAnimationSelectButton.SetAnimation(l2DAnimationSet, configUIItem_Live2dAnimation.getFacial());
                buttonMotion.L2DAnimationSelectButton.SetAnimation(l2DAnimationSet, configUIItem_Live2dAnimation.getMotion());
            }
            bool interactable = getAnimationSet() != null;
            buttonFacial.L2DAnimationSelectButton.button.interactable = interactable;
            buttonMotion.L2DAnimationSelectButton.button.interactable = interactable;
        }
    }
}