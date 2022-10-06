using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioCMDInput_ItemBilibiliUtilities : GIP_RadioCMDInput_ItemBase
    {
        public InputField inputField_roomId;
        public InputField inputField_AutoReconnectTime;
        public InputField inputField_ConnectRetryWaittime;

        public override RadioCommandinputSettingsBase Settings
        {
            get
            {
                RadioCommandinput_BilibiliUtilities.Settings settings = new RadioCommandinput_BilibiliUtilities.Settings();
                settings.roomId = int.Parse(inputField_roomId.text);
                settings.autoReconnectTime = float.Parse(inputField_AutoReconnectTime.text) * 60;

                settings.instanceSettings = new RadioCommandinput_BilibiliUtilities_Instance.Settings();
                settings.instanceSettings.connectRetryWaittime = float.Parse(inputField_ConnectRetryWaittime.text);
                
                return settings;
            }
        }
    }
}