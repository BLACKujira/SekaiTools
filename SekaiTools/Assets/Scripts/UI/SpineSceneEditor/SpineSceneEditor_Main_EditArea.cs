using SekaiTools.Spine;
using SekaiTools.UI.SpineLayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main_EditArea : MonoBehaviour
    {
        public SpineSceneEditor_Main spineSceneEditor;
        [Header("Components")]
        public SpineSceneEditor_Main_EditArea_Page[] pages;

        public Spine.SpineControllerTypeA.ModelPair currentModel
        {
            get
            {
                if (spineSceneEditor.spineImage.selectedID == -1) return null;
                return spineSceneEditor.spineController.models[spineSceneEditor.spineImage.selectedID];
            }
        }

        public Spine.SpineScene.SpineObject currentModelData
        {
            get
            {
                if (spineSceneEditor.spineImage.selectedID == -1) return null;
                return spineSceneEditor.spineScene.spineObjects[spineSceneEditor.spineImage.selectedID];
            }
        }

        private void Awake()
        {
            foreach (var page in pages)
            {
                page.Initialize(this);
            }
        }

        private void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (spineSceneEditor.spineImage.selectedID == -1)
            {
                foreach (var page in pages)
                {
                    page.interactable = false;
                }
            }
            else
            {
                foreach (var page in pages)
                {
                    if (!page.interactable) page.interactable = true;
                    page.SetData(currentModel);
                }
            }
        }
    }
    }