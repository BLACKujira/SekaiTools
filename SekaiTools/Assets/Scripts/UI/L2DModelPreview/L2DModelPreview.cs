using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelPreview
{
    public class L2DModelPreview : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtModelName;
        public L2DModelPreview_AnimationArea animationArea;
        public L2DModelPreview_TransformArea transformArea;
        public L2DControllerTypeC l2DController;
        [Header("Settings")]
        public InbuiltAnimationSet animationSet;
        [Header("Prefab")]
        public Window l2DModelSelectorPrefab;

        private void Awake()
        {
            animationArea.Initialize();
            transformArea.Initialize();
            Refresh();
        }

        public void SelectModel()
        {
            L2DModelSelect.L2DModelSelect l2DModelSelect = window.OpenWindow<L2DModelSelect.L2DModelSelect>(l2DModelSelectorPrefab);
            l2DModelSelect.Initialize((smi) =>
            {
                L2DModelLoaderObjectBase l2DModelLoaderObjectBase = L2DModelLoader.LoadModel(smi.modelName);
                WindowController.ShowNowLoadingCenter("正在加载模型", l2DModelLoaderObjectBase).OnFinish+=
                ()=>
                {
                    SekaiLive2DModel model = l2DModelLoaderObjectBase.Model;
                    model.AnimationSet = animationSet.GetAnimationSet(smi.animationSet);
                    l2DController.ShowModel(model);
                    Refresh();
                    transformArea.ResetTransform();
                    animationArea.ResetAll();
                };
            });
        }

        public void Refresh()
        {
            txtModelName.text = l2DController.model == null ? "请选择模型" : l2DController.model.name; 
        }
    }
}