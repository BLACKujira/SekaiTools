using SekaiTools.SystemLive2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.SysL2DShowPlayer
{
    public class SysL2DShowPlayer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public SysL2DShowPlayer_Player player;
        [Header("Settings")]
        public float waitTimeMin = 3;
        public float voiceWaitTime = .5f;
        public float fadeWaitTime = .3f;

        SysL2DShowData sysL2DShowData;
        public SysL2DShowData SysL2DShowData => sysL2DShowData;
        AudioData AudioData => player.AudioData;

        bool ifPlaying = false;

        public class Settings
        {
            public SysL2DShowData sysL2DShowData;
            public SysL2DShowPlayer_Player.Settings playerSettings = new SysL2DShowPlayer_Player.Settings();
        }

        public void Initialize(Settings settings)
        {
            window.OnClose.AddListener(() =>
            {
                foreach (var sekaiLive2DModel in player.l2DControllerTypeB.live2DModels)
                {
                    if (sekaiLive2DModel)
                        Destroy(sekaiLive2DModel.gameObject);
                }
            });

            sysL2DShowData = settings.sysL2DShowData;
            player.Initialize(settings.playerSettings);
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(CoPlay());
        }

        IEnumerator CoPlay()
        {
            for (int i = 0; i < sysL2DShowData.sysL2DShows.Count; i++)
            {
                SysL2DShow currentShow = sysL2DShowData.sysL2DShows[i];
                SysL2DShow nextShow = i == sysL2DShowData.sysL2DShows.Count - 1 ?
                    null : sysL2DShowData.sysL2DShows[i + 1];

                AudioClip audioClip = AudioData.GetValue(currentShow.AudioKey);
                float waitTime = audioClip ? Mathf.Max(waitTimeMin, audioClip.length + voiceWaitTime) : waitTimeMin;
                player.SetData(currentShow);
                if (string.IsNullOrEmpty(currentShow.translationText))
                {
                    player.Play(true);
                }
                else
                {
                    player.Play(false);
                }
                yield return new WaitForSeconds(waitTime);
                if (nextShow != null && currentShow.systemLive2D.CharacterId != nextShow.systemLive2D.CharacterId)
                {
                    player.FadeOutModel();
                }
                player.FadeOutText();
                yield return new WaitForSeconds(fadeWaitTime);
            }
        }

        private void Update()
        {
            if (!ifPlaying && Input.GetKeyDown(KeyCode.Space))
            {
                Play();
                ifPlaying = true;
            }
        }
    }
}