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
        public RectTransform playerTf;
        public CutinScenePlayer_Player player;
        public CutinSceneEditor_EditArea editArea;
        public CutinSceneEditor_Scroll scroll;
        public AudioSource audioPlayer;
        public SaveTipCloseWindowButton saveTipCloseWindowButton;
        public MessageLayer.MessageLayerBase messageLayer;
        [Header("Settings")]
        public CutinScenePlayerSet playerSet;
        [Header("Prefabs")]
        public Window playerWindow;
        public Window gswPrefab;

        [NonSerialized] public AudioData audioData;
        [NonSerialized] public CutinSceneData cutinSceneData;
        [NonSerialized] public CutinScene currentCutinScene;

        Settings settings;
        public Settings EditorSettings => settings;

        public void Initialize(Settings settings)
        {
            this.settings = settings;
            audioData = settings.audioData;
            cutinSceneData = settings.cutinSceneData;

            saveTipCloseWindowButton.Initialize(() => cutinSceneData.SavePath);

            ChangePlayer(settings.cutinSceneData.playerType);

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

        public class Settings : CutinScenePlayer.CutinScenePlayer.Settings
        {

        }

        public void Save()
        {
            cutinSceneData.SaveData();
            messageLayer.ShowMessage("保存成功");
        }

        public void OpenPlayerWindow()
        {
            CutinScenePlayer.CutinScenePlayer.Settings cutinScenePlayerSettings = new CutinScenePlayer.CutinScenePlayer.Settings();
            cutinScenePlayerSettings.audioData = audioData;
            cutinScenePlayerSettings.cutinSceneData = new CutinSceneData(currentCutinScene);
            cutinScenePlayerSettings.sekaiLive2DModels = player.l2DController.live2DModels;
        
            CutinScenePlayer.CutinScenePlayer cutinScenePlayer = window.OpenWindow<CutinScenePlayer.CutinScenePlayer>(playerWindow);
            cutinScenePlayer.Initialize(cutinScenePlayerSettings);
        }

        public void OpenConfWindow()
        {
            GeneralSettingsWindow.GeneralSettingsWindow generalSettingsWindow = window.OpenWindow<GeneralSettingsWindow.GeneralSettingsWindow>(gswPrefab);
            generalSettingsWindow.Initialize(
                new ConfigUIItem[]
                {
                    new ConfigUIItem_CutinScenePlayer
                        ("播放器样式","播放器",
                            ()=>cutinSceneData.playerType,
                            (value) => 
                            { 
                                cutinSceneData.playerType = value;
                                ChangePlayer(value);
                            })
                });
        }

        void ChangePlayer(string key)
        {
            CutinScenePlayerSet_Item cutinScenePlayerSet_Item = playerSet.GetItem(key) ?? playerSet.DefaultItem;
            if (player) Destroy(player);
            player = Instantiate(cutinScenePlayerSet_Item.player, playerTf);
            player.audioData = settings.audioData;
            player.l2DController.live2DModels = settings.sekaiLive2DModels;
        }
    }
}