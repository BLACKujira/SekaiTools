using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools;
using SekaiTools.Live2D;
using System.IO;
using SekaiTools.Cutin;
using SekaiTools.UI.CutinSceneEditor;
using Spine.Unity;
using SekaiTools.UI.SpineLayer;
using SekaiTools.UI.BackGround;
using SekaiTools.Spine;
using static SekaiTools.UI.BackGround.BackGroundController;
using SekaiTools.Count;
using SekaiTools.IO;

public class _Test : MonoBehaviour
{
    public float animationSpeed = 0.8403361344537815f * 2;
    public List<string> atlasAssets;
    public SpineControllerTypeA_DefaultSettings defaultSettings;
    public Vector3 modelScale = new Vector3(0.69f, 0.69f,1);
    public L2DControllerTypeC l2DController;

    public class Item
    {
        public List<string> atlasAssets;
        public TextAsset bgSettings;
    }

    SpineAniShowData spineAniShowData = new SpineAniShowData();

    private void Awake()
    {
        string json = MessagePackConverter.ToJSON(File.ReadAllBytes(@"C:\Users\KUROKAWA_KUJIRA\Desktop\0\2.0.5.41.dec"));
        File.WriteAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\0\2.0.5.41.json", json);

    }

    void AddSpineData(string backGroundSaveData,params SpineScene.SpineObject[] spineObjects)
    {
        SpineScene spineScene = new SpineScene();
        spineScene.backGroundData = JsonUtility.FromJson<BackGroundSaveData>(backGroundSaveData);
        spineScene.spineLayerID = 1;
        spineScene.spineObjects = spineObjects;
        spineAniShowData.spineScenes.Add(spineScene);
    }

    SpineScene.SpineObject GetSpineObject(string atlasAsset,int id,int count)
    {
        SpineScene.SpineObject spineObject = new SpineScene.SpineObject();
        SpineControllerTypeA_DefaultSettings.DefaultSettingItem defaultSettingItem = defaultSettings.GetDefaultSetting(id, count);
        spineObject.atlasAssetName = atlasAsset;
        spineObject.position = defaultSettingItem.position;
        spineObject.scale = modelScale;
        spineObject.ifFlip = defaultSettingItem.flipX;
        spineObject.animation = "z_test_F_negi01";
        spineObject.animationProgress = 0;
        spineObject.animationSpeed = animationSpeed;
        int order = id;
        order *= 2;
        if (atlasAsset.Equals("sd_negionly_normal")) order++;
        spineObject.sortingOrder = order;

        return spineObject;
    }
}
