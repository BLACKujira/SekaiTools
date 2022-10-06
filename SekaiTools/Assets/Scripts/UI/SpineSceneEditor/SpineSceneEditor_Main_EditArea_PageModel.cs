using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main_EditArea_PageModel : SpineSceneEditor_Main_EditArea_Page
    {
        public Button buttonChangeModel;
        public Text textModelName;
        public Button buttonRemoveModel;
        [Header("Settings")]
        public InbuiltImageData modelPreviewImage;
        public Sprite modelUnselect;


        private void Awake()
        {
            buttonChangeModel.onClick.AddListener(() =>
            {
                SpineModelSelect.SpineModelSelect spineModelSelect = editArea.spineSceneEditor.window.OpenWindow<SpineModelSelect.SpineModelSelect>(editArea.spineSceneEditor.spineModelSelectPrefab);
                spineModelSelect.Initialize(editArea.spineSceneEditor.spineModelSet,
                    (atlasAssetPair) =>
                    {
                        int index = spineController.models.IndexOf(modelPair);
                        spineController.ReplaceModel(atlasAssetPair, index);
                        Refresh();
                    });
            });
            buttonRemoveModel.onClick.AddListener(() =>
            { RemoveModel(); });
        }

        public override void Refresh()
        {
            buttonChangeModel.image.sprite = modelPreviewImage.GetValue(modelPair.Name);
            textModelName.text = modelPair.Name;
        }

        protected override void Interactive()
        {
            buttonChangeModel.interactable = true;
            buttonRemoveModel.interactable = true;
        }

        protected override void NonInteractive()
        {
            buttonChangeModel.image.sprite = modelUnselect;
            textModelName.text = "请选择模型";
            buttonChangeModel.interactable = false;
            buttonRemoveModel.interactable = false;
        }

        public void RemoveModel()
        {
            WindowController.ShowCancelOK("移除模型", $"是否要移除模型{modelPair.Name}", () =>
              {
                  spineController.RemoveModel(modelPair);
                  spineSceneEditor.spineImage.ResetAll();
              });
        }
    }
}