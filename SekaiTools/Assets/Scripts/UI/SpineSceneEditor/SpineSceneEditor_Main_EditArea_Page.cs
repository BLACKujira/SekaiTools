using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using static SekaiTools.Spine.SpineControllerTypeA;

namespace SekaiTools.UI.SpineSceneEditor
{
    public abstract class SpineSceneEditor_Main_EditArea_Page : MonoBehaviour
    {
        protected SpineSceneEditor_Main_EditArea editArea;
        protected ModelPair modelPair;
        protected SpineControllerTypeA spineController => editArea.spineSceneEditor.spineController;
        protected SpineSceneEditor_Main spineSceneEditor => editArea.spineSceneEditor;

        public void Initialize(SpineSceneEditor_Main_EditArea editArea)
        {
            this.editArea = editArea;   
        }

        public void SetData(ModelPair modelPair)
        {
            this.modelPair = modelPair;
            Refresh();
        }

        public abstract void Refresh();

        bool _interactable = true;
        public bool interactable 
        {
            get => _interactable;
            set
            {
                if (_interactable == value) return;
                if (_interactable) NonInteractive();
                else Interactive();
                _interactable = value;
            }
        }
        protected abstract void Interactive();
        protected abstract void NonInteractive();
    }
}