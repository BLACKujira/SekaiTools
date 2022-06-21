using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;
using SekaiTools.Count.Showcase;
using SekaiTools.UI.NicknameCountShowcase;
using SekaiTools.UI.BackGround;

namespace SekaiTools.UI.NCSEditor
{
    public class NCSEditor_Item : MonoBehaviour
    {
        public NCSEditor nCSEditor;
        [Header("Components_Button")]
        public Button buttonRemove;
        public Button buttonEdit;
        public Button buttonOrderUp;
        public Button buttonOrderDown;
        [Header("Components_Main")]
        public Text textItemName;
        public Text textDescription;
        public Image imagePreview;
        [Header("Components_BackGround")]
        public Toggle toggleBgOn;
        public Button buttonBgEdit;
        public Text textBgName;
        [Header("Components_Transition")]
        public Toggle toggleTrOn;
        public Button buttonTrEdit;
        public Button buttonTrChange;
        public Text textTrName;
        [Header("Prefab")]
        public Window transitionSelectWindowPrefab;
        public Window transitionEditorWindowPrefab;
        public Window sceneEditorWindowPrefab;
        public Window backgroundEditorWindowPrefab;

        Count.Showcase.NicknameCountShowcase.Scene scene;

        public void Initialize(Count.Showcase.NicknameCountShowcase.Scene scene, NCSEditor nCSEditor)
        {
            this.scene = scene;
            this.nCSEditor = nCSEditor;

            #region  button event
            buttonRemove.onClick.AddListener(() => 
            {
                nCSEditor.showcase.scenes.Remove(scene);
                nCSEditor.Refresh();
            });

            buttonEdit.onClick.AddListener(() =>
            {
                BackGroundController.BackGroundSaveData backGroundSaveData = null;
                if (scene.changeBackGround)
                {
                    backGroundSaveData = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
                    BackGroundController.backGroundController.ClearAndReset();
                    BackGroundController.backGroundController.Load(scene.backGround);
                }

                NCSSceneEditor.NCSSceneEditor nCSSceneEditor = nCSEditor.window.OpenWindow<NCSSceneEditor.NCSSceneEditor>(sceneEditorWindowPrefab);

                if (scene.changeBackGround)
                {
                    nCSSceneEditor.window.OnClose.AddListener(() =>
                    {
                        BackGroundController.backGroundController.ClearAndReset();
                        BackGroundController.backGroundController.Load(backGroundSaveData);
                    });
                }

                scene.nCSScene.Initialize(nCSEditor);
                nCSSceneEditor.Initialize(scene);
                nCSSceneEditor.window.OnClose.AddListener(() => nCSEditor.Refresh());
            });

            buttonOrderUp.onClick.AddListener(() =>
            {
                int index = nCSEditor.showcase.scenes.IndexOf(scene);
                if (index <= 0) return;
                nCSEditor.showcase.scenes[index] = nCSEditor.showcase.scenes[index - 1];
                nCSEditor.showcase.scenes[index - 1] = scene;
                nCSEditor.Refresh();
            });

            buttonOrderDown.onClick.AddListener(() =>
            {
                int index = nCSEditor.showcase.scenes.IndexOf(scene);
                if (index >= nCSEditor.showcase.scenes.Count - 1) return;
                nCSEditor.showcase.scenes[index] = nCSEditor.showcase.scenes[index + 1];
                nCSEditor.showcase.scenes[index + 1] = scene;
                nCSEditor.Refresh();
            });
            #endregion

            textItemName.text = scene.nCSScene.itemName;
            textDescription.text = scene.nCSScene.information;
            imagePreview.sprite = scene.nCSScene.preview;

            toggleBgOn.isOn = scene.changeBackGround;
            toggleBgOn.onValueChanged.AddListener((value) => scene.changeBackGround = value);
            buttonBgEdit.onClick.AddListener(() =>
            {
                BackGroundController.BackGroundSaveData backGroundSaveData = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
                BackGroundController.backGroundController.Load(scene.backGround);
                BackGroundSettings.BackGroundSettings backGroundSettings = WindowController.windowController.currentWindow.OpenWindow<BackGroundSettings.BackGroundSettings>(backgroundEditorWindowPrefab);
                backGroundSettings.window.OnClose.AddListener(() =>
                {
                    BackGroundController.BackGroundSaveData bgsd = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
                    scene.backGround = bgsd;
                    BackGroundController.backGroundController.Load(backGroundSaveData);
                    nCSEditor.Refresh();
                });
            });
            textBgName.text = scene.backGround.backGround.name;

            toggleTrOn.isOn = scene.useTransition;
            toggleTrOn.onValueChanged.AddListener((value) => scene.useTransition = value);
            buttonTrEdit.onClick.AddListener(() =>
            {
                TransitionEditor.TransitionEditor transitionEditor = nCSEditor.window.OpenWindow<TransitionEditor.TransitionEditor>(transitionEditorWindowPrefab);
                transitionEditor.Initialize(GlobalData.globalData.transitionSet.GetValue(scene.transition.type), scene.transition.serialisedSettings,
                    (value) =>
                    {
                        scene.transition.serialisedSettings = value;
                        nCSEditor.Refresh();
                    });
            });
            if (scene.transition == null||string.IsNullOrEmpty(scene.transition.type)) buttonTrEdit.interactable = false;
            buttonTrChange.onClick.AddListener(() =>
            {
                UniversalSelector universalSelector = nCSEditor.window.OpenWindow<UniversalSelector>(transitionSelectWindowPrefab);
                List<Transition.Transition> transitions = GlobalData.globalData.transitionSet.transitions;
                universalSelector.Generate(transitions.Count, (Button button, int id) =>
                {
                    ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                    buttonWithIconAndText.Label = transitions[id].itemName;
                    buttonWithIconAndText.Icon = transitions[id].preview;
                },
                (int id) =>
                {
                    scene.transition = new Count.Showcase.NicknameCountShowcase.Transition(transitions[id].name, string.Empty);
                    nCSEditor.Refresh();
                });
            });
            textTrName.text = scene.transition?.type ?? "无";
        }
    }
}