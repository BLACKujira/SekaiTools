using SekaiTools.SekaiViewerInterface;
using SekaiTools.SekaiViewerInterface.Utils;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class MasterRefUpdateItem : MonoBehaviour
    {
        public string masterName;
        public ServerRegion serverRegion = ServerRegion.jp;
        public Text nameText;
        public Text lastUpdateTimeText;
        public Text infoText;
        public Button updateButton;

        public event Action OnTableUpdated;

        public string savePath => Path.Combine(EnvPath.Sekai_master_db_diff[serverRegion], masterName + ".json");
        public string url => $"{Url.MasterUrl[SekaiViewer.masterSever, serverRegion]}/{masterName}.json";

        private void Awake()
        {
            infoText.text = string.Empty;
            if (!string.IsNullOrEmpty(masterName))
                Refresh();
        }

        public void Refresh()
        {
            string masterName = this.masterName;
            if(serverRegion!=ServerRegion.jp)
            {
                switch (serverRegion)
                {
                    case ServerRegion.jp:
                        break;
                    case ServerRegion.tw:
                        masterName += "（繁中）";
                        break;
                    case ServerRegion.cn:
                        masterName += "（简中）";
                        break;
                    case ServerRegion.en:
                        masterName += "（英文）";
                        break;
                    case ServerRegion.kr:
                        masterName += "（韩文）";
                        break;
                    default:
                        break;
                }
            }
            nameText.text = masterName;
            lastUpdateTimeText.text =
                File.Exists(savePath) ?
                "上次更新 " + File.GetLastAccessTime(savePath).ToString() :
                "数据表不存在，请更新数据表";
        }

        public void UpdateMaster()
        {
            StartCoroutine(IUpdateMaster());
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(tempFilePath)
                && File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        string tempFilePath = null;
        IEnumerator IUpdateMaster()
        {
            updateButton.interactable = false;

            if (string.IsNullOrEmpty(tempFilePath)
                && File.Exists(tempFilePath))
                File.Delete(tempFilePath);
            tempFilePath = Path.GetTempFileName();
            using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
            {
                getRequest.downloadHandler = new DownloadHandlerFile(tempFilePath, false);
                getRequest.SendWebRequest();
                while (!getRequest.isDone)
                {
                    yield return null;
                }
                if (getRequest.error != null)
                {
                    infoText.text = getRequest.error;
                }
                else
                {
                    if (File.Exists(savePath))
                        File.Delete(savePath);
                    string path = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    File.Copy(tempFilePath, savePath);
                    infoText.text = "更新完成";
                    OnTableUpdated?.Invoke();
                }
            }

            Refresh();
            updateButton.interactable = true;
        }
    }
}