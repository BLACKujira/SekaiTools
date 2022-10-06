using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class RadioCommandinput_BilibiliUtilities : RadioCommandinput_Base
    {
        public RadioCommandinput_BilibiliUtilities_Instance commandInputInstance;
        float autoReconnectTime = 15 * 60;

        public override void Initialize(Radio radio, RadioCommandinputSettingsBase settings)
        {
            Settings config = settings as Settings;
            if (config == null) throw new RadioCommandinputSettingsBase.IncorrectSettingTypeException();

            this.radio = radio;
            autoReconnectTime = config.autoReconnectTime;
            RadioCommandinput_BilibiliUtilities_MessageHandler messageHandler = new RadioCommandinput_BilibiliUtilities_MessageHandler(radio);
            commandInputInstance.Initialize(radio, config.roomId ,messageHandler, config.instanceSettings);
            StartCoroutine(AutoReconnect());
        }

        IEnumerator AutoReconnect()
        {
            yield return commandInputInstance.ReConnect();
            radio.messageLayer.AddMessage("系统", MessageType.system, "连接成功");
            while (true)
            {
                yield return new WaitForSeconds(autoReconnectTime);
                yield return commandInputInstance.ReConnect();
                Debug.Log("直播间已重连");
            }
        }

        public class Settings : RadioCommandinputSettingsBase
        {
            public int roomId;
            public float autoReconnectTime;
            public RadioCommandinput_BilibiliUtilities_Instance.Settings instanceSettings;
        }
    }
}