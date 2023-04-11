using SekaiTools.DecompiledClass;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class GIP_ExtraFunc : MonoBehaviour, IGenericInitializationPart
    {
        [Header("Components")]
        public Toggle togAssetList;
        public AssetListGetter assetListGetter;

        public bool UseAssetList => togAssetList.isOn;
        public BundleRoot BundleRoot => assetListGetter.GetBundleRoot();

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (togAssetList.isOn && string.IsNullOrEmpty(assetListGetter.lfsi_AssetList.SelectedPath))
                errors.Add("未选择数据表文件");
            return GenericInitializationCheck.GetErrorString("强化功能错误", errors);
        }
    }
}