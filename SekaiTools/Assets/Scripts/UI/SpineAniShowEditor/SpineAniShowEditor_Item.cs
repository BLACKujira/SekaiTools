using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniShowEditor
{
    public class SpineAniShowEditor_Item : MonoBehaviour
    {
        public SpineAniShowEditor spineAniShowEditor;
        [Header("Components_Button")]
        public Button buttonRemove;
        public Button buttonEdit;
        public Button buttonOrderUp;
        public Button buttonOrderDown;
        [Header("Components_Main")]
        public Text textItemName;
        public Text textDescription;
        public InputField inputFieldHoldTime;
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

        public void Initialize(SpineSceneWithMeta scene, SpineAniShowEditor spineAniShowEditor)
        {
            this.spineAniShowEditor = spineAniShowEditor;

            #region  button event
            buttonRemove.onClick.AddListener(() =>
            {
                spineAniShowEditor.data.spineScenes.Remove(scene);
                spineAniShowEditor.Refresh();
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

                SpineSceneEditor.SpineSceneEditor spineSceneEditor = spineAniShowEditor.window.OpenWindow<SpineSceneEditor.SpineSceneEditor>(sceneEditorWindowPrefab);

                if (scene.changeBackGround)
                {
                    spineSceneEditor.window.OnClose.AddListener(() =>
                    {
                        BackGroundController.backGroundController.ClearAndReset();
                        BackGroundController.backGroundController.Load(backGroundSaveData);
                    });
                }

                spineSceneEditor.Initialize(scene.spineScene, (ss) =>
                {
                    scene.spineScene = ss;
                    spineAniShowEditor.Refresh();
                });
            });

            buttonOrderUp.onClick.AddListener(() =>
            {
                int index = spineAniShowEditor.data.spineScenes.IndexOf(scene);
                if (index <= 0) return;
                spineAniShowEditor.data.spineScenes[index] = spineAniShowEditor.data.spineScenes[index - 1];
                spineAniShowEditor.data.spineScenes[index - 1] = scene;
                spineAniShowEditor.Refresh();
            });

            buttonOrderDown.onClick.AddListener(() =>
            {
                int index = spineAniShowEditor.data.spineScenes.IndexOf(scene);
                if (index >= spineAniShowEditor.data.spineScenes.Count - 1) return;
                spineAniShowEditor.data.spineScenes[index] = spineAniShowEditor.data.spineScenes[index + 1];
                spineAniShowEditor.data.spineScenes[index + 1] = scene;
                spineAniShowEditor.Refresh();
            });
            #endregion

            textItemName.text = (spineAniShowEditor.data.spineScenes.IndexOf(scene)+1).ToString();
            textDescription.text = scene.Information;
            inputFieldHoldTime.text = scene.holdTime.ToString();
            inputFieldHoldTime.onEndEdit.AddListener((str) =>
            {
                if (!float.TryParse(inputFieldHoldTime.text, out scene.holdTime))
                    scene.holdTime = 10f;
                inputFieldHoldTime.text = scene.holdTime.ToString();
            });

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
                    spineAniShowEditor.Refresh();
                });
            });
            textBgName.text = scene.backGround.backGround.name;

            toggleTrOn.isOn = scene.useTransition;
            toggleTrOn.onValueChanged.AddListener((value) => scene.useTransition = value);
            buttonTrEdit.onClick.AddListener(() =>
            {
                TransitionEditor.TransitionEditor transitionEditor = spineAniShowEditor.window.OpenWindow<TransitionEditor.TransitionEditor>(transitionEditorWindowPrefab);
                transitionEditor.Initialize(GlobalData.globalData.transitionSet.GetValue(scene.transition.type), scene.transition.serializedSettings,
                    (value) =>
                    {
                        scene.transition.serializedSettings = value;
                        spineAniShowEditor.Refresh();
                    });
            });
            if (scene.transition == null || string.IsNullOrEmpty(scene.transition.type)) buttonTrEdit.interactable = false;
            buttonTrChange.onClick.AddListener(() =>
            {
                UniversalSelector universalSelector = spineAniShowEditor.window.OpenWindow<UniversalSelector>(transitionSelectWindowPrefab);
                List<Transition.Transition> transitions = GlobalData.globalData.transitionSet.transitions;
                universalSelector.Generate(transitions.Count, (Button button, int id) =>
                {
                    ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                    buttonWithIconAndText.Label = transitions[id].itemName;
                    buttonWithIconAndText.Icon = transitions[id].preview;
                },
                (int id) =>
                {
                    scene.transition = new Transition.SerializedTransition(transitions[id].name, transitions[id].SaveSettings());
                    spineAniShowEditor.Refresh();
                });
            });
            textTrName.text = scene.transition?.type ?? "Null";
        }
    }
}