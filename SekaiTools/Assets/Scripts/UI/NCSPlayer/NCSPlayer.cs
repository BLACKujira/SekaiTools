using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NCSPlayer
{
    public class NCSPlayer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NCSPlayer_Player player;
        bool ifStartPlaying = false;

        private void Update()
        {
            if (!ifStartPlaying && Input.GetKeyDown(KeyCode.Return))
            {
                player.Play();
                ifStartPlaying = true;
            }
        }

        public void Initialize(NCSPlayer_Player.Settings settings)
        {
            player.Initialize(settings);
        }
    }
}