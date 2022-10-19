using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.AssetDownloaderInitialize
{
    public class GIP_AssetListSettings : MonoBehaviour, IGenericInitializationPart
    {
        public Toggle toggle_StartWith;
        public InputField if_StartWith;

        public bool UseStartWith => toggle_StartWith.isOn;

        public string GetStartWithString()
        {
            return if_StartWith.text;
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (UseStartWith && string.IsNullOrEmpty(GetStartWithString()))
                errors.Add("目录起始字符串为空");
            return GenericInitializationCheck.GetErrorString("文件筛选设置错误", errors);
        }
    }
}