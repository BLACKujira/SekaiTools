using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_NicknameCountData : MonoBehaviour
    {
        [Header("Components")]
        public InputField pathInputField;
        public Text textInfo;

        [System.NonSerialized] public NicknameCountData nicknameCountData;

        public void LoadData()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string selectedPath = folderBrowserDialog.SelectedPath;
            Load(selectedPath);
        }

        public void Load(string selectedPath)
        {
            nicknameCountData = NicknameCountData.Load(selectedPath);
            Refresh();
        }

        public void Refresh()
        {
            pathInputField.text = nicknameCountData.SavePath;
            textInfo.text = $@"包含以下统计数据
{nicknameCountData.countMatrix_Unit.Count} 主线剧情，{nicknameCountData.countMatrix_Event.Count} 活动剧情，{nicknameCountData.countMatrix_Card.Count} 卡面剧情
{nicknameCountData.countMatrix_Map.Count} 地图对话，{nicknameCountData.countMatrix_Live.Count} Live对话，{nicknameCountData.countMatrix_Other.Count} 其他剧情";
        }
    }
}