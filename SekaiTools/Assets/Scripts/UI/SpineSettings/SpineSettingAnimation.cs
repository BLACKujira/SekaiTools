using SekaiTools.Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineSettings
{
    public class SpineSettingAnimation : SpineSettingBase
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

        SpineScene.SpineObject spineObject;
        Action playFromBeginning;

        private void Awake()
        {
            selectAnimationButton.onClick.AddListener(() =>
            {
                var spineAnimationSelect = WindowController.windowController.currentWindow.OpenWindow<SpineAnimationSelect.SpineAnimationSelect>(animationSelectWindowPrefab);
                spineAnimationSelect.Initialize((string str) =>
                {
                    spineObject.animation = str;
                    playFromBeginning();
                    SetData(spineObject);
                });
            });

            animationProgressSlider.onValueChanged.AddListener((float value)=> { spineObject.animationProgress = value; playFromBeginning(); });
        }
        public void Initialize(Action playFromBeginning)
        {
            this.playFromBeginning = playFromBeginning;
        }

        public void SetData(SpineScene.SpineObject spineObject)
        {
            this.spineObject = spineObject;
            selectAnimationButton.image.sprite = animationPreview.GetValue(spineObject.animation);
            animationName.text = spineObject.animation;
            animationProgressSlider.value = spineObject.animationProgress;
            selectAnimationButton.interactable = true;
            animationProgressSlider.interactable = true;
        }

        public override void NoData()
        {
            selectAnimationButton.interactable = false;
            selectAnimationButton.image.sprite = noDataIcon;
            animationName.text = "请选择模型";
            animationProgressSlider.interactable = false;
        }
    }
}