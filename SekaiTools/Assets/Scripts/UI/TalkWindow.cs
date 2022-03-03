using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SekaiTools.UI
{
    public class TalkWindow : MonoBehaviour
    {
        public RectTransform rootTrans;
        [SerializeField] Text nameLabel;
        [SerializeField] Text wordsLabel;
        [SerializeField] Text translationLabel;
        [SerializeField] float wordInterval;

        [HideInInspector] public bool ifOpen = false;

        public bool IfEndShowWords { get; private set; } = true;

        Coroutine showWordsCorotine = null;

        private void Awake()
        {
            rootTrans.transform.localScale = Vector3.forward;
        }

        public void Open(float time = .15f)
        {
            rootTrans.DOScale(Vector3.one, time);
            ifOpen = true;
        }

        public void Close(float time = .15f)
        {
            rootTrans.DOScale(Vector3.forward, time).OnComplete(Clear);
            ifOpen = false;
        }

        public void ShowWords(string words, string name, string translation)
        {
            nameLabel.text = name;
            IfEndShowWords = false;
            if (showWordsCorotine != null) StopCoroutine(showWordsCorotine);
            showWordsCorotine = StartCoroutine(IShowWords(words, translation));
        }

        IEnumerator IShowWords(string words, string translation)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(wordInterval);
            for (int i = 0; i < words.Length + 1 || i < translation.Length + 1; i++)
            {
                if (!string.IsNullOrEmpty(words)) wordsLabel.text = words.Substring(0, Mathf.Min(i, words.Length));
                if (!string.IsNullOrEmpty(translation)) translationLabel.text = translation.Substring(0, Mathf.Min(i, translation.Length));
                yield return waitForSeconds;
            }
            IfEndShowWords = true;
        }

        public void Clear()
        {
            if (showWordsCorotine != null) StopCoroutine(showWordsCorotine);
            nameLabel.text = string.Empty;
            wordsLabel.text = string.Empty;
            translationLabel.text = string.Empty;
        }
    }
}