using SekaiTools.UI;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
}

namespace SekaiTools.UI.AssetDownloaderInitialize
{

    public class GIP_AssetList : MonoBehaviour, IGenericInitializationPart
    {
        public InputField if_URLHead;
        public AssetListGetter assetListGetter;
        public LoadFileSelectItem lfsi_Cookie;

        public string GetURLHead()
        {
            string urlHead = if_URLHead.text;
            if (!urlHead.EndsWith("/"))
                urlHead += '/';
            return urlHead;
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(if_URLHead.text))
                errors.Add("URL起始字符串为空");
            if (string.IsNullOrEmpty(assetListGetter.lfsi_AssetList.SelectedPath))
                errors.Add("未选择数据表文件");
            if (string.IsNullOrEmpty(lfsi_Cookie.SelectedPath))
                errors.Add("未选择Cookie文件");
            return GenericInitializationCheck.GetErrorString("URL或数据表错误", errors);
        }
    }
}