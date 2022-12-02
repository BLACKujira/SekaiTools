using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowPlayer
{
    public class SysL2DShowPlayer_Player_Balloon : MonoBehaviour
    {
        [Header("Components")]
        public Text text;
        [Header("Settings")]
        public float secPerWord;

        string serif;
        public string Serif
        {
            get => serif;
        }

        public void Clear()
        {
            text.text = string.Empty;
        }

        public void SetSerif(string serif)
        {
            StopAllCoroutines();
            this.serif = serif;
        }

        public void Play()
        {
            StartCoroutine(CoPlay());
        }

        IEnumerator CoPlay()
        {
            string showWords = serif.Replace("\\n", "\n");
            for (int i = 0; i < showWords.Length + 1; i++)
            {
                text.text = showWords.Substring(0, i);
                yield return new WaitForSeconds(secPerWord);
            }
        }
    }
}