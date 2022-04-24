using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Cutin;
using SekaiTools.Live2D;
using SekaiTools.UI.CutinScenePlayer;
using System;
using static SekaiTools.UI.CutinScenePlayer.CutinScenePlayer;
using System.IO;

namespace SekaiTools.UI.CutinSceneEditor
{
    public class CutinSceneEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public CutinScenePlayer_Player player;
        public CutinSceneEditor_EditArea editArea;
        public CutinSceneEditor_Scroll scroll;
        public AudioSource audioPlayer;
        public MessageLayer.MessageLayerBase messageLayer;
        [Header("Prefabs")]
        public Window playerWindow;

        [NonSerialized] public AudioData audioData;
        [NonSerialized] public CutinSceneData cutinSceneData;
        [NonSerialized] public CutinScene currentCutinScene;

        public void Initialize(CutinSceneEditorSettings settings)
        {
            audioData = settings.audioData;
            player.audioData = settings.audioData;
            cutinSceneData = settings.cutinSceneData;
            player.l2DController.live2DModels = settings.sekaiLive2DModels;

            scroll.Generate(cutinSceneData, (CutinScene cutinScene) =>
            { editArea.SetScene(cutinScene); currentCutinScene = cutinScene; });
        }

        public void PlayAudioClip(string audioName)
        {
            AudioClip audioClip = audioData.GetValue(audioName);
            if (audioClip)
                audioPlayer.PlayOneShot(audioClip);
        }

        public void PreviewScene()
        {
            player.cutinSceneData = new CutinSceneData(currentCutinScene);
            player.Play();
        }

        public class CutinSceneEditorSettings : CutinScenePlayerSettings
        {
        }

        public void Save()
        {
            cutinSceneData.SaveData();
            messageLayer.ShowMessage("保存成功");
        }

        public void OpenPlayerWindow()
        {
            CutinScenePlayerSettings cutinScenePlayerSettings = new CutinScenePlayerSettings();
            cutinScenePlayerSettings.audioData = audioData;
            cutinScenePlayerSettings.cutinSceneData = new CutinSceneData(currentCutinScene);
            cutinScenePlayerSettings.sekaiLive2DModels = player.l2DController.live2DModels;
        
            CutinScenePlayer.CutinScenePlayer cutinScenePlayer = window.OpenWindow<CutinScenePlayer.CutinScenePlayer>(playerWindow);
            cutinScenePlayer.Initialize(cutinScenePlayerSettings);
        }
    }
}