using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using SekaiTools.Count;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class NicknameCounterInitialize_SampleArea : MonoBehaviour
    {
        public NicknameCounterInitialize_Old nicknameCounterInitialize;
        [Header("Components")]
        public InputField inputFieldFolderPath;
        public Text textUnitStories;
        public Text textEventStories;
        public Text textCardStories;
        public Text textMapTalk;
        public Text textLiveTalk;
        public Text textOtherStories;

        [System.NonSerialized] public string path;

        public const string unitStoriesFolder = NicknameCountData.unitStoriesFolder;
        public const string eventStoriesFolder = NicknameCountData.eventStoriesFolder;
        public const string cardStoriesFolder = NicknameCountData.cardStoriesFolder;
        public const string mapTalkFolder = NicknameCountData.mapTalkFolder;
        public const string liveTalkFolder = NicknameCountData.liveTalkFolder;
        public const string otherStoriesFolder = NicknameCountData.otherStoriesFolder;

        public string[] unitStoryFiles => GetFile(unitStoriesFolder);
        public string[] eventStoryFiles => GetFile(eventStoriesFolder);
        public string[] cardStoryFiles => GetFile(cardStoriesFolder);
        public string[] mapTalkFiles => GetFile(mapTalkFolder);
        public string[] liveTalkFiles => GetFile(liveTalkFolder);
        public string[] otherStoriesFiles => GetFile(otherStoriesFolder);

        private string[] GetFile(string folder)
        {
            if (string.IsNullOrEmpty(this.path)) return new string[0];
            string path = Path.Combine(this.path, folder);
            string[] files = Directory.GetFiles(path);
            return files;
        }

        void GetCount()
        {
            string _unitStoriesFolder = Path.Combine(path, unitStoriesFolder);
            string _eventStoriesFolder = Path.Combine(path, eventStoriesFolder);
            string _cardStoriesFolder = Path.Combine(path, cardStoriesFolder);
            string _mapTalkFolder = Path.Combine(path, mapTalkFolder);
            string _liveTalkFolder = Path.Combine(path, liveTalkFolder);
            string _otherStoriesFolder = Path.Combine(path, otherStoriesFolder);

            textUnitStories.text = $"{(Directory.Exists(_unitStoriesFolder)?Directory.GetFiles(_unitStoriesFolder).Length:0)} 组合剧情"; 
            textEventStories.text = $"{(Directory.Exists(_eventStoriesFolder)?Directory.GetFiles(_eventStoriesFolder).Length:0)} 活动剧情"; 
            textCardStories.text = $"{(Directory.Exists(_cardStoriesFolder)?Directory.GetFiles(_cardStoriesFolder).Length:0)} 卡片剧情"; 
            textMapTalk.text = $"{(Directory.Exists(_mapTalkFolder)?Directory.GetFiles(_mapTalkFolder).Length:0)} 区域对话"; 
            textLiveTalk.text = $"{(Directory.Exists(_liveTalkFolder)?Directory.GetFiles(_liveTalkFolder).Length:0)} Live对话"; 
            textOtherStories.text = $"{(Directory.Exists(_otherStoriesFolder)?Directory.GetFiles(_otherStoriesFolder).Length:0)} 其他剧情"; 

        }

        public void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择存放样本的文件夹";
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult != DialogResult.OK) return;

            path = folderBrowserDialog.SelectedPath;
            inputFieldFolderPath.text = path;

            GetCount();
        }
    }
}