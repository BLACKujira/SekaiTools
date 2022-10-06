using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Kizuna;
using SekaiTools.Cutin;
using Button = UnityEngine.UI.Button;
using System.IO;

namespace SekaiTools.UI.KizunaScenePlayerInitialize
{
    public class KizunaScenePlayerInitialize_Audio : MonoBehaviour
    {
        public KizunaScenePlayerInitialize kizunaScenePlayerInitialize;
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

            saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_AUD);

            openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_AUD);
        }

        public void Initialize(KizunaSceneDataBase kizunaSceneData)
        {
            audioDataPath.text = audioData?.savePath ?? "无音频资料";
            if (kizunaSceneData != null && audioData != null)
            {
                CutinSceneData.AudioMatchingCount audioMatchingCount = kizunaSceneData.CountMatching(audioData);
                textMatching.text = $"{audioMatchingCount.matching}/{audioMatchingCount.countAll} 匹配";
                textMissingFirst.text = $"{audioMatchingCount.missingFirst}/{audioMatchingCount.countAll} 缺失前段";
                textMissingSecond.text = $"{audioMatchingCount.missingSecond}/{audioMatchingCount.countAll} 缺失后段";
                textMissingBoth.text = $"{audioMatchingCount.missingBoth}/{audioMatchingCount.countAll} 完全缺失";
            }
            else
            {
                textMatching.text = $"-/- 匹配";
                textMissingFirst.text = $"-/- 缺失前段";
                textMissingSecond.text = $"-/- 缺失后段";
                textMissingBoth.text = $"-/- 完全缺失";
            }
            if (kizunaScenePlayerInitialize.kizunaSceneData == null) LockButtons();
            else UnlockButtons();
        }

        public void LoadData()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;
            LoadData(null, fileName);
        }
        public void LoadData(Action onFinish, string audioPath)
        {
            NowLoadingTypeA nowLoadingTypeA = kizunaScenePlayerInitialize.window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.TitleText = "正在读取音频";

            AudioData newAudioData = new AudioData(audioPath);
            audioData = newAudioData;
            nowLoadingTypeA.StartProcess(newAudioData.LoadData(File.ReadAllText(audioPath)));
            nowLoadingTypeA.OnFinish += () => { Initialize(kizunaScenePlayerInitialize.kizunaSceneData); if (onFinish != null) onFinish(); };
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
            NowLoadingTypeA nowLoadingTypeA = kizunaScenePlayerInitialize.window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.TitleText = "正在读取音频";

            AudioData newAudioData = new AudioData(savePath);
            audioData = newAudioData;
            List<string> selectedFiles = new List<string>();
            string[] files = Directory.GetFiles(audioPath);
            foreach (var file in files)
            {
                CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(Path.GetFileName(file));
                if (cutinSceneInfo != null)
                {
                    foreach (var kizunaScene in kizunaScenePlayerInitialize.kizunaSceneData.kizunaSceneBaseArray)
                    {
                        if(cutinSceneInfo.IsConversationOf(kizunaScene.charAID,kizunaScene.charBID))
                        {
                            selectedFiles.Add(file);
                            break;
                        }
                    }
                }
            }
            nowLoadingTypeA.StartProcess(newAudioData.LoadFile(selectedFiles.ToArray()));
            nowLoadingTypeA.OnFinish += () =>
            {
                Initialize(kizunaScenePlayerInitialize.kizunaSceneData);
                kizunaScenePlayerInitialize.kizunaSceneData.StandardizeAudioData(audioData);
                newAudioData.SaveData();
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