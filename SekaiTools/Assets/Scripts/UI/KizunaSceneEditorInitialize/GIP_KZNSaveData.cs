using SekaiTools.Kizuna;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.KizunaSceneCreate;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaSceneEditorInitialize
{
    public class GIP_KZNSaveData : GIP_CreateOrSaveData
    {
        [Header("Components")]
        public Text txt_CreatedDataInfo;
        [Header("Prefab")]
        public Window createWindowPrefab;

        CreatedKizunaSceneInfo createdData;
        public KizunaSceneData KizunaSceneData
        {
            get
            {
                KizunaSceneData kizunaSceneData;
                if (IfNewFile)
                {
                    kizunaSceneData = new KizunaSceneData(createdData.bonds);
                    kizunaSceneData.AddCutinScenes(createdData.cutinSceneData);
                }
                else
                {
                    kizunaSceneData = KizunaSceneData.LoadData(File.ReadAllText(file_LoadData.SelectedPath));
                }
                kizunaSceneData.SavePath = SelectedDataPath;
                return kizunaSceneData;
            }
        }
        public CustomKizunaData CustomKizunaData
        {
            get
            {
                CustomKizunaData customKizunaData;
                if (IfNewFile)
                {
                    customKizunaData = new CustomKizunaData(createdData.bonds);
                    customKizunaData.AddCutinScenes(createdData.cutinSceneData);
                }
                else
                {
                    customKizunaData = CustomKizunaData.LoadData(File.ReadAllText(file_LoadData.SelectedPath));
                }
                return customKizunaData;
            }
        }

        private void Awake()
        {
            RefreshInfo();
        }

        public void CreateData()
        {
            KizunaSceneCreate_Step1 kizunaSceneCreate_Step1
                = WindowController.CurrentWindow.OpenWindow<KizunaSceneCreate_Step1>(createWindowPrefab);
            kizunaSceneCreate_Step1.Initialize((data) =>
            {
                createdData = data;
                RefreshInfo();
            });
        }

        public void RefreshInfo()
        {
            if (txt_CreatedDataInfo == null) return; 

            if (createdData != null)
                txt_CreatedDataInfo.text = $"存档内有{createdData.bonds.Length}个场景，{createdData.cutinSceneData.cutinScenes.Count}对互动语音，共出现{createdData.AppearCharacters.Length}名角色";
            else
                txt_CreatedDataInfo.text = Message.IO.STR_PLEASECREATEDATA;
        }

        protected override List<string> GetErrorList()
        {
            List<string> list = base.GetErrorList();
            if (IfNewFile && createdData == null)
                list.Add("未创建存档");
            return list;
        }
    }
}