using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_NicknameCountData : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public InputField pathInputField;
        public Text textInfo;

        NicknameCountData nicknameCountData;
        public NicknameCountData NicknameCountData => nicknameCountData;

        public void LoadData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string selectedPath = folderBrowserDialog.SelectedPath;
            Load(selectedPath);
        }

        private void Awake()
        {
            Refresh();
        }

        public void Load(string selectedPath)
        {
            nicknameCountData = NicknameCountData.Load(selectedPath);
            Refresh();
        }

        public void Refresh()
        {
            if(nicknameCountData == null) 
            { 
                textInfo.text = "请选择文件";
                return;
            }

            pathInputField.text = nicknameCountData.SavePath;
            try
            {
                textInfo.text = $@"包含以下统计数据
{nicknameCountData.countMatrix_Unit.Count} 主线剧情，{nicknameCountData.countMatrix_Event.Count} 活动剧情，{nicknameCountData.countMatrix_Card.Count} 卡面剧情
{nicknameCountData.countMatrix_Map.Count} 地图对话，{nicknameCountData.countMatrix_Live.Count} Live对话，{nicknameCountData.countMatrix_Other.Count} 其他剧情";
            }
            catch
            {
                nicknameCountData = null;
                textInfo.text = "文件可能损坏";
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if(nicknameCountData == null ) 
            {
                errors.Add("没有加载统计数据或文件损坏");
            }
            return GenericInitializationCheck.GetErrorString("统计数据错误", errors);
        }
    }
}