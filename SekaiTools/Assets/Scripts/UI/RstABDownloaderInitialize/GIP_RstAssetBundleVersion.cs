using SekaiTools.OtherGames.ReStage;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.RstABDownloaderInitialize
{
	public class GIP_RstAssetBundleVersion : MonoBehaviour , IGenericInitializationPart
	{
		public LoadFileSelectItem assetBundleVersionsFile;
		public List<AssetBundleVersion> assetBundleVersions => AssetBundleVersion.ToList(File.ReadAllText(assetBundleVersionsFile.SelectedPath));

        public string CheckIfReady()
        {
            if (string.IsNullOrEmpty(assetBundleVersionsFile.SelectedPath))
                return "Î´Ñ¡ÔñÎÄ¼þ";
            return null;
        }
    }
}