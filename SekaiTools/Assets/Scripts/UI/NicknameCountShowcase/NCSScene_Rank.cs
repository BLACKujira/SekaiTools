using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using SekaiTools.Count;
using SekaiTools.UI.BackGround;
using UnityEngine.UI;
using DG.Tweening;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Rank : NCSScene, IImageFileReference
    {
        [Header("Components")]
        public NCSScene_Rank_RankPage rankPage;
        public NCSScene_Rank_InfoPage infoPage;
        public List<GameObject> _graphicsPage1;
        public List<GameObject> _graphicsPage2;
        public GameObject page1;
        public GameObject page2;
        [Header("Settings")]
        public float defaultModelScale = 0.6f;
        public float holdTimePage1 = 5;
        public float fadeTime = 1;
        [Header("Prefab")]
        public SpineControllerTypeA spineControllerPrefab;
        public BackGroundPart bgpSpinePrefab;

        List<Graphic> graphicsPage1;
        List<Graphic> graphicsPage2;

        [System.NonSerialized] int talkerId = 1;
        [System.NonSerialized] SpineScene spineScene;
        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public BackGroundPart bgpSpine;

        public InbuiltSpineModelSet spineModelSet => GlobalData.globalData.inbuiltSpineModelSet;
        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Character("角色","scene",()=>talkerId,(value)=>talkerId = value),
                new ConfigUIItem_SpineScene("编辑Spine场景","spine",()=>spineScene,(value)=>spineScene = value),
                new ConfigUIItem_ResetSpineScene("重设Spine场景","spine",ResetSpineScene)
            };

        public override string Information => $"角色 {ConstData.characters[talkerId].Name} , 持续时间 {holdTime.ToString("0.00")}";

        float[] graphicsPage1Alpha;
        float[] graphicsPage2Alpha;

        float _page1Alpha = 1;
        float page1Alpha
        {
            get => _page1Alpha;
            set
            {
                _page1Alpha = value;
                SetAlpha(value, graphicsPage1, graphicsPage1Alpha);
            }
        }
        float _page2Alpha = 1;
        float page2Alpha
        {
            get => _page2Alpha;
            set
            {
                _page2Alpha = value;
                SetAlpha(value, graphicsPage2, graphicsPage2Alpha);
            }
        }

        public HashSet<string> RequireImageKeys => new HashSet<string>() { infoPage.requireEvIconKey };

        private void SetAlpha(float alpha, List<Graphic> graphics, float[] alphas)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                Graphic graphic = graphics[i];
                Color color = graphic.color;
                color.a = Mathf.Lerp(0, alphas[i], alpha);
                graphic.color = color;
            }
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serialisedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serialisedData);
            this.holdTime = settings.holdTime;
            this.talkerId = settings.talkerId;
            infoPage.requireEvIconKey = settings.evImageKey;
            this.spineScene = settings.spineScene;
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
                spineController.AddModel(spineModelSet.GetValue(defaultSpineModel[ConstData.GetUnitVirtualSinger(nameId,ConstData.characters[talkerId].unit)])).Model.transform.localScale = modelScale;
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
            page1.SetActive(true);
            page2.SetActive(false);
            page1Alpha = 1;
            page2Alpha = 0;

            rankPage.Initialize(countData, talkerId);
            infoPage.Initialize(countData, talkerId);
            spineController.ClearModel();
            spineController.LoadData(spineScene);

            if(gameObject.activeSelf) StartCoroutine(IPageChangeCoroutine());
        }

        public IEnumerator IPageChangeCoroutine()
        {
            yield return new WaitForSeconds(holdTimePage1);
            DOTween.To((float value) => page1Alpha = value, 1, 0, fadeTime / 2);
            yield return new WaitForSeconds(fadeTime / 2);
            page1.SetActive(false);
            page2.SetActive(true);
            DOTween.To((float value) => page2Alpha = value, 0, 1, fadeTime / 2);
        }

        private void Awake()
        {
            graphicsPage1 = new List<Graphic>();
            graphicsPage2 = new List<Graphic>();
            foreach (var gameObject in _graphicsPage1)
            {
                graphicsPage1.AddRange(gameObject.GetComponentsInChildren<Graphic>());
            }
            foreach (var gameObject in _graphicsPage2)
            {
                graphicsPage2.AddRange(gameObject.GetComponentsInChildren<Graphic>());
            }

            graphicsPage1Alpha = new float[graphicsPage1.Count];
            graphicsPage2Alpha = new float[graphicsPage2.Count];

            for (int i = 0; i < graphicsPage1.Count; i++)
            {
                Graphic graphic = graphicsPage1[i];
                graphicsPage1Alpha[i] = graphic.color.a;
            }
            for (int i = 0; i < graphicsPage2.Count; i++)
            {
                Graphic graphic = graphicsPage2[i];
                graphicsPage2Alpha[i] = graphic.color.a;
            }

            if (!spineController) spineController = Instantiate(spineControllerPrefab);
        }

        private void OnDestroy()
        {
            if (bgpSpine != null) BackGroundController.backGroundController.RemoveDecoration(bgpSpine);
            if (spineController) Destroy(spineController.gameObject);
        }

        private void OnEnable()
        {
            if (spineScene != null&& bgpSpine == null) bgpSpine = BackGroundController.backGroundController.AddDecoration(bgpSpinePrefab,spineScene.spineLayerID);
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
            spineController.gameObject.SetActive(true);
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int talkerId;
            public string evImageKey;
            public SpineScene spineScene;

            public Settings(NCSScene_Rank nCSScene_Rank)
            {
                holdTime = nCSScene_Rank.holdTime;
                talkerId = nCSScene_Rank.talkerId;
                evImageKey = nCSScene_Rank.infoPage.requireEvIconKey;
                spineScene = nCSScene_Rank.spineScene;
            }
        }
    }
}