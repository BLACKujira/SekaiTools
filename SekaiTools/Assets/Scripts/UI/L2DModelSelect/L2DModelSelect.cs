using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelSelect
{
    public class L2DModelSelect : L2DModelManagement.L2DModelManagement
    {
        [Header("Components")]
        public Text txtAnimationSet;
        [Header("Settings")]
        public InbuiltAnimationSet animationSets;

        Action<SelectedModelInfo> onSelect;
        string selectedAnimationSetName;

        public void Initialize(Action<SelectedModelInfo> onSelect)
        {
            onChangeSelection += (modelInfo) =>
            {
                L2DAnimationSet l2DAnimationSet = animationSets.GetAnimationSetByModelName(modelInfo.modelName);
                selectedAnimationSetName = l2DAnimationSet != null ? l2DAnimationSet.name : "无";
                txtAnimationSet.text = selectedAnimationSetName;
            };
            this.onSelect = onSelect;
        }

        public void SelectCurrentModel()
        {
            window.Close();
            onSelect(new SelectedModelInfo(CurrentModelInfo.modelName,selectedAnimationSetName));
        }
    }

    public struct SelectedModelInfo
    {
        public string modelName;
        public string animationSet;

        public SelectedModelInfo(string modelName, string animationSet)
        {
            this.modelName = modelName;
            this.animationSet = animationSet;
        }
    }

}