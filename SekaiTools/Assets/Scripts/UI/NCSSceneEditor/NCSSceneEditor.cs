using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NCSSceneEditor
{
    public class NCSSceneEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform editAreaRectTransform;
        [Header("Prefab")]
        public Window gswPrefab;

        Count.Showcase.NicknameCountShowcase.Scene scene;

        public void Initialize(Count.Showcase.NicknameCountShowcase.Scene scene)
        {
            this.scene = scene;
            scene.nCSScene.transform.SetParent(editAreaRectTransform);
            scene.nCSScene.rectTransform.anchoredPosition = Vector2.zero;
            scene.nCSScene.gameObject.SetActive(true);
            scene.nCSScene.Refresh();
            window.OnClose.AddListener(() =>
            {
                scene.nCSSceneSettings = scene.nCSScene.GetSaveData();
                scene.nCSScene.transform.SetParent(null);
                scene.nCSScene.gameObject.SetActive(false);
            });
        }

        public void Edit()
        {
            GeneralSettingsWindow.GeneralSettingsWindow generalSettingsWindow = window.OpenWindow<GeneralSettingsWindow.GeneralSettingsWindow>(gswPrefab);
            generalSettingsWindow.Initialize(
                scene.nCSScene.configUIItems,
                () =>
                {
                    scene.nCSSceneSettings = scene.nCSScene.GetSaveData();
                    scene.nCSScene.Refresh();
                });
        }

    }
}