using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI;
using System.Windows.Forms;
using System;

namespace SekaiTools
{
    public class FileLoader : MonoBehaviour
    {
        [Header("Prefab")]
        public Window _nowLoadingWindowPrefab;

        public static FileLoader fileLoader;
        public static Window window => WindowController.windowController.currentWindow;
        public static Window nowLoadingWindowPrefab => fileLoader._nowLoadingWindowPrefab;

        private void Awake()
        {
            fileLoader = this;
        }

        public static AudioData LoadAudioData(Action<AudioData> onFinish)
        {
            OpenFileDialog openFileDialog = FileDialogFactory.GetOpenFileDialog_Audio();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return null;
            string fileName = openFileDialog.FileName;

            AudioData audioData = new AudioData(fileName);
            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
            nowLoadingTypeA.TitleText = "正在读取音频";
            nowLoadingTypeA.OnFinish += () => onFinish(audioData);
            nowLoadingTypeA.StartProcess(audioData.LoadFile(fileName));
            return audioData;
        }
    }
}