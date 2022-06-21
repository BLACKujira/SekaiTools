using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using SekaiTools.UI.SpineLayer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public SpineImage spineImage;
        [Header("Settings")]
        public InbuiltSpineModelSet spineModelSet;
        public SpineSceneEditor_Main_EditArea editArea;
        [Header("Prefab")]
        public BackGroundPart bgpSpinePrefab;
        public SpineControllerTypeA spineControllerPrefab;
        public Window spineModelSelectPrefab;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public SpineScene spineScene;
        [System.NonSerialized] public BackGroundPart bgpSpine;

        public BackGroundController backGroundController => BackGroundController.backGroundController;

        public void SetScene(SpineScene spineScene)
        {
            this.spineScene = spineScene;
            if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
            bgpSpine = backGroundController.AddDecoration(bgpSpinePrefab, Mathf.Min(spineScene.spineLayerID,backGroundController.Decorations.Count));
            Refresh();
        }

        public void Refresh()
        {
            spineController.ClearModel();
            spineController.LoadData(spineScene);
            spineImage.UpdateInfo();
        }

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
            spineImage.Initialize(spineController,(int value) => { if (editArea.gameObject.activeSelf) editArea.UpdateInfo(); }, null, null);
        }

        public void PlayFromBeginning()
        {
            for (int i = 0; i < spineController.models.Count; i++)
            {
                SpineControllerTypeA.ModelPair modelPair = spineController.models[i];
                global::Spine.TrackEntry trackEntry = modelPair.Model.AnimationState.SetAnimation(0, modelPair.Model.AnimationName, true);
                trackEntry.TrackTime = trackEntry.animationEnd * modelPair.animationProgress;
            }
        }

        public void SaveChanges()
        {
            spineScene = spineController.GetSaveData();
        }

        public void AddModel()
        {
            SpineModelSelect.SpineModelSelect spineModelSelect = window.OpenWindow<SpineModelSelect.SpineModelSelect>(spineModelSelectPrefab);
            spineModelSelect.Initialize(spineModelSet,
                (atlasAssetPair) =>
                {
                    spineController.AddModel(atlasAssetPair);
                    SaveChanges();
                    Refresh();
                });
        }

        private void OnDestroy()
        {
            if (spineController) Destroy(spineController.gameObject);
            if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
        }

        private void OnEnable()
        {
            if (spineController) spineController.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (spineController) spineController.gameObject.SetActive(false);
        }
    }
}