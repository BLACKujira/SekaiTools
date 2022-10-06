using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.RadioInitialize
{
    public class RadioInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_RadioCMDInput gIP_RadioCMDInput;
        public GIP_RadioMain gIP_RadioMain;
        public GIP_RadioEffect gIP_RadioEffect;
        public GIP_RadioWelcome gIP_RadioWelcome;
        public GIP_RadioMusicPlayer gIP_RadioMusicPlayer;
        public GIP_RadioSerifQuery gIP_RadioSerifQuery;
        public GIP_RadioCardAppreciation gIP_RadioCardAppreciation;
        [Header("Prefabs")]
        public Window radioWindowPrefab;

        private void Awake()
        {
            gIP_RadioMusicPlayer.Initialize();
            gIP_RadioSerifQuery.Initialize();
            gIP_RadioMain.Initialize();
            gIP_RadioCardAppreciation.Initialize();
        }

        public void Apply()
        {
            Radio.Radio.Settings settings = new Radio.Radio.Settings();
            settings.cMDAliases = gIP_RadioMain.cMDAliases;
            settings.radioCMDInputInitializeParts = gIP_RadioCMDInput.radioCMDInputInitializeParts;
            settings.effectControllerSettings = gIP_RadioEffect.Settings;
            settings.welcomeLayerSettings = gIP_RadioWelcome.Settings;
            settings.musicPlayerSettings = gIP_RadioMusicPlayer.settings;
            settings.serifQuerySettings = gIP_RadioSerifQuery.settings;
            settings.cardAppreciationSettings = gIP_RadioCardAppreciation.Settings;

            if(settings.serifQuerySettings.enable)
            {
                settings.welcomeLayerSettings.tips.Add(
                    "输入 \"/对话查询 角色1 角色2\" 查询 角色1 提到 角色2 的对话");
            }

                Radio.Radio radio = window.OpenWindow<Radio.Radio>(radioWindowPrefab);
            radio.Initialize(settings);
        }
    }
}