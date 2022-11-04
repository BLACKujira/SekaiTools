using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SekaiTools.SekaiViewerInterface.Utils;

namespace SekaiTools.SekaiViewerInterface.Pages
{
    public class AssetViewer
    {
        public static string GetFilePathURL(string path)
        {
            if (!path.EndsWith("/")) path += '/';
            return $"{Env.VITE_ASSET_DOMAIN_MINIO}/sekai-assets/?list-type=2&delimiter=%2F&prefix={path.Replace("/", "%2F")}&max-keys=500";
        }


        /// <summary>
        /// 获取资产浏览器一个目录的信息，需要MonoBehaviour启动协程
        /// </summary>
        /// <returns></returns>
        public static IEnumerator GetFilePath(string path, Action<ListBucketResult> onCompleted, Action<string> onError)
        {
            using (UnityWebRequest getRequest = UnityWebRequest.Get(GetFilePathURL(path)))
            {
                using (getRequest.downloadHandler = new DownloadHandlerBuffer())
                {
                    getRequest.SendWebRequest();
                    while (!getRequest.isDone)
                    {
                        yield return 1;
                    }
                    if (getRequest.error != null)
                    {
                        onError(getRequest.error);
                    }
                    File.WriteAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\2\1.xml", getRequest.downloadHandler.text);

                    try
                    {
                        ListBucketResult filePath = ListBucketResult.Deserialize(getRequest.downloadHandler.data);
                        onCompleted(filePath);
                    }
                    catch (System.Exception ex)
                    {
                        onError(ex.Message);
                    }
                }
            }
        }
    }
}