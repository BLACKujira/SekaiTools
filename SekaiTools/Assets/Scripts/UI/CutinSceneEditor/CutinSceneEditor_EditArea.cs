using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Cutin;
using SekaiTools.Live2D;
using SekaiTools.UI.Live2DMotionSelect;
using System;

namespace SekaiTools.UI.CutinSceneEditor
{
    public class CutinSceneEditor_EditArea : MonoBehaviour
    {
        public CutinSceneEditor cutinSceneEditor;
        [Header("L2DAnimationSelectButton")]
        public L2DAnimationSelectButton_Open button_SF_CF_F;
        public L2DAnimationSelectButton_Open button_SF_CF_M;
        public L2DAnimationSelectButton_Open button_SF_CS_F;
        public L2DAnimationSelectButton_Open button_SF_CS_M;
        public L2DAnimationSelectButton_Open button_SS_CF_F;
        public L2DAnimationSelectButton_Open button_SS_CF_M;
        public L2DAnimationSelectButton_Open button_SS_CS_F;
        public L2DAnimationSelectButton_Open button_SS_CS_M;
        [Header("InputField")]
        public InputField inputField_SF_O;
        public InputField inputField_SF_T;
        public InputField inputField_SS_O;
        public InputField inputField_SS_T;
        [Header("Character")]
        public List<Text> nameLabels_F;
        public List<Text> nameLabels_S;
        public List<Image> iconImage_F;
        public List<Image> iconImage_S;
        [Header("Audio")]
        public Button playAudioButton_F;        
        public Button playAudioButton_S;        

        [Header("Prefabs")]
        public Window motionSelectWindwPrefab;
        [Header("Settings")]
        public InbuiltAnimationSet inbuiltAnimationSet;
        public IconSet iconSet;

        public CutinScene cutinScene;

        private void Awake()
        {
            button_SF_CF_F.Initialize((string str) =>
            {
                cutinScene.talkData_First.facialCharFirst = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SF_CF_M.Initialize((string str) =>
            {
                cutinScene.talkData_First.motionCharFirst = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SF_CS_F.Initialize((string str) =>
            {
                cutinScene.talkData_First.facialCharSecond = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SF_CS_M.Initialize((string str) =>
            {
                cutinScene.talkData_First.motionCharSecond = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SS_CF_F.Initialize((string str) =>
            {
                cutinScene.talkData_Second.facialCharFirst = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SS_CF_M.Initialize((string str) =>
            {
                cutinScene.talkData_Second.motionCharFirst = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SS_CS_F.Initialize((string str) =>
            {
                cutinScene.talkData_Second.facialCharSecond = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);
            button_SS_CS_M.Initialize((string str) =>
            {
                cutinScene.talkData_Second.motionCharSecond = str;
                SetScene(cutinScene);
            }, cutinSceneEditor.window);

            playAudioButton_F.onClick.AddListener(() => cutinSceneEditor.PlayAudioClip(cutinScene.talkData_First.talkVoice));
            playAudioButton_S.onClick.AddListener(() => cutinSceneEditor.PlayAudioClip(cutinScene.talkData_Second.talkVoice));

            inputField_SF_O.onValueChanged.AddListener((string str) => { cutinScene.talkData_First.talkText = str; });
            inputField_SF_T.onValueChanged.AddListener((string str) => { cutinScene.talkData_First.talkText_Translate = str; });
            inputField_SS_O.onValueChanged.AddListener((string str) => { cutinScene.talkData_Second.talkText = str; });
            inputField_SS_T.onValueChanged.AddListener((string str) => { cutinScene.talkData_Second.talkText_Translate = str; });
        }

        public void SetScene(CutinScene cutinScene)
        {
            this.cutinScene = cutinScene;
            throw new System.NotImplementedException();
            L2DAnimationSet animationSetFirst = null;//inbuiltAnimationSet.l2DAnimationSets[cutinScene.charFirstID];
            L2DAnimationSet animationSetSecond = null;//inbuiltAnimationSet.l2DAnimationSets[cutinScene.charSecondID];

            button_SF_CF_F.L2DAnimationSelectButton.SetAnimation(animationSetFirst, cutinScene.talkData_First.facialCharFirst);
            button_SF_CF_M.L2DAnimationSelectButton.SetAnimation(animationSetFirst, cutinScene.talkData_First.motionCharFirst);
            button_SF_CS_F.L2DAnimationSelectButton.SetAnimation(animationSetSecond, cutinScene.talkData_First.facialCharSecond);
            button_SF_CS_M.L2DAnimationSelectButton.SetAnimation(animationSetSecond, cutinScene.talkData_First.motionCharSecond);
            button_SS_CF_F.L2DAnimationSelectButton.SetAnimation(animationSetFirst, cutinScene.talkData_Second.facialCharFirst);
            button_SS_CF_M.L2DAnimationSelectButton.SetAnimation(animationSetFirst, cutinScene.talkData_Second.motionCharFirst);
            button_SS_CS_F.L2DAnimationSelectButton.SetAnimation(animationSetSecond, cutinScene.talkData_Second.facialCharSecond);
            button_SS_CS_M.L2DAnimationSelectButton.SetAnimation(animationSetSecond, cutinScene.talkData_Second.motionCharSecond);

            inputField_SF_O.text = cutinScene.talkData_First.talkText;
            inputField_SF_T.text = cutinScene.talkData_First.talkText_Translate;
            inputField_SS_O.text = cutinScene.talkData_Second.talkText;
            inputField_SS_T.text = cutinScene.talkData_Second.talkText_Translate;

            foreach (var text in nameLabels_F)
            {
                text.text = ConstData.characters[cutinScene.charFirstID].namae;
            }
            foreach (var text in nameLabels_S)
            {
                text.text = ConstData.characters[cutinScene.charSecondID].namae;
            }
            foreach (var image in iconImage_F)
            {
                image.sprite = iconSet.icons[cutinScene.charFirstID];
            }
            foreach (var image in iconImage_S)
            {
                image.sprite = iconSet.icons[cutinScene.charSecondID];
            }
        }
    }
}