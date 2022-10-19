using SekaiTools.DecompiledClass;
using SekaiTools.UI;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.AssetDownloaderInitialize
{
    public enum AssetListType { Undecrypted, MessagePack, RawJSON, JSON }

    public class GIP_AssetList : MonoBehaviour, IGenericInitializationPart
    {
        public InputField if_URLHead; 
        public LoadFileSelectItem lfsi_AssetList;
        public Dropdown dd_AssetListType;

        public void Initialize()
        {
            //与枚举AssetListType一一对应
            dd_AssetListType.options = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData("未解密的MessagePack通信"),
                new Dropdown.OptionData("解密的MessagePack通信"),
                new Dropdown.OptionData("MessagePack直接转换成的JSON,未经处理"),
                new Dropdown.OptionData("可直接读取的JSON")
            };
        }

        public string GetURLHead()
        {
            string urlHead = if_URLHead.text;
            if (!urlHead.EndsWith("/"))
                urlHead += '/';
            return urlHead;
        }

        public BundleRoot GetBundleRoot()
        {
            switch ((AssetListType)dd_AssetListType.value)
            {
                case AssetListType.Undecrypted:
                    WindowController.windowController.SendMessage("错误", "此功能模块还未完成");
                    break;
                case AssetListType.MessagePack:
                    byte[] msgPack = File.ReadAllBytes(lfsi_AssetList.SelectedPath);
                    string json = MessagePackConverter.ToJSONAssetList(
                                                msgPack);
                    return JsonUtility.FromJson<BundleRoot>(
                        json);
                case AssetListType.RawJSON:
                    return JsonHelper.getJsonArray<BundleRoot>(
                        MessagePackConverter.ModifyAssetListJSON(
                            File.ReadAllText(lfsi_AssetList.SelectedPath)))[0];
                case AssetListType.JSON:
                    return JsonHelper.getJsonArray<BundleRoot>(
                            File.ReadAllText(lfsi_AssetList.SelectedPath))[0];
                default:
                    break;
            }
            return null;
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(if_URLHead.text))
                errors.Add("URL起始字符串为空");
            if (string.IsNullOrEmpty(lfsi_AssetList.SelectedPath))
                errors.Add("未选择数据表文件");
            return GenericInitializationCheck.GetErrorString("URL或数据表错误", errors);
        }
    }
}