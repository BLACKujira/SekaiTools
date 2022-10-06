using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using SekaiTools.UI.SpineLayer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
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
        public MessageLayer.MessageLayerBase msgLayer_Msg;
        public MessageLayer.MessageLayerBase msgLayer_Err;

        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public BackGroundPart bgpSpine;

        public SpineScene spineScene => spineController.GetSaveData();

        public BackGroundController backGroundController => BackGroundController.backGroundController;

        public void SetScene(SpineScene spineScene)
        {
            if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
            bgpSpine = backGroundController.AddDecoration(bgpSpinePrefab, Mathf.Min(spineScene.spineLayerID,backGroundController.Decorations.Count));
            spineController.ClearModel();
            try
            {
                spineController.LoadData(spineScene);
            }
            catch(SpineControllerTypeA.ModelNotFoundException mnfe)
            {
                WindowController.ShowMessage($"模型未找到 {mnfe.Message}", "退出此界面后请不要保存，这有可能导致此场景原来的的模型资料丢失");
            }
            catch(System.Exception ex) 
            {
                msgLayer_Err.ShowMessage($"错误\n{ex.Message}");
            }

            spineImage.ResetAll();
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

        public void AddModel()
        {
            SpineModelSelect.SpineModelSelect spineModelSelect = window.OpenWindow<SpineModelSelect.SpineModelSelect>(spineModelSelectPrefab);
            spineModelSelect.Initialize(spineModelSet,
                (atlasAssetPair) =>
                {
                    spineController.AddModel(atlasAssetPair);
                    spineImage.ResetAll();
                });
        }

        int lastModelId = -1;
        private void Update()
        {
            if (!editArea.gameObject.activeSelf) return;
            if (lastModelId != spineImage.selectedID)
            {
                editArea.UpdateInfo();
            }
            lastModelId = spineImage.selectedID;
        }

        private void OnDestroy()
        {
            if (spineController) Destroy(spineController.gameObject);
            if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
        }

        private void OnEnable()
        {
            if (spineController) spineController.gameObject.SetActive(true);
            PlayFromBeginning();
        }

        private void OnDisable()
        {
            if (spineController) spineController.gameObject.SetActive(false);
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_SS);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string saveData = JsonUtility.ToJson(spineController.GetSaveData());
            File.WriteAllText(saveFileDialog.FileName, saveData);

            msgLayer_Msg.ShowMessage("保存完成");
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_SS);
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            try
            {
                string saveData = File.ReadAllText(openFileDialog.FileName);
                SpineScene spineScene = JsonUtility.FromJson<SpineScene>(saveData);
                spineController.LoadData(spineScene);
                spineImage.ResetAll();
            }
            catch(System.Exception ex)
            {
                WindowController.ShowMessage($"读取失败", ex.Message);
            }
        }

        public void ResetPosition()
        {
            WindowController.ShowCancelOK("重置模型位置", "确定将模型放置到默认位置吗?", () =>
              {
                  spineController.ResetPosition();
                  spineImage.ResetPosition();
                  msgLayer_Msg.ShowMessage("已重置模型位置");
              });
        }
    }
}