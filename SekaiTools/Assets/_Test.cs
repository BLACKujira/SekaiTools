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

public class _Test : MonoBehaviour
{
    public float animationSpeed = 0.8403361344537815f * 2;
    public List<string> atlasAssets;
    public SpineControllerTypeA_DefaultSettings defaultSettings;
    public Vector3 modelScale = new Vector3(0.69f, 0.69f,1);

    public class Item
    {
        public List<string> atlasAssets;
        public TextAsset bgSettings;
    }

    SpineAniShowData spineAniShowData = new SpineAniShowData();

    private void Awake()
    {
        AddSpineData(SpineAniShowData.bgs_lnSekai,
                GetSpineObject("sd_01ichika_cloth001",0,4),
                GetSpineObject("sd_negionly_normal", 0,4),
                
                GetSpineObject("sd_02saki_cloth001", 1,4),
                GetSpineObject("sd_negionly_normal", 1, 4),
                
                GetSpineObject("sd_03honami_cloth001", 2,4),
                GetSpineObject("sd_negionly_normal", 2, 4),

                GetSpineObject("sd_04shiho_cloth001", 3,4),
                GetSpineObject("sd_negionly_normal", 3, 4)
                );

        AddSpineData(SpineAniShowData.bgs_lnSekai,
                    GetSpineObject("sd_21miku_band", 0, 6),
                GetSpineObject("sd_negionly_normal", 0, 6),

                    GetSpineObject("sd_22rin_band", 1, 6),
                GetSpineObject("sd_negionly_normal", 1, 6),

                    GetSpineObject("sd_23len_band", 2, 6),
                GetSpineObject("sd_negionly_normal", 2, 6),

                    GetSpineObject("sd_24luka_band", 3, 6),
                GetSpineObject("sd_negionly_normal", 3, 6),

                    GetSpineObject("sd_25meiko_band", 4, 6),
                GetSpineObject("sd_negionly_normal", 4, 6),

                    GetSpineObject("sd_26kaito_band", 5, 6),
                GetSpineObject("sd_negionly_normal", 5, 6)
                    );

        AddSpineData(SpineAniShowData.bgs_mmjSekai,
                GetSpineObject("sd_05minori_cloth001", 0, 4),
                GetSpineObject("sd_negionly_normal", 0,4),
        
                GetSpineObject("sd_06haruka_cloth001", 1, 4),
                GetSpineObject("sd_negionly_normal", 1,4),
        
                GetSpineObject("sd_07airi_cloth001", 2, 4),
                GetSpineObject("sd_negionly_normal", 2,4),
       
                GetSpineObject("sd_08shizuku_cloth001", 3, 4),
                GetSpineObject("sd_negionly_normal", 3,4)
        );

        AddSpineData(SpineAniShowData.bgs_mmjSekai,
                GetSpineObject("sd_21miku_idol", 0, 6),
                GetSpineObject("sd_negionly_normal", 0, 6),

            
                GetSpineObject("sd_22rin_idol", 1, 6),
                GetSpineObject("sd_negionly_normal", 1,6),
            
                GetSpineObject("sd_23len_idol", 2, 6),
                GetSpineObject("sd_negionly_normal", 2,6),
            
                GetSpineObject("sd_24luka_idol", 3, 6),
                GetSpineObject("sd_negionly_normal", 3,6),
            
                GetSpineObject("sd_25meiko_idol", 4, 6),
                GetSpineObject("sd_negionly_normal", 4,6),
            
                GetSpineObject("sd_26kaito_idol", 5, 6),
                GetSpineObject("sd_negionly_normal", 5,6)
            );

        AddSpineData(SpineAniShowData.bgs_vbsSekai,
                GetSpineObject("sd_09kohane_unit", 0, 4),
                GetSpineObject("sd_negionly_normal", 0, 4),
        
                GetSpineObject("sd_10an_unit", 1, 4),
                GetSpineObject("sd_negionly_normal", 1,4),
        
                GetSpineObject("sd_11akito_unit", 2, 4),
                GetSpineObject("sd_negionly_normal", 2,4),
        
                GetSpineObject("sd_12touya_unit", 3, 4),
                GetSpineObject("sd_negionly_normal", 3,4)
        );

        AddSpineData(SpineAniShowData.bgs_vbsSekai,
            GetSpineObject("sd_21miku_street", 0, 6),
                GetSpineObject("sd_negionly_normal", 0, 6),

            GetSpineObject("sd_22rin_street", 1, 6),
                GetSpineObject("sd_negionly_normal", 1, 6),

            GetSpineObject("sd_23len_street", 2, 6),
                GetSpineObject("sd_negionly_normal", 2, 6),

            GetSpineObject("sd_24luka_street", 3, 6),
                GetSpineObject("sd_negionly_normal", 3, 6),

            GetSpineObject("sd_25meiko_street", 4, 6),
                GetSpineObject("sd_negionly_normal", 4, 6),

            GetSpineObject("sd_26kaito_street", 5, 6),
                GetSpineObject("sd_negionly_normal", 5, 6)
            );

        AddSpineData(SpineAniShowData.bgs_wsSekai,
        GetSpineObject("sd_13tsukasa_cloth001", 0, 4),
                GetSpineObject("sd_negionly_normal", 0, 4),
        
                GetSpineObject("sd_14emu_cloth001", 1, 4),
                GetSpineObject("sd_negionly_normal", 1, 4),
        
                GetSpineObject("sd_15nene_cloth001", 2, 4),
                GetSpineObject("sd_negionly_normal", 2, 4),
        
                GetSpineObject("sd_16rui_cloth001", 3, 4),
                GetSpineObject("sd_negionly_normal", 3, 4)
        );

        AddSpineData(SpineAniShowData.bgs_wsSekai,
            GetSpineObject("sd_21miku_wonder", 0, 6),
                GetSpineObject("sd_negionly_normal", 0, 6),

            GetSpineObject("sd_22rin_wonder", 1, 6),
                GetSpineObject("sd_negionly_normal", 1, 6),
            
                GetSpineObject("sd_23len_wonder", 2, 6),
                GetSpineObject("sd_negionly_normal", 2, 6),
            
                GetSpineObject("sd_24luka_wonder", 3, 6),
                GetSpineObject("sd_negionly_normal", 3, 6),
            
                GetSpineObject("sd_25meiko_wonder", 4, 6),
                GetSpineObject("sd_negionly_normal", 4, 6),
            
                GetSpineObject("sd_26kaito_wonder", 5, 6),
                GetSpineObject("sd_negionly_normal",5, 6)
            );

        AddSpineData(SpineAniShowData.bgs_25Sekai,
            GetSpineObject("sd_17kanade_normal", 0, 4),
                GetSpineObject("sd_negionly_normal", 0, 4),
            
                GetSpineObject("sd_18mafuyu_cloth001", 1, 4),
                GetSpineObject("sd_negionly_normal", 1, 4),
            
                GetSpineObject("sd_19ena_cloth001", 2, 4),
                GetSpineObject("sd_negionly_normal", 2, 4),
            
                GetSpineObject("sd_20mizuki_cloth001", 3, 4),
                GetSpineObject("sd_negionly_normal", 3, 4)
            );

        AddSpineData(SpineAniShowData.bgs_25Sekai,
        GetSpineObject("sd_21miku_night", 0, 4),
                GetSpineObject("sd_negionly_normal", 0, 4),

        GetSpineObject("sd_22rin_night", 1, 4),
                GetSpineObject("sd_negionly_normal", 1, 4),

        GetSpineObject("sd_24luka_night", 2, 4),
                GetSpineObject("sd_negionly_normal", 2, 4),

        GetSpineObject("sd_25meiko_night", 3, 4),
                GetSpineObject("sd_negionly_normal", 3, 4)
        );

        string json = JsonUtility.ToJson(spineAniShowData,true);
        File.WriteAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas", json);
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
