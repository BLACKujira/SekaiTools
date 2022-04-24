using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Kizuna;
using SekaiTools.Live2D;
using SekaiTools.UI.BackGround;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public KizunaScenePlayer_Player player;
        bool ifStartPlaying = false;

        private void Update()
        {
            if (!ifStartPlaying && Input.GetKeyDown(KeyCode.Return))
            {
                player.Play();
                ifStartPlaying = true;
            }
        }

        public void Initialize(KizunaScenePlayerSettings settings)
        {
            player.audioData = settings.audioData;
            player.imageData = settings.imageData;
            player.kizunaSceneData = settings.kizunaSceneData;
            player.l2DController.live2DModels = settings.sekaiLive2DModels;
            player.l2DController.ResetAllModels();
            player.bGController.Initialize(settings.backGroundParts);
            player.Initialize();
        }

        public class KizunaScenePlayerSettings
        {
            public AudioData audioData;
            public ImageData imageData;
            public KizunaSceneDataBase kizunaSceneData;
            public SekaiLive2DModel[] sekaiLive2DModels;
            public BackGroundPart[] backGroundParts;
        }
    }
}