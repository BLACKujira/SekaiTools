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
        [NonSerialized] public string cutinSceneDataPath = null;

        OpenFileDialog openFileDialog;
        FolderBrowserDialog folderBrowserDialog;
        SaveFileDialog saveFileDialog;

        private void Awake()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择互动场景存档";
            openFileDialog.Filter = "JSON (*.json)|*.json|Others (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存场景资料和语音资料";
            saveFileDialog.Filter = "JSON (*.json)|*.json|Others (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            folderBrowserDialog = new FolderBrowserDialog();

            Refresh();
        }

        public void Refresh()
        {
            modelArea.Initialize(cutinSceneData?.CountAppearCharacters());
            audioArea.Initialize(cutinSceneData);
            inputFieldSaveData.text = string.IsNullOrEmpty(cutinSceneDataPath) ? "请读取或创建存档" : cutinSceneDataPath;

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

            string audioDataSavePath = Path.Combine( 
                Path.GetDirectoryName(savePath) , 
                Path.GetFileNameWithoutExtension(savePath) + 
                ".AudioData" + 
                Path.GetExtension(savePath));

            audioArea.NewData(() => 
            {
                cutinSceneDataPath = savePath;
                cutinSceneData = new CutinSceneData(audioArea.audioData);
                string json = JsonUtility.ToJson(cutinSceneData,true);
                File.WriteAllText(savePath, json);
                Refresh();
            },audioPath,audioDataSavePath);
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;
            cutinSceneDataPath = fileName;

            CutinSceneData cutinSceneData = JsonUtility.FromJson<CutinSceneData>(File.ReadAllText(fileName));
            this.cutinSceneData = cutinSceneData;
            audioArea.audioData = null;

            string audioDatafileName = Path.Combine(
                Path.GetDirectoryName(fileName),
                Path.GetFileNameWithoutExtension(fileName) +
                ".AudioData" +
                Path.GetExtension(fileName));

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
                cutinSceneEditorSettings.cutinSceneDataPath = cutinSceneDataPath;

                CutinSceneEditor.CutinSceneEditor cutinSceneEditor = window.OpenWindow<CutinSceneEditor.CutinSceneEditor>(openWindow);
                cutinSceneEditor.Initialize(cutinSceneEditorSettings);
            }
        }
    }
}