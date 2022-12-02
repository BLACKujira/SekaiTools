using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using System;
using UnityEngine.UI;
using SekaiTools.UI.L2DAniSetManagement;
using SekaiTools.UI.L2DModelManagement;

namespace SekaiTools.UI.L2DModelSelect
{
    public class L2DModelSelect : L2DModelManagement.L2DModelManagement
    {
        [Header("Components")]
        public Text txtAnimationSet;
        public Button btnSelectAnimationSet;
        [Header("Settings")]
        public InbuiltAnimationSet animationSets;
        [Header("Settings")]
        public Window anisetSelectorPrefab;

        Action<SelectedModelInfo> onSelect;
        string selectedAnimationSetName;

        public void Initialize(Action<SelectedModelInfo> onSelect)
        {
            btnSelectAnimationSet.interactable = false;
            txtAnimationSet.text = string.Empty;
            onChangeSelection += (modelInfo) =>
            {
                L2DAnimationSet l2DAnimationSet = animationSets.GetAnimationSetByModelName(modelInfo.modelName);
                selectedAnimationSetName = l2DAnimationSet != null ? l2DAnimationSet.name : string.Empty;
                btnSelectAnimationSet.interactable = true;
                txtAnimationSet.text = string.IsNullOrEmpty(selectedAnimationSetName) ? "无" : l2DAnimationSet.name;
            };
            this.onSelect = onSelect;
        }

        public void SelectCurrentModel()
        {
            window.Close();
            if(CurrentModelInfo == null || string.IsNullOrEmpty(CurrentModelInfo.modelName))
                onSelect(new SelectedModelInfo(null,null));
            else
                onSelect(new SelectedModelInfo(CurrentModelInfo.modelName,selectedAnimationSetName));
        }

        public void ChangeAnimationSet()
        {
            UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(anisetSelectorPrefab);
            L2DAnimationSet[] l2DAnimationSetArray = animationSets.L2DAnimationSetArray;
            universalSelector.Generate(l2DAnimationSetArray.Length,
                (btn, id) =>
                {
                    L2DModelManagement_Item l2DModelManagement_Item = btn.GetComponent<L2DModelManagement_Item>();
                    l2DModelManagement_Item.Initialize(l2DAnimationSetArray[id].name);
                },
                (id) =>
                {
                    if (id < 0) selectedAnimationSetName = string.Empty;
                    else
                    {
                        selectedAnimationSetName = l2DAnimationSetArray[id].name;
                        txtAnimationSet.text = string.IsNullOrEmpty(selectedAnimationSetName) ? "无" : selectedAnimationSetName;
                    }
                });
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

        public static SelectedModelInfo Empty => new SelectedModelInfo(null, null);
    }

}