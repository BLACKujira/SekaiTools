using SekaiTools.Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniGIFGenerator
{
    public class SpineAniGIFGenerator_ModelArea : MonoBehaviour
    {
        public SpineAniGIFGenerator spineAniGIFGenerator;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public Text animationText;
        [Header("Settings")]
        public InbuiltSpineModelSet modelSet;
        [Header("Prefab")]
        public Button addModelButtonPrefab;
        public Window modelSelectWindowPrefab;
        public Window animationSelectWindowPrefab;

        private void Awake()
        {
            animationText.text = ConstData.defaultSpineAnimation;
        }

        public void GenerateButton()
        {
            Spine.SpineControllerTypeA spineController = spineAniGIFGenerator.spineController;
            universalGenerator.Generate(spineController.models.Count,
                (GameObject gameObject,int id) =>
                {
                    SpineAniGIFGenerator_ModelArea_Item item = gameObject.GetComponent<SpineAniGIFGenerator_ModelArea_Item>();
                    item.Initialize(spineController.models.Count-1-id, this);
                    if (id == spineController.models.Count - 1)
                        item.deleteButton.interactable = false;
                });
            universalGenerator.AddItem(addModelButtonPrefab.gameObject, (GameObject gameObject) =>
            {
                gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SpineModelSelect.SpineModelSelect spineModelSelect = spineAniGIFGenerator.window.OpenWindow<SpineModelSelect.SpineModelSelect>(modelSelectWindowPrefab);
                    spineModelSelect.Initialize(modelSet, (AtlasAssetPair atlasAssetPair) => 
                    {
                        SpineControllerTypeA.ModelPair modelPair = spineController.AddModel(atlasAssetPair);
                        modelPair.Model.transform.position = spineAniGIFGenerator.modelPosition;
                        modelPair.Model.transform.localScale = spineAniGIFGenerator.modelScale;
                        Refresh();
                    });
                });
            });
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();
            GenerateButton();
            foreach (var model in spineAniGIFGenerator.spineController.models)
            {
                model.Model.AnimationState.SetAnimation(0, animationText.text, true);
            }
        }

        public void SelectAnimation()
        {
            SpineAnimationSelect.SpineAnimationSelect spineAnimationSelect = spineAniGIFGenerator.window.OpenWindow<SpineAnimationSelect.SpineAnimationSelect>(animationSelectWindowPrefab);
            spineAnimationSelect.Initialize((string str) =>
            {
                animationText.text = str;
                Refresh();
            });
        }
    }
}