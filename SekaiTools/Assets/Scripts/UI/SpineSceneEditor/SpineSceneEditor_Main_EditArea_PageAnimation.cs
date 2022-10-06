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
        public InputField animationProgressInput;
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
                    spineSceneEditor.PlayFromBeginning();
                });
            });

            animationProgressInput.onEndEdit.AddListener((str) => 
            {
                try
                {
                    modelPair.animationProgress = float.Parse(str);
                }
                catch
                {
                    spineSceneEditor.msgLayer_Err.ShowMessage("非法输入，将重置动画偏移为0");
                    animationProgressInput.text = "0";
                    modelPair.animationProgress = 0;
                }
                if(modelPair.animationProgress<0 || modelPair.animationProgress>1)
                {
                    spineSceneEditor.msgLayer_Err.ShowMessage("动画偏移应在[0,1]之间，将重置偏移为0");
                    animationProgressInput.text = "0";
                    modelPair.animationProgress = 0;
                }
                spineSceneEditor.PlayFromBeginning();
            });
        }

        public override void Refresh()
        {
            selectAnimationButton.image.sprite = animationPreview.GetValue(modelPair.Model.AnimationName);
            animationName.text = modelPair.Model.AnimationName;
            animationProgressInput.text = modelPair.animationProgress.ToString();
            selectAnimationButton.interactable = true;
            animationProgressInput.interactable = true;
        }

        protected override void Interactive()
        {
            selectAnimationButton.interactable = true;
            animationProgressInput.interactable = true;
        }

        protected override void NonInteractive()
        {
            selectAnimationButton.interactable = false;
            selectAnimationButton.image.sprite = noDataIcon;
            animationName.text = "请选择模型";
            animationProgressInput.text = string.Empty;
            animationProgressInput.interactable = false;
        }
    }
}