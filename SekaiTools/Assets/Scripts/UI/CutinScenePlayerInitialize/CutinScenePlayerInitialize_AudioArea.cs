using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Cutin;
using UnityEngine.UI;
using System.IO;
using System.Windows.Forms;
using System;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.CutinScenePlayerInitialize
{
    public class CutinScenePlayerInitialize_AudioArea : MonoBehaviour
    {
        public CutinScenePlayerInitialize cutinScenePlayerInitialize;
        [Header("Components")]
        public InputField audioDataPath;
        public Text textMatching;
        public Text textMissingFirst;
        public Text textMissingSecond;
        public Text textMissingBoth;
        public List<Button> buttons;
        [Header("Prefabs")]
        public Window nowLoadingWindow;

        [NonSerialized] public AudioData audioData = null;

        FolderBrowserDialog folderBrowserDialog;
        SaveFileDialog saveFileDialog;
        OpenFileDialog openFileDialog;

        private void Awake()
        {
            folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择存放语音的文件夹";

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存语音资料";
            saveFileDialog.Filter = "JSON (*.json)|*.json|Others (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择音频存档";
            openFileDialog.Filter = "JSON (*.json)|*.json|Others (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
        }

        public void Initialize(CutinSceneData cutinSceneData)
        {
            audioDataPath.text = audioData?.savePath ?? "无音频资料";
            if (cutinSceneData != null && audioData != null)
            {
                CutinSceneData.AudioMatchingCount audioMatchingCount = cutinSceneData.CountMatching(audioData);
                textMatching.text = $"{audioMatchingCount.matching}/{cutinSceneData.cutinScenes.Count} 匹配";
                textMissingFirst.text = $"{audioMatchingCount.missingFirst}/{cutinSceneData.cutinScenes.Count} 缺失前段";
                textMissingSecond.text = $"{audioMatchingCount.missingSecond}/{cutinSceneData.cutinScenes.Count} 缺失后段";
                textMissingBoth.text = $"{audioMatchingCount.missingBoth}/{cutinSceneData.cutinScenes.Count} 完全缺失";
            }
            else
            {
                textMatching.text = $"-/- 匹配";
                textMissingFirst.text = $"-/- 缺失前段";
                textMissingSecond.text = $"-/- 缺失后段";
                textMissingBoth.text = $"-/- 完全缺失";
            }
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;
            LoadData(null, fileName);
        }
        public void LoadData(Action onFinish,string directory)
        {
            NowLoadingTypeA nowLoadingTypeA = cutinScenePlayerInitialize.window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.TitleText = "正在读取音频";

            AudioData newAudioData = new AudioData();
            audioData = newAudioData;
            nowLoadingTypeA.StartProcess(newAudioData.LoadAudioData(directory));
            nowLoadingTypeA.OnFinish += () => { Initialize(cutinScenePlayerInitialize.cutinSceneData); if (onFinish != null) onFinish(); };
        }

        public void NewData()
        {
            DialogResult dialogResult1 = folderBrowserDialog.ShowDialog();
            if (dialogResult1 != DialogResult.OK) return;
            string audioPath = folderBrowserDialog.SelectedPath;

            DialogResult dialogResult2 = saveFileDialog.ShowDialog();
            if (dialogResult2 != DialogResult.OK) return;
            string savePath = saveFileDialog.FileName;

            NewData(null, audioPath, savePath);
        }
        public void NewData(Action onFinish, string audioPath, string savePath)
        {
            NowLoadingTypeA nowLoadingTypeA = cutinScenePlayerInitialize.window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.TitleText = "正在读取音频";

            AudioData newAudioData = new AudioData();
            audioData = newAudioData;
            audioData.savePath = audioPath;
            nowLoadingTypeA.StartProcess(newAudioData.LoadDatas(Directory.GetFiles(audioPath)));
            nowLoadingTypeA.OnFinish += () =>
            {
                Initialize(cutinScenePlayerInitialize.cutinSceneData);
                newAudioData.SaveData(savePath);
                if (onFinish != null) onFinish();
            };
        }

        public void LockButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
        }
        public void UnlockButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = true;
            }
        }
    }
}