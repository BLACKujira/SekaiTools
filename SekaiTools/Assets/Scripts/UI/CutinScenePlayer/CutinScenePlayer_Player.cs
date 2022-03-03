using SekaiTools.Cutin;
using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SekaiTools.UI.CutinScenePlayer
{
    /// <summary>
    /// 互动语音场景播放器主要的播放器组件
    /// </summary>
    public class CutinScenePlayer_Player : MonoBehaviour
    {
        [Header("Components")]
        public TalkWindow talkWindow;
        public L2DControllerTypeB l2DController;
        [Header("Settings")]
        public Vector2 modelPositionL = new Vector2(-3.9f, -3.6f);
        public Vector2 modelPositionR = new Vector2(3.9f, -3.6f);
        public float modelScale = 16;
        public float waitTime_Voice;
        public float waitTime_Scene;
        public float minHoldTime;

        [NonSerialized] public CutinSceneData cutinSceneData;
        [NonSerialized] public AudioData audioData;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopAllCoroutines();
                StartCoroutine(IPlay());
            }
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }
        IEnumerator IPlay()
        {
            talkWindow.Open();
            foreach (var scene in cutinSceneData.cutinScenes)
            {
                yield return IPlayScene(scene);
                yield return new WaitForSeconds(waitTime_Scene);
            }
            l2DController.HideModelAll();
            talkWindow.Close();
        }
        IEnumerator IPlayScene(CutinScene scene)
        {
            l2DController.HideModelAll();
            l2DController.ShowModelLeft((ConstData.Character)scene.charFirstID);
            l2DController.ShowModelRight((ConstData.Character)scene.charSecondID);
            l2DController.SetModelPositionLeft(modelPositionL);
            l2DController.SetModelPositionRight(modelPositionR);
            l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DController.modelR.transform.localScale = new Vector3(modelScale, modelScale, 1);

            l2DController.modelL.StopAllAnimation();
            l2DController.modelR.StopAllAnimation();
            l2DController.modelL.PlayAnimation(scene.talkData_First.motionCharFirst, scene.talkData_First.facialCharFirst);
            l2DController.modelR.PlayAnimation(scene.talkData_First.motionCharSecond, scene.talkData_First.facialCharSecond, Mathf.Infinity);
            yield return new WaitForSeconds(.15f);
            l2DController.FadeInAll();

            l2DController.modelL.PlayVoice(audioData.audioClips[scene.talkData_First.talkVoice]);
            talkWindow.ShowWords(scene.talkData_First.talkText, ConstData.characters[scene.charFirstID].namae, scene.talkData_First.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.audioClips[scene.talkData_First.talkVoice].length) + waitTime_Voice);

            l2DController.modelL.PlayAnimation(scene.talkData_Second.motionCharFirst, scene.talkData_Second.facialCharFirst);
            l2DController.modelR.PlayAnimation(scene.talkData_Second.motionCharSecond, scene.talkData_Second.facialCharSecond);
            l2DController.modelR.PlayVoice(audioData.audioClips[scene.talkData_Second.talkVoice]);
            talkWindow.ShowWords(scene.talkData_Second.talkText, ConstData.characters[scene.charSecondID].namae, scene.talkData_Second.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.audioClips[scene.talkData_Second.talkVoice].length) + waitTime_Scene);

            talkWindow.Clear();
            l2DController.FadeOutAll();
        }
    }
}