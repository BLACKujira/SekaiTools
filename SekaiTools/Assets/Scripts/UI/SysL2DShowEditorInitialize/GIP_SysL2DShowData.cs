using SekaiTools.SystemLive2D;
using SekaiTools.UI.GenericInitializationParts;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowEditorInitialize
{
    public class GIP_SysL2DShowData : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public SaveFileSelectItem file_SaveData;
        public LoadFileSelectItem file_LoadData;
        public Text txt_DataInfo;
        [Header("Settings")]
        public Window sysL2DSelectorPrefab;

        SysL2DShowData loadedData = null;
        SysL2DShowData createdData = null;
        bool ifNewFile = true;
        public SysL2DShowData SysL2DShowData => ifNewFile ? createdData : loadedData;

        public event Action<SysL2DShowData> onDataChanged;

        private void Awake()
        {
            Action<string> setSaveDataPath = (str) =>
                           {
                               if (createdData != null)
                                   createdData.SavePath = str;
                               RefreshDataInfo();
                           };
            if (file_SaveData)
            {
                file_SaveData.onPathSelect += setSaveDataPath;
                file_SaveData.onPathReset += setSaveDataPath;
            }
            file_LoadData.onPathSelect += (str) =>
             {
                 string json = File.ReadAllText(str);
                 SysL2DShowData sysL2DShowData = JsonUtility.FromJson<SysL2DShowData>(json);
                 loadedData = sysL2DShowData;
                 loadedData.SavePath = str;
                 if (onDataChanged!=null) onDataChanged(loadedData);
                 RefreshDataInfo();
             };
            file_LoadData.onPathReset += (_) =>
            {
                loadedData = null;
                if (onDataChanged != null) onDataChanged(loadedData);
                RefreshDataInfo();
            };
            RefreshDataInfo();
        }

        public void SelectSysL2D()
        {
            SysL2DSelect.SysL2DSelect sysL2DSelect = 
                WindowController.windowController.currentWindow.OpenWindow<SysL2DSelect.SysL2DSelect>(sysL2DSelectorPrefab);
            sysL2DSelect.Initialize(createdData,(data) =>
            {
                createdData = data;
                createdData.SavePath = file_SaveData.SelectedPath;
                RefreshDataInfo();
                if (onDataChanged != null) onDataChanged(data);
            });
        }

        void RefreshDataInfo()
        {
            if (txt_DataInfo == null)
                return;

            if(createdData == null)
            {
                txt_DataInfo.text = "请创建存档";
            }
            else
            {
                if (createdData.sysL2DShows.Count == 0)
                    txt_DataInfo.text = "存档为空，请重新创建存档";
                else
                    txt_DataInfo.text = $"创建的存档内有{createdData.sysL2DShows.Count}个片段";
            }
        }

        public void SwitchMode_Create() => ifNewFile = true;
        public void SwitchMode_Load() => ifNewFile = false;

        public string CheckIfReady()
        {
            if (ifNewFile)
            {
                if(string.IsNullOrEmpty(file_SaveData.SelectedPath))
                    return GenericInitializationCheck.GetErrorString("目录错误", "无效的目录");
                if (createdData == null)
                    return GenericInitializationCheck.GetErrorString("存档错误", "没有创建存档");
                else if(createdData.sysL2DShows.Count == 0)
                    return GenericInitializationCheck.GetErrorString("存档错误", "存档为空");
            }
            else if (!ifNewFile)
            {
                if (string.IsNullOrEmpty(file_LoadData.SelectedPath))
                    return GenericInitializationCheck.GetErrorString("目录错误", "无效的目录");
                if (!File.Exists(file_LoadData.SelectedPath))
                    return GenericInitializationCheck.GetErrorString("目录错误", "文件不存在");
            }
            return null;
        }
    }
}