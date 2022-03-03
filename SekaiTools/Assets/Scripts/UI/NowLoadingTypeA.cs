using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SekaiTools.UI
{
    public class NowLoadingTypeA : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        [SerializeField] Text titleText;

        public event Action OnFinish;

        public string TitleText { get => titleText.text; set => titleText.text = value; }

        public void StartProcess(IEnumerator coroutine)
        {
            StartCoroutine(IProcess(coroutine));
        }
        IEnumerator IProcess(IEnumerator coroutine)
        {
            yield return coroutine;
            if(OnFinish!=null) OnFinish();
            window.Close();
        }
    }
}