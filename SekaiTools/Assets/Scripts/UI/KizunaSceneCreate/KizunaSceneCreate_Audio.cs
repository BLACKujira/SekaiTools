using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Cutin;
using System.Windows.Forms;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class KizunaSceneCreate_Audio : MonoBehaviour
    {
        public enum Mode { none,folder,cutinData};

        public KizunaSceneCreate kizunaSceneCreate;

        [Header("Components")]
        public InputField pathInputField;

        [HideInInspector]public Mode mode = Mode.none;

        [System.NonSerialized] public string[] files = new string[0];
        [System.NonSerialized] public CutinSceneData cutinSceneData;

        FolderBrowserDialog folderBrowserDialog;
        OpenFileDialog openFileDialog;

        private void Awake()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_CSD);
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
                if (CutinSceneData.IsCutinVoice(Path.GetFileName(file)) != null)
                {
                    selectedFiles.Add(file);
                }
            }

            this.files = selectedFiles.ToArray();

            mode = Mode.folder;

            pathInputField.text = selectedPath;

            kizunaSceneCreate.Refresh();
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = openFileDialog.FileName;
            try
            {
                cutinSceneData = JsonUtility.FromJson<CutinSceneData>(File.ReadAllText(fileName));
                cutinSceneData.SavePath = fileName;
                mode = Mode.cutinData;
            }
            catch(System.Exception ex)
            {
                kizunaSceneCreate.window.ShowMessageBox("读取失败", ex.GetType().ToString());
            }
        
            kizunaSceneCreate.Refresh();

            pathInputField.text = fileName;
        }

    }
}