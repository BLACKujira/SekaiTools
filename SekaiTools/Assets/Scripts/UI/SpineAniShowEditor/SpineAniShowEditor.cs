using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using SekaiTools.UI.SpineLayer;
using SekaiTools.UI.SpineSettings;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniShowEditor
{
    //public class SpineAniShowEditor : MonoBehaviour
    //{
    //    public TextAsset json;
    //    [Header("Components")]
    //    public SpineControllerTypeA spineControllerPrefab;
    //    public SpineImage spineImage;
    //    public SpineAniShowEditor_EditArea editArea;
    //    [Header("Settings")]
    //    public InbuiltSpineModelSet spineModelSet;
    //    public BackGroundPart bgpSpinePrefab;
    //    [Header("Message")]
    //    public MessageLayer.MessageLayerBase messageLayer;

    //    [System.NonSerialized] public SpineControllerTypeA spineController;
    //    [System.NonSerialized] public SpineAniShowData spineAniShowData;

    //    public SpineScene currentScene => spineScenes[currentID];

    //    public SpineScene[] spineScenes => spineAniShowData.spineScenes.ToArray();
    //    public BackGroundController backGroundController => BackGroundController.backGroundController;

    //    int currentID = 0;

    //    private void Awake()
    //    {
    //        spineController = Instantiate(spineControllerPrefab);
    //        spineImage.spineController = spineController;
    //        spineImage.Initialize(spineController,(int value)=> {if(editArea.gameObject.activeSelf) editArea.UpdateInfo(); }, null, null);

    //        SpineAniShowData spineAniShowData = JsonUtility.FromJson<SpineAniShowData>(File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas"));
    //        spineAniShowData.savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas";
    //        Initialize(spineAniShowData);

    //    }

    //    private void Initialize(SpineAniShowData spineAniShowData)
    //    {
    //        this.spineAniShowData = spineAniShowData;
    //        ShowScene(0);
    //    }

    //    public void Prev()
    //    {
    //        if (currentID <= 0) return;
    //        spineAniShowData.spineScenes[currentID] = spineController.GetSaveData();
    //        ShowScene(--currentID);

    //        spineImage.UpdateInfo();
    //    }

    //    public void Next()
    //    {
    //        if (currentID >= spineScenes.Length - 1) return;
    //        spineAniShowData.spineScenes[currentID] = spineController.GetSaveData();
    //        ShowScene(++currentID);

    //        spineImage.UpdateInfo();
    //    }

    //    public void ShowScene(int index)
    //    {
    //        spineController.LoadData(spineScenes[index]);
    //        backGroundController.ClearAndReset();
    //        backGroundController.Load(spineScenes[index].backGroundData);
    //        backGroundController.AddDecoration(bgpSpinePrefab,1);

    //        spineImage.UpdateInfo();
    //    }

    //    private void OnDestroy()
    //    {
    //        Destroy(spineController);
    //    }

    //    public void PlayFromBeginning()
    //    {
    //        SpineScene spineScene = spineScenes[currentID];
    //        for (int i = 0; i < spineScene.spineObjects.Length; i++)
    //        {
    //            spineController.SetModel(spineScene.spineObjects[i], i);
    //        }
    //    }

    //    public void SaveData()
    //    {
    //        spineAniShowData.SaveData();
    //        messageLayer.ShowMessage("保存成功");
    //    }
    //}

    public class SpineAniShowEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public SaveTipCloseWindowButton closeWindowButton;
        [Header("Settings")]
        public SpineSceneSet spineSceneSet;
        [Header("Prefab")]
        public Window itemSelectorWindow;
        public GameObject addItemButton;
        [Header("Message")]
        public MessageLayer.MessageLayerBase msgLayer_Msg;
        public MessageLayer.MessageLayerBase msgLayer_Err;

        [HideInInspector] public SpineAniShowData data;

        public void Initialize(SpineAniShowData data)
        {
            this.data = data;
            closeWindowButton.Initialize(() => data.SavePath);
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();
            foreach (var spineScene in data.spineScenes)
            {
                universalGenerator.AddItem((gobj) =>
                {
                    SpineAniShowEditor_Item spineAniShowEditor_Item = gobj.GetComponent<SpineAniShowEditor_Item>();
                    spineAniShowEditor_Item.Initialize(spineScene, this);
                });
            }
            universalGenerator.AddItem(addItemButton, (gobj) =>
             {
                 Button button = gobj.GetComponent<Button>();
                 button.onClick.AddListener(() =>
                 {
                    UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(itemSelectorWindow);
                     universalSelector.Generate(spineSceneSet.scenes.Count, (btn, id) =>
                      {
                          ButtonWithIconAndText buttonWithIconAndText = btn.GetComponent<ButtonWithIconAndText>();
                          buttonWithIconAndText.Label = spineSceneSet[id].name;
                          buttonWithIconAndText.Icon = spineSceneSet[id].preview;
                      },
                     (id) =>
                     {
                         data.spineScenes.Add(spineSceneSet[id].GetInstance());
                         Refresh();
                         universalSelector.window.Close();
                     });
                 });
             });
        }

        public void Save()
        {
            for (int i = 0; i < data.spineScenes.Count; i++)
            {
                SpineSceneWithMeta spineSceneWithMeta = data.spineScenes[i];
                if (spineSceneWithMeta.useTransition&&spineSceneWithMeta.transition==null)
                {
                    WindowController.ShowMessage("无法保存文件", $"第{i + 1}个场景开启了转场但没有设置转场。");
                    return;
                }
            }

            data.SaveData();
            msgLayer_Msg.ShowMessage("保存完成");
        }
    }
}