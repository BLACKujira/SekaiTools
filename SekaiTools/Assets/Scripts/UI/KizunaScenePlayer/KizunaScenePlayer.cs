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

        public void Initialize(KizunaSceneEditor.KizunaSceneEditor.Settings settings)
        {
            player.Initialize(settings);
        }

        public void DestroyLive2DModels()
        {
            foreach (var live2DModel in player.l2DController.live2DModels)
            {
                if (live2DModel)
                    Destroy(live2DModel.gameObject);
            }
        }
    }
}