using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SVDownloaders
{
    public class GIP_SVCard : MonoBehaviour, IGenericInitializationPart
    {
        public Toggle[] toggleFormats = new Toggle[2];
        public FolderSelectItem folderSelectItem;
        string[] formats = { ".png", ".webp" };

        public string format
        {
            get
            {
                for (int i = 0; i < toggleFormats.Length; i++)
                {
                    if (toggleFormats[i].isOn)
                        return formats[i];
                }
                return formats[0];
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(folderSelectItem.SelectedPath))
                errors.Add("无效的保存目录");
            return GenericInitializationCheck.GetErrorString("保存设置错误", errors);
        }

        public void Initialize()
        {
            folderSelectItem.defaultPath = $"{EnvPath.assets}/character/member";
            folderSelectItem.ResetPath();
        }
    }
}