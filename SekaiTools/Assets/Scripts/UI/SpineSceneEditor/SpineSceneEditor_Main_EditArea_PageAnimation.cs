using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main_EditArea_PageAnimation : SpineSceneEditor_Main_EditArea_Page
    {
        [Header("Component")]
        public Button selectAnimationButton;
        public Slider animationProgressSlider;
        public Text animationName;
        [Header("Settings")]
        public InbuiltImageData animationPreview;
        public Sprite noDataIcon;
        [Header("Prefab")]
        public Window animationSelectWindowPrefab;

        private void Awake()
        {
            selectAnimationButton.onClick.AddListener(() =>
            {
                var spineAnimationSelect = WindowController.windowController.currentWindow.OpenWindow<SpineAnimationSelect.SpineAnimationSelect>(animationSelectWindowPrefab);
                spineAnimationSelect.Initialize((string str) =>
                {
                    modelPair.Model.AnimationState.SetAnimation(0,str,true);
                    spineSceneEditor.SaveChanges();
                    spineSceneEditor.PlayFromBeginning();
                });
            });

            animationProgressSlider.onValueChanged.AddListener((float value) => 
            { 
                modelPair.animationProgress = value;
                spineSceneEditor.SaveChanges();
                spineSceneEditor.PlayFromBeginning();
            });
        }

        public override void Refresh()
        {
            selectAnimationButton.image.sprite = animationPreview.GetValue(modelPair.Model.AnimationName);
            animationName.text = modelPair.Model.AnimationName;
            animationProgressSlider.value = modelPair.animationProgress;
            selectAnimationButton.interactable = true;
            animationProgressSlider.interactable = true;
        }

        protected override void Interactive()
        {
            selectAnimationButton.interactable = true;
            animationProgressSlider.interactable = true;
        }

        protected override void NonInteractive()
        {
            selectAnimationButton.interactable = false;
            selectAnimationButton.image.sprite = noDataIcon;
            animationName.text = "请选择模型";
            animationProgressSlider.value = 0;
            animationProgressSlider.interactable = false;
        }
    }
}