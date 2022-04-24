using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Cutin;
using System.Windows.Forms;
using System.IO;
using UnityEngine.UI;
using System;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.CutinScenePlayerInitialize
{
    /// <summary>
    /// 互动语音播放器/编辑器的初始化窗口
    /// </summary>
    public class CutinScenePlayerInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public CutinScenePlayerInitialize_ModelArea modelArea;
        public CutinScenePlayerInitialize_AudioArea audioArea;
        public InputField inputFieldSaveData;
        public Button applyButton;
        [Header("Prefab")]
        public Window openWindow;

        [NonSerialized] public CutinSceneData cutinSceneData = null;

        OpenFileDialog openFileDialog;
        FolderBrowserDialog folderBrowserDialog;
        SaveFileDialog saveFileDialog;

        private void Awake()
        {
            openFileDialog = FileDialogFactory.GetOpenFileDialog_CutinSceneData();

            saveFileDialog = FileDialogFactory.GetSaveFileDialog_CutinSceneData();

            folderBrowserDialog = new FolderBrowserDialog();

            Refresh();
        }

        public void Refresh()
        {
            modelArea.Initialize(cutinSceneData?.CountAppearCharacters());
            audioArea.Initialize(cutinSceneData);
            inputFieldSaveData.text = cutinSceneData == null? "请读取或创建存档" : cutinSceneData.savePath;

            if (cutinSceneData != null) audioArea.UnlockButtons();
            else audioArea.LockButtons();
        }

        public void CheckIfGetReady()
        {
            if (cutinSceneData != null && modelArea.IfGetReady()) applyButton.interactable = true;
            else applyButton.interactable = false;
        }

        private void Update()
        {
            CheckIfGetReady();
        }

        public void NewData()
        {
            DialogResult dialogResult1 = folderBrowserDialog.ShowDialog();
            if (dialogResult1 != DialogResult.OK) return;
            string audioPath = folderBrowserDialog.SelectedPath;

            DialogResult dialogResult2 = saveFileDialog.ShowDialog();
            if (dialogResult2 != DialogResult.OK) return;
            string savePath = saveFileDialog.FileName;

            string audioDataSavePath = Path.ChangeExtension(savePath, ".aud");

            string[] files = Directory.GetFiles(audioPath);
            List<CutinSceneData.CutinSceneInfo> cutinSceneInfos = new List<CutinSceneData.CutinSceneInfo>();
            foreach (var file in files)
            {
                CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(Path.GetFileName(file));
                if (cutinSceneInfo != null) cutinSceneInfos.Add(cutinSceneInfo);
            }
            cutinSceneData = new CutinSceneData(cutinSceneInfos.ToArray());
            cutinSceneData.savePath = savePath;
            cutinSceneData.SaveData();

            audioArea.NewData(() => 
            {
                cutinSceneData.StandardizeAudioData(audioArea.audioData);
                Refresh();
            }, audioPath, audioDataSavePath);
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;

            CutinSceneData cutinSceneData = JsonUtility.FromJson<CutinSceneData>(File.ReadAllText(fileName));
            this.cutinSceneData = cutinSceneData;
            cutinSceneData.savePath = fileName;
            audioArea.audioData = null;

            string audioDatafileName = Path.ChangeExtension(fileName, ".aud");

            if (File.Exists(audioDatafileName))
            {
                audioArea.LoadData(()=> { Refresh(); },audioDatafileName);
            }
            else
            {
                Refresh();
            }
        }
    
        public void Apply()
        {
            if(openWindow.controlScript is CutinScenePlayer.CutinScenePlayer)
            {
                CutinScenePlayer.CutinScenePlayer.CutinScenePlayerSettings cutinScenePlayerSettings = new CutinScenePlayer.CutinScenePlayer.CutinScenePlayerSettings();
                cutinScenePlayerSettings.audioData = audioArea.audioData;
                cutinScenePlayerSettings.cutinSceneData = cutinSceneData;
                cutinScenePlayerSettings.sekaiLive2DModels = modelArea.sekaiLive2DModels;

                CutinScenePlayer.CutinScenePlayer cutinScenePlayer = window.OpenWindow<CutinScenePlayer.CutinScenePlayer>(openWindow);
                cutinScenePlayer.Initialize(cutinScenePlayerSettings);
            }
            else if(openWindow.controlScript is CutinSceneEditor.CutinSceneEditor)
            {
                CutinSceneEditor.CutinSceneEditor.CutinSceneEditorSettings cutinSceneEditorSettings = new CutinSceneEditor.CutinSceneEditor.CutinSceneEditorSettings();
                cutinSceneEditorSettings.audioData = audioArea.audioData;
                cutinSceneEditorSettings.cutinSceneData = cutinSceneData;
                cutinSceneEditorSettings.sekaiLive2DModels = modelArea.sekaiLive2DModels;

                CutinSceneEditor.CutinSceneEditor cutinSceneEditor = window.OpenWindow<CutinSceneEditor.CutinSceneEditor>(openWindow);
                cutinSceneEditor.Initialize(cutinSceneEditorSettings);
            }
        }
    }
}