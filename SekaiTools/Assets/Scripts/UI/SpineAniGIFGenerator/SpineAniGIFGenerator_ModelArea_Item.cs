using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Spine;

namespace SekaiTools.UI.SpineAniGIFGenerator
{
    public class SpineAniGIFGenerator_ModelArea_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image iconImage;
        public Text labelText;
        public Button deleteButton;
        public Button configButton;
        public Button layerUpButton;
        public Button layerDownButton;
        [Header("Settings")]
        public InbuiltImageData imageData;
        public InbuiltSpineModelSet modelSet;
        [Header("Prefab")]
        public Window modelSelectWindowPrefab;


        public void Initialize(int modelId,SpineAniGIFGenerator_ModelArea modelArea)
        {
            SpineControllerTypeA spineController = modelArea.spineAniGIFGenerator.spineController;
            SpineControllerTypeA.ModelPair modelPair = spineController.models[modelId];
            iconImage.sprite = imageData.GetValue(modelPair.Name);
            labelText.text = modelPair.Name;
            deleteButton.onClick.AddListener(() => { spineController.RemoveModel(modelId); modelArea.Refresh(); });
            configButton.onClick.AddListener(() => 
            {
                SpineModelSelect.SpineModelSelect spineModelSelect = modelArea.spineAniGIFGenerator.window.OpenWindow<SpineModelSelect.SpineModelSelect>(modelSelectWindowPrefab);
                spineModelSelect.Initialize(modelSet, (AtlasAssetPair atlasAssetPair) => { spineController.ReplaceModel(atlasAssetPair, modelId); modelArea.Refresh(); }); 
            });
            layerUpButton.onClick.AddListener(() => { spineController.SortingOrderUp(spineController.models[modelId]); modelArea.Refresh(); });
            layerDownButton.onClick.AddListener(() => { spineController.SortingOrderDown(spineController.models[modelId]); modelArea.Refresh(); });
        }
    }
}