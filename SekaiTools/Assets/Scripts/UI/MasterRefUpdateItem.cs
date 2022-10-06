using SekaiTools.SekaiViewerInterface;
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

        public Text nameText;
        public Text lastUpdateTimeText;
        public Text infoText;
        public Button updateButton;

        public string savePath => Path.Combine(EnvPath.sekai_master_db_diff, masterName + ".json");
        public string url => $"{SekaiViewer.MasterUrl}/{masterName}.json";

        private void Awake()
        {
            infoText.text = string.Empty;
            if (!string.IsNullOrEmpty(masterName))
                Refresh();
        }

        public void Refresh()
        {
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
                    File.Copy(tempFilePath, savePath);
                    infoText.text = "更新完成";
                }
            }

            Refresh();
            updateButton.interactable = true;
        }
    }
}