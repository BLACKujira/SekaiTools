using BilibiliUtilities.Live;
using BilibiliUtilities.Live.Lib;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class RadioCommandinput_BilibiliUtilities_Instance : MonoBehaviour
    {
        Radio radio;
        int roomId;
        IMessageHandler messageHandler;
        float connectRetryWaittime = 5;

        bool _isReconnecting = true;
        public bool isReconnecting => _isReconnecting;
        float _lastReconnectTime = 0;
        public float lastReconnectTime => _lastReconnectTime;


        LiveRoom currentLiveRoom;
        public LiveRoom liveRoom => currentLiveRoom;

        class ReConnectObject : CustomYieldInstruction
        {
            bool _keepWaiting = true;
            public override bool keepWaiting => _keepWaiting;
            public LiveRoom liveRoom = null;
            public bool Success => liveRoom != null;

            public ReConnectObject(int roomId, IMessageHandler messageHandler)
            {
                ReConnect(roomId, messageHandler);
            }

            async void ReConnect(int roomId,IMessageHandler messageHandler)
            {
                LiveRoom liveRoom = new LiveRoom(roomId, messageHandler);
                if (await liveRoom.ConnectAsync())
                {
                    this.liveRoom = liveRoom;
                    Debug.Log("连接成功");
                }
                _keepWaiting = false;
            }
        }

        public IEnumerator ReConnect()
        {
            _isReconnecting = true;
            while (true)
            {
                ReConnectObject reConnectObject = new ReConnectObject(roomId,messageHandler);
                yield return reConnectObject;
                if(reConnectObject.Success)
                {
                    if (currentLiveRoom != null) currentLiveRoom.Disconnect();
                    currentLiveRoom = reConnectObject.liveRoom;
                    reConnectObject.liveRoom.ReadMessageLoop();
                    break;
                }
                radio.messageLayer.AddMessage("系统", MessageType.system, "直播间连接失败，尝试重新连接");
                yield return new WaitForSeconds(connectRetryWaittime);
            }
            _isReconnecting = false;
            _lastReconnectTime = Time.time;
        }

        public void Initialize(Radio radio, int roomId,IMessageHandler messageHandler,Settings settings)
        {
            this.radio = radio;
            this.roomId = roomId;
            this.messageHandler = messageHandler;
            connectRetryWaittime = settings.connectRetryWaittime;
        }

        public class Settings
        {
            public float connectRetryWaittime;
        }
    }
}