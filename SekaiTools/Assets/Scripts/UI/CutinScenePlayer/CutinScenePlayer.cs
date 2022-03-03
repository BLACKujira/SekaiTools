using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI;
using SekaiTools.Live2D;
using SekaiTools;
using SekaiTools.Cutin;

namespace SekaiTools.UI.CutinScenePlayer
{
    /// <summary>
    /// 互动语音场景播放器
    /// </summary>
    public class CutinScenePlayer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public CutinScenePlayer_Player player;

        bool ifStartPlaying = false;

        private void Update()
        {
            if(!ifStartPlaying&&Input.anyKeyDown)
            {
                player.Play();
                ifStartPlaying = true;
            }
        }

        /// <summary>
        /// 传入一个设置对象以初始化
        /// </summary>
        /// <param name="settings"></param>
        public void Initialize(CutinScenePlayerSettings settings)
        {
            player.audioData = settings.audioData;
            player.cutinSceneData = settings.cutinSceneData;
            player.l2DController.live2DModels = settings.sekaiLive2DModels;
            player.l2DController.ResetAllModels();
        }

        /// <summary>
        /// 用于初始化
        /// </summary>
        public class CutinScenePlayerSettings
        {
            public AudioData audioData;
            public CutinSceneData cutinSceneData;
            public SekaiLive2DModel[] sekaiLive2DModels;
        }
    }
}