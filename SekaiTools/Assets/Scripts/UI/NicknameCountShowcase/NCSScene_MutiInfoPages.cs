using SekaiTools.Count;
using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_MutiInfoPages : NCSScene, IImageFileReference
    {
        [Header("Components")]
        public NCSScene_MutiInfoPage_PageBase[] pages;
        [Header("Settings")]
        public float defaultModelScale = 0.6f;
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public BackGroundPart bgpSpinePrefab;

        [System.NonSerialized] int talkerId = 1;
        [System.NonSerialized] public SpineScene spineScene;
        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public BackGroundPart bgpSpine;

        public InbuiltSpineModelSet spineModelSet => GlobalData.globalData.inbuiltSpineModelSet;
        public override ConfigUIItem[] configUIItems
        {
            get
            {
                ConfigUIItem[] configUIItemsBase = new ConfigUIItem[]
                            {
                new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Character("角色","scene",()=>talkerId,(value)=>talkerId = value),
                new ConfigUIItem_SpineScene("编辑Spine场景","spine",()=>spineScene,(value)=>spineScene = value),
                new ConfigUIItem_ResetSpineScene("重设Spine场景","spine",ResetSpineScene),
                new ConfigUIItem_Int("Spine背景部件图层","spine",()=>spineScene.spineLayerID,(value)=>{spineScene.spineLayerID = value; ResetBGPSpine(); })
                            };
                List<ConfigUIItem> configUIItems = new List<ConfigUIItem>(configUIItemsBase);
                foreach (var pageBase in pages)
                {
                    if(pageBase.ConfigUIItems!=null)
                        configUIItems.AddRange(pageBase.ConfigUIItems);
                }
                return configUIItems.ToArray();
            }
        }

        public override string Information => $"角色 {ConstData.characters[talkerId].Name} , 持续时间 {holdTime.ToString("0.00")}";

        public List<string> requireImageKeys;
        public HashSet<string> RequireImageKeys => new HashSet<string>(requireImageKeys);

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            this.holdTime = settings.holdTime;
            this.talkerId = settings.talkerId;
            this.spineScene = settings.spineScene;
            this.requireImageKeys = settings.requireImageKeys;
            if (bgpSpine == null) bgpSpine = BackGroundController.backGroundController.AddDecoration(bgpSpinePrefab, spineScene.spineLayerID);
        }

        public override void NewData()
        {
            ResetSpineScene();
        }

        private void ResetSpineScene()
        {
            spineController.ClearModel();
            string[] defaultSpineModel = GlobalData.globalData.defaultSpineModels.values;

            List<NicknameCountItem> nicknameCountItems = new List<NicknameCountItem>();
            for (int i = 1; i < 27; i++)
            {
                if (i == talkerId) continue;
                NicknameCountItem nicknameCountItem = countData[talkerId, i];
                nicknameCountItems.Add(nicknameCountItem);
            }
            nicknameCountItems.Sort((x, y) => -x.Total.CompareTo(y.Total));

            int[] remapArray = new int[] { 3, 1, 0, 2, 4 };

            for (int i = 0; i < 5; i++)
            {
                Vector3 modelScale = new Vector3(defaultModelScale, defaultModelScale, 1);
                int nameId = nicknameCountItems[remapArray[i]].nameId;
                spineController.AddModel(spineModelSet.GetValue(defaultSpineModel[ConstData.GetUnitVirtualSinger(nameId, ConstData.characters[talkerId].unit)])).Model.transform.localScale = modelScale;
                if (i == 1)
                    spineController.AddModel(spineModelSet.GetValue(defaultSpineModel[talkerId])).Model.transform.localScale = modelScale;
            }
            spineController.ResetPosition();

            int[] layerArray = new int[] { 4, 2, 0, 1, 3, 5 };
            spineScene = spineController.GetSaveData();
            for (int i = 0; i < layerArray.Length; i++)
            {
                int layer = layerArray[i];
                spineScene.spineObjects[i].sortingOrder = layer;
            }
        }

        public override void Refresh()
        {
            StopAllCoroutines();
            RefreshPages();
            spineController.ClearModel();
            spineController.LoadData(spineScene);
        }

        private void RefreshPages()
        {
            foreach (var pageBase in pages)
            {
                pageBase.Initialize(player.countData, talkerId, player);
            }
            requireImageKeys = new List<string>();
            foreach (var page in pages)
            {
                if (page is IImageFileReference imageFileReference)
                    requireImageKeys.AddRange(imageFileReference.RequireImageKeys);
            }
        }

        public void ResetBGPSpine()
        {
            if (bgpSpine != null) BackGroundController.backGroundController.RemoveDecoration(bgpSpine);
            bgpSpine = BackGroundController.backGroundController.AddDecoration(bgpSpinePrefab, spineScene.spineLayerID);
        }

        private void Awake()
        {
            if (!spineController) spineController = Instantiate(spineControllerPrefab);
        }

        private void OnDestroy()
        {
            if (bgpSpine != null) BackGroundController.backGroundController.RemoveDecoration(bgpSpine);
            if (spineController) Destroy(spineController.gameObject);
        }

        private void OnEnable()
        {
            if (spineScene != null && bgpSpine == null) bgpSpine = BackGroundController.backGroundController.AddDecoration(bgpSpinePrefab, spineScene.spineLayerID);
            spineController.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (bgpSpine != null) BackGroundController.backGroundController.RemoveDecoration(bgpSpine);
            spineController.gameObject.SetActive(false);
        }

        public override void Initialize(NCSPlayerBase player)
        {
            base.Initialize(player);
            RefreshPages();
            spineController.gameObject.SetActive(true);
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int talkerId;
            public SpineScene spineScene;
            public List<string> requireImageKeys;

            public Settings(NCSScene_MutiInfoPages nCSScene_MutiInfoPages)
            {
                holdTime = nCSScene_MutiInfoPages.holdTime;
                talkerId = nCSScene_MutiInfoPages.talkerId;
                spineScene = nCSScene_MutiInfoPages.spineScene;
                requireImageKeys = nCSScene_MutiInfoPages.requireImageKeys;
            }
        }
    }
}