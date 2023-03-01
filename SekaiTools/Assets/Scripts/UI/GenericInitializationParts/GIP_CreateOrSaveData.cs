using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    public abstract class GIP_CreateOrSaveData : MonoBehaviour, IGenericInitializationPart
    {
        [Header("Components")]
        public SaveFileSelectItem file_SaveData;
        public LoadFileSelectItem file_LoadData;

        bool ifNewFile = true;

        public string SelectedDataPath => ifNewFile ? file_SaveData.SelectedPath : file_LoadData.SelectedPath;
        public bool IfNewFile => ifNewFile;

        public virtual string CheckIfReady()
        {
            return GenericInitializationCheck.GetErrorString("目录错误", GetErrorList());
        }

        protected virtual List<string> GetErrorList()
        {
            List<string> errorList = new List<string>();
            if (ifNewFile && string.IsNullOrEmpty(file_SaveData.SelectedPath))
            {
                errorList.Add("无效的目录");
            }
            else if (!ifNewFile)
            {
                if (string.IsNullOrEmpty(file_LoadData.SelectedPath))
                    errorList.Add("无效的目录");
                if (!File.Exists(file_LoadData.SelectedPath))
                    errorList.Add("文件不存在");
            }
            return errorList;
        }

        public void SwitchMode_Create() => ifNewFile = true;
        public void SwitchMode_Load() => ifNewFile = false;
    }
}