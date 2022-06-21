using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.TransitionEditor
{
    public class TransitionEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform targetTransform;
        [Header("Prefab")]
        public Window gswPrefab;

        [NonSerialized] public Transition.Transition transition;
        [NonSerialized] public Action<string> saveSettings;

        private void Awake()
        {
            window.OnClose.AddListener(() => transition.Abort());
        }

        public void Initialize(Transition.Transition transitionPrefab,string settings,Action<string> saveSettings)
        {
            transition = Instantiate(transitionPrefab, targetTransform);
            transition.targetTransform = targetTransform;
            if(!string.IsNullOrEmpty(settings)) transition.LoadSettings(settings);
            this.saveSettings = saveSettings;
            StartCoroutine(IPreview());

            window.OnHide.AddListener(() => StopAllCoroutines());
            window.OnReShow.AddListener(() => StartCoroutine(IPreview()));
        }

        public void Edit()
        {
            GeneralSettingsWindow.GeneralSettingsWindow generalSettingsWindow = window.OpenWindow<GeneralSettingsWindow.GeneralSettingsWindow>(gswPrefab);
            generalSettingsWindow.Initialize(transition.configUIItems,()=>saveSettings(transition.SaveSettings()));
        }

        IEnumerator IPreview()
        {
            while (true)
            {
                transition.StartTransition(()=> { });
                yield return new WaitForSeconds(5);
            }
        }
    }
}