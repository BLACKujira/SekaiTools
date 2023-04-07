using SekaiTools.SystemLive2D;
using SekaiTools.UI.Downloader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools
{
    public class _temp1 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SysL2DShowData sysL2DShowData = JsonUtility.FromJson<SysL2DShowData>(
                File.ReadAllText("C:\\Users\\KUROKAWA_KUJIRA\\Desktop\\Save\\af2023.sls"));

            string cookie = File.ReadAllText("C:\\Users\\KUROKAWA_KUJIRA\\Desktop\\1\\cookie.txt");
            
            List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();

            foreach (var sysL2DShow in sysL2DShowData.sysL2DShows)
            {
                string url = "https://production-cf2d2388-assetbundle.sekai.colorfulpalette.org/2.6.0.15/c417bde9-8077-62d9-fe5c-cda21cdf29c6/android/";
                url += "sound/system_live2d/voice";
                url += $"{sysL2DShow.systemLive2D.AssetbundleName}";


                //DownloadFileInfo downloadFileInfo = new DownloadFileInfo();

            }
        }
    }
}