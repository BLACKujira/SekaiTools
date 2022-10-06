using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_AudioData : MonoBehaviour
    {
        [Header("Components")]
        public InputField pathInputField;
        public Text textInfo;
        [Header("Settings")]
        public string defaultFileName = "AudioData.aud";

        FolderBrowserDialog folderBrowserDialog;
        OpenFileDialog openFileDialog;

        [System.NonSerialized] public SerializedAudioData serializedAudioData;

        private void Awake()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_AUD);
        }

        public void NewData()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string selectedPath = folderBrowserDialog.SelectedPath;
            string[] files = Directory.GetFiles(selectedPath);

            List<string> selectedFiles = new List<string>();

            foreach (var file in files)
            {
                selectedFiles.Add(file);
            }

            serializedAudioData = new SerializedAudioData(selectedFiles.ToArray());
            pathInputField.text = Path.Combine(selectedPath,defaultFileName);
            Refresh();
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;

            Load(fileName);
        }

        public void Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            SerializedAudioData serializedAudioData = JsonUtility.FromJson<SerializedAudioData>(json);
            this.serializedAudioData = serializedAudioData;
            pathInputField.text = fileName;
            Refresh();
        }

        public void Refresh()
        {
            if(textInfo)
            {
                int countAll = 0;
                int countAvailable = 0;
                int countLost = 0;
                foreach (var item in serializedAudioData.items)
                {
                    countAll++;
                    if (File.Exists(item.path))
                        countAvailable++;
                    else
                        countLost++;
                }
                textInfo.text = $"共{countAll}段音频，{countAvailable}段可用，{countLost}段缺失";
            }
        }

        public void OpenInfoWindow()
        {
            //TODO 详细信息窗口
        }
    }
}