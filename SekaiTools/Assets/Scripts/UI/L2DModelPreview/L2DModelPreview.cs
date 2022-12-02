using SekaiTools.Live2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
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
        public GameObject gobjEditArea;
        [Header("Prefab")]
        public Window l2DModelSelectorPrefab;

        InbuiltAnimationSet animationSet => L2DModelLoader.InbuiltAnimationSet;
        public event Action<SekaiLive2DModel> OnModelSet;

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
                    if (l2DController.model != null) Destroy(l2DController.model.gameObject);
                    l2DController.ShowModel(model);
                    Refresh();
                    transformArea.ResetTransform();
                    animationArea.ResetAll();
                    if(OnModelSet!=null) OnModelSet(model);
                };
            });
        }

        public void Refresh()
        {
            txtModelName.text = l2DController.model == null ? "请选择模型" : l2DController.model.name;
            gobjEditArea.SetActive(l2DController.model == null ? false : true);
        }

        public void Capture()
        {
            RenderTexture lastTex = RenderTexture.active;
            RenderTexture renderTex = l2DController.L2DCamera.targetTexture;
            RenderTexture.active = renderTex;
            Texture2D texture2D = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBA32, false);
            texture2D.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            RenderTexture.active = lastTex;

            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_PNG);
            if (l2DController.model)
                saveFileDialog.FileName = l2DController.model.name;
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            byte[] png = texture2D.EncodeToPNG();
            File.WriteAllBytes(saveFileDialog.FileName, png);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                animationArea.PlayAll();
        }

        private void OnDestroy()
        {
            if (l2DController.model != null)
                Destroy(l2DController.model);
        }
    }
}