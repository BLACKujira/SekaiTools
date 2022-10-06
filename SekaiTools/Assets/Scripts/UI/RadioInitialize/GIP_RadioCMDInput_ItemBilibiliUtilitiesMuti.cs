using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioCMDInput_ItemBilibiliUtilitiesMuti : GIP_RadioCMDInput_ItemBase
    {
        public InputField inputField_roomId;
        public InputField inputField_AutoReconnectTime;
        public InputField inputField_ConnectRetryWaittime;
        public InputField inputField_SurvivalTimeThreshold;
        public InputField inputField_CheckTime;
        public InputField inputField_InitialTimeDifference;
        public InputField inputField_NumberOfInstance;

        public override RadioCommandinputSettingsBase Settings
        {
            get
            {
                RadioCommandinput_BilibiliUtilitiesMuti.Settings settings = new RadioCommandinput_BilibiliUtilitiesMuti.Settings();
                settings.roomId = int.Parse(inputField_roomId.text);
                settings.autoReconnectTime = float.Parse(inputField_AutoReconnectTime.text) * 60;
                settings.survivalTimeThreshold = float.Parse(inputField_SurvivalTimeThreshold.text);
                settings.checkTime = float.Parse(inputField_CheckTime.text) * 60;
                settings.initialTimeDifference = float.Parse(inputField_InitialTimeDifference.text);
                settings.numberOfInstance = int.Parse(inputField_NumberOfInstance.text);

                settings.instanceSettings = new RadioCommandinput_BilibiliUtilities_Instance.Settings();
                settings.instanceSettings.connectRetryWaittime = int.Parse(inputField_ConnectRetryWaittime.text);

                return settings;
            }
        }
    }
}