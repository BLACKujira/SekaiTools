using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.OtherGames.ReStage
{
    public class AssetBundleVersion
    {
		public const char SEPALATE_COLON = ':';

		public const char SEPALATE_CAMMA = ',';

		public const string FILE_NAME = "assetbundle_versions.txt";

		public string assetBundleName;

		public long length;

		public static List<AssetBundleVersion> ToList(string text)
		{
            string trimedText = text.Trim('\0');
            string[] rows = trimedText.Split(SEPALATE_CAMMA);
			List<AssetBundleVersion> assetBundleVersions = new List<AssetBundleVersion>();
			foreach (var row in rows)
            {
                string[] values = row.Split(SEPALATE_COLON);
                AssetBundleVersion assetBundleVersion = new AssetBundleVersion();
				assetBundleVersion.assetBundleName = values[0];
				assetBundleVersion.length = long.Parse(values[1]);
				assetBundleVersions.Add(assetBundleVersion);
			}
			return assetBundleVersions;
		}
	}
}