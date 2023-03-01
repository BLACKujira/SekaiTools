using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NCSPlayer;
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCSPlayerInitialize
{
    public class NCSPlayerInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public InputField pathInputField;
        public Text textInfo;
        public GIP_NicknameCountData ncdArea;
        public GIP_CharLive2dPair l2dArea;
        public GIP_AudioData audioArea;
        [Header("Prefab")]
        public Window playerWindowPrefab;

        Count.Showcase.NicknameCountShowcase nicknameCountShowcase;

        public void LoadData()
        {
            OpenFileDialog openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_NCS);
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = openFileDialog.FileName;
            nicknameCountShowcase = Count.Showcase.NicknameCountShowcase.LoadData(fileName,false);
            string audioDataPath = Path.ChangeExtension(fileName, ".aud");
            if (File.Exists(audioDataPath)) throw new NotImplementedException();
                //audioArea.Load(audioDataPath);
            l2dArea.Initialize(nicknameCountShowcase.charactersRequireL2d);
            ncdArea.Load(Path.GetDirectoryName(audioDataPath));

            pathInputField.text = nicknameCountShowcase.SavePath;
            textInfo.text = $"包含{nicknameCountShowcase.scenes.Count}个场景";
        }

        public void Apply()
        {
            NCSPlayer_Player.Settings settings = new NCSPlayer_Player.Settings();
            settings.showcase = nicknameCountShowcase;
            settings.countData = ncdArea.NicknameCountData;
            settings.live2DModels = l2dArea.sekaiLive2DModels;
            AudioData audioData = new AudioData();
            settings.audioData = audioData;
            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(WindowController.windowController.nowLoadingTypeAWindow);
            nowLoadingTypeA.OnFinish += () =>
            {
                NCSPlayer.NCSPlayer nCSPlayer = window.OpenWindow<NCSPlayer.NCSPlayer>(playerWindowPrefab);
                nCSPlayer.Initialize(settings);
            };
            
            nowLoadingTypeA.StartProcess(audioData.LoadFile(audioArea.SerializedAudioData));
        }
    }
}