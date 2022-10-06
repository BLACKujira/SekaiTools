using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class RadioCommandinput_BilibiliUtilitiesMuti : RadioCommandinput_BilibiliUtilitiesMutiBase
    {
        float checkTime = 5 * 60;
        float initialTimeDifference = 5;

        public override void Initialize(Radio radio, RadioCommandinputSettingsBase settings)
        {
            Settings config = settings as Settings;
            if (config == null) throw new RadioCommandinputSettingsBase.IncorrectSettingTypeException();

            this.radio = radio;
            this.survivalTimeThreshold = config.survivalTimeThreshold;
            this.autoReconnectTime = config.autoReconnectTime;
            this.checkTime = config.checkTime;
            this.initialTimeDifference = config.initialTimeDifference;

            instances = new RadioCommandinput_BilibiliUtilities_Instance[config.numberOfInstance];
            sourceList = new List<int>();
            for (int i = 0; i < instances.Length; i++)
            {
                RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler messageHandler =
                    new RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler(this, i);
                instances[i] = Instantiate(instancePrefab,transform);
                instances[i].Initialize(radio, config.roomId ,messageHandler, config.instanceSettings);

                sourceList.Add(i);
            }
            StartCoroutine(Process());
        }

        IEnumerator Process()
        {
            foreach (var instance in instances)
            {
                yield return instance.ReConnect();
                yield return new WaitForSeconds(initialTimeDifference);
            }
            radio.messageLayer.AddMessage("系统", MessageType.system, "连接成功");
            ready = true;

            while (true)
            {
                yield return new WaitForSeconds(checkTime);
                yield return CheckAndReconnect();
            }
        }

        IEnumerator CheckAndReconnect()
        {
            HashSet<int> reconnectedSources = new HashSet<int>();
            List<Danmaku> removeDanmaku = new List<Danmaku>();

            for (int i = 0; i < instances.Length; i++)
            {
                RadioCommandinput_BilibiliUtilities_Instance instance = instances[i];
                if(Time.time-instance.lastReconnectTime>autoReconnectTime)
                {
                    reconnectedSources.Add(i);
                    Debug.Log($"{i}号接收器周期性重连");
                    yield return instance.ReConnect();
                }
            }
            
            foreach (var keyValuePair in danamkuDictionary)
            {
                if (keyValuePair.Value.allSourcesReceived)
                    removeDanmaku.Add(keyValuePair.Key);
                else if(keyValuePair.Value.survivalTime>survivalTimeThreshold)
                {
                    removeDanmaku.Add(keyValuePair.Key);
                    foreach (var source in keyValuePair.Value.sourcesNotReceived)
                    {
                        if(!reconnectedSources.Contains(source))
                        {
                            reconnectedSources.Add(source);
                            Debug.Log($"{source}号接收器没有收到弹幕{keyValuePair.Key.userName} {keyValuePair.Key.content}");
                            yield return instances[source].ReConnect();
                        }
                    }
                }
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
            public float checkTime;
            public float initialTimeDifference;
            public int numberOfInstance;
            public RadioCommandinput_BilibiliUtilities_Instance.Settings instanceSettings;
        }
    }
}