using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main_EditArea_PageModel : SpineSceneEditor_Main_EditArea_Page
    {
        public Text textModelName;
        public Button buttonRemoveModel;

        private void Awake()
        {
            buttonRemoveModel.onClick.AddListener(() =>
            { RemoveModel(); });
        }

        public override void Refresh()
        {
            textModelName.text = modelPair.Name;
        }

        protected override void Interactive()
        {
            buttonRemoveModel.interactable = true;
        }

        protected override void NonInteractive()
        {
            textModelName.text = "请选择模型";
            buttonRemoveModel.interactable = false;
        }

        public void RemoveModel()
        {
            spineController.RemoveModel(modelPair);
            spineSceneEditor.SaveChanges();
            spineSceneEditor.Refresh();
        }
    }
}