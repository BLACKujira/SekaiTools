using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using SekaiTools.UI.SpineLayer;
using SekaiTools.UI.SpineSettings;
using System.IO;

namespace SekaiTools.UI.SpineAniShowEditor
{
    public class SpineAniShowEditor : MonoBehaviour
    {
        public TextAsset json;
        [Header("Components")]
        public SpineControllerTypeA spineControllerPrefab;
        public SpineImage spineImage;
        public SpineAniShowEditor_EditArea editArea;
        [Header("Settings")]
        public InbuiltSpineModelSet spineModelSet;
        public BackGroundPart bgpSpinePrefab;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public SpineAniShowData spineAniShowData;

        public SpineScene currentScene => spineScenes[currentID];

        public SpineScene[] spineScenes => spineAniShowData.spineScenes.ToArray();
        public BackGroundController backGroundController => BackGroundController.backGroundController;

        int currentID = 0;

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
            spineImage.spineController = spineController;
            spineImage.Initialize((int value)=> {if(editArea.gameObject.activeSelf) editArea.UpdateInfo(); }, null, null);

            SpineAniShowData spineAniShowData = JsonUtility.FromJson<SpineAniShowData>(File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas"));
            spineAniShowData.savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas";
            Initialize(spineAniShowData);

        }

        private void Initialize(SpineAniShowData spineAniShowData)
        {
            this.spineAniShowData = spineAniShowData;
            ShowScene(0);
        }

        public void Prev()
        {
            if (currentID <= 0) return;
            spineAniShowData.spineScenes[currentID] = spineController.GetSaveData(spineAniShowData.spineScenes[currentID]);
            ShowScene(--currentID);

            spineImage.UpdateInfo();
        }

        public void Next()
        {
            if (currentID >= spineScenes.Length - 1) return;
            spineAniShowData.spineScenes[currentID] = spineController.GetSaveData(spineAniShowData.spineScenes[currentID]);
            ShowScene(++currentID);

            spineImage.UpdateInfo();
        }

        public void ShowScene(int index)
        {
            spineController.LoadData(spineScenes[index]);
            backGroundController.ClearAndReset();
            backGroundController.Load(spineScenes[index].backGroundData);
            backGroundController.AddDecoration(bgpSpinePrefab,1);

            spineImage.UpdateInfo();
        }

        private void OnDestroy()
        {
            Destroy(spineController);
        }

        public void PlayFromBeginning()
        {
            SpineScene spineScene = spineScenes[currentID];
            for (int i = 0; i < spineScene.spineObjects.Length; i++)
            {
                spineController.SetModel(spineScene.spineObjects[i], i);
            }
        }

        public void SaveData()
        {
            spineAniShowData.SaveData();
            messageLayer.ShowMessage("保存成功");
        }
    }
}