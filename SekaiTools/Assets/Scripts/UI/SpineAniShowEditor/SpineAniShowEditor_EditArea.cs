using SekaiTools.UI.SpineSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SpineAniShowEditor
{
    public class SpineAniShowEditor_EditArea : MonoBehaviour
    {
        public SpineAniShowEditor spineAniShowEditor;
        [Header("Components")]
        public SpineSettingAnimation spineSettingAnimation;

        public Spine.SpineControllerTypeA.ModelPair currentModel
        {
            get
            {
                if (spineAniShowEditor.spineImage.selectedID == -1) return null;
                return spineAniShowEditor.spineController.models[spineAniShowEditor.spineImage.selectedID];
            }
        }

        public Spine.SpineScene.SpineObject currentModelData
        {
            get
            {
                if (spineAniShowEditor.spineImage.selectedID == -1) return null;
                return spineAniShowEditor.currentScene.spineObjects[spineAniShowEditor.spineImage.selectedID];
            }
        }

        private void Awake()
        {
            spineSettingAnimation.Initialize(spineAniShowEditor.PlayFromBeginning);

        }

        private void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (spineAniShowEditor.spineImage.selectedID == -1)
            {
                spineSettingAnimation.NoData();
            }
            else
            {
                spineSettingAnimation.SetData(currentModelData);
            }
        }
    }
}