using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SekaiTools.UI
{
    public class TipFade : MonoBehaviour
    {
        public GraphicsAlphaController targetAlpha;
        [Header("Settings")]
        public float fadeTime;
        public float holdTime;

        private void Awake()
        {
            targetAlpha.alpha = 0;
        }

        private void Start()
        {
            StartCoroutine(MainProcess());
        }

        IEnumerator MainProcess()
        {
            DOTween.To(() => targetAlpha.alpha, (value) => targetAlpha.alpha = value, 1, fadeTime);
            yield return new WaitForSeconds(fadeTime + holdTime);
            DOTween.To(() => targetAlpha.alpha, (value) => targetAlpha.alpha = value, 0, fadeTime);
        }
    }
}