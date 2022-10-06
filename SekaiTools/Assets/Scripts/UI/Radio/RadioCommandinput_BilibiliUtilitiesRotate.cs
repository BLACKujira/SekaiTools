using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class RadioCommandinput_BilibiliUtilitiesRotate : RadioCommandinput_BilibiliUtilitiesMutiBase
    {
        public override void Initialize(Radio radio, RadioCommandinputSettingsBase settings)
        {
            Settings config = settings as Settings;
            if (config == null) throw new RadioCommandinputSettingsBase.IncorrectSettingTypeException();

            this.radio = radio;
            this.survivalTimeThreshold = config.survivalTimeThreshold;
            this.autoReconnectTime = config.autoReconnectTime;

            instances = new RadioCommandinput_BilibiliUtilities_Instance[config.numberOfInstance];
            sourceList = new List<int>();
            for (int i = 0; i < instances.Length; i++)
            {
                RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler messageHandler =
                    new RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler(this, i);
                instances[i] = Instantiate(instancePrefab, transform);
                instances[i].Initialize(radio, config.roomId, messageHandler, config.instanceSettings);

                sourceList.Add(i);
            }
            StartCoroutine(Process());
        }

        IEnumerator Process()
        {
            foreach (var instance in instances)
            {
                yield return instance.ReConnect();
            }
            radio.messageLayer.AddMessage("系统", MessageType.system, "连接成功");
            ready = true;

            int reconnectInstanceId = 0;
            while (true)
            {
                yield return new WaitForSeconds(autoReconnectTime);
                RemoveTimeoutDanmaku();
                yield return instances[reconnectInstanceId].ReConnect();
                reconnectInstanceId++;
                if (reconnectInstanceId >= instances.Length)
                    reconnectInstanceId = 0;
            }
        }

        public void RemoveTimeoutDanmaku()
        {
            List<Danmaku> removeDanmaku = new List<Danmaku>();
            foreach (var keyValuePair in danamkuDictionary)
            {
                if (keyValuePair.Value.survivalTime > survivalTimeThreshold)
                    removeDanmaku.Add(keyValuePair.Key);
            }
            foreach (var danmaku in removeDanmaku)
            {
                danamkuDictionary.Remove(danmaku);
            }
        }

        public class Settings : RadioCommandinputSettingsBase
        {
            public int roomId;
            public float autoReconnectTime;
            public float survivalTimeThreshold;
            public int numberOfInstance;
            public RadioCommandinput_BilibiliUtilities_Instance.Settings instanceSettings;
        }

    }
}