using SekaiTools.Cutin;
using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SekaiTools.Exception;

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
        [Header("Exception")]
        public MonoBehaviour _exceptionPrinter;

        public IExceptionPrinter exceptionPrinter => _exceptionPrinter as IExceptionPrinter;


        [NonSerialized] public CutinSceneData cutinSceneData;
        [NonSerialized] public AudioData audioData;

        /// <summary>
        /// 开始播放,在此组件上开启协程
        /// </summary>
        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }
        /// <summary>
        /// 播放协程，方便从其它组件调用
        /// </summary>
        /// <returns></returns>
        public IEnumerator IPlay()
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
            l2DController.ShowModelLeft((Character)scene.charFirstID);
            l2DController.ShowModelRight((Character)scene.charSecondID);
            l2DController.SetModelPositionLeft(modelPositionL);
            l2DController.SetModelPositionRight(modelPositionR);
            l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DController.modelR.transform.localScale = new Vector3(modelScale, modelScale, 1);

            l2DController.modelL.StopAllAnimation();
            l2DController.modelR.StopAllAnimation();

            //播放动画并试图捕获SEKAI异常
            try
            {
                l2DController.modelL.PlayAnimation(scene.talkData_First.motionCharFirst, scene.talkData_First.facialCharFirst);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            try
            {
                l2DController.modelR.PlayAnimation(scene.talkData_First.motionCharSecond, scene.talkData_First.facialCharSecond, Mathf.Infinity);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }

            //等待模型淡入后再播放动画
            yield return new WaitForSeconds(.15f);
            l2DController.FadeInAll();


            //播放语音并试图捕获SEKAI异常
            try
            {
                if (string.IsNullOrEmpty(scene.talkData_First.talkVoice)) throw new EmptyAudioNameException();
                AudioClip voiceFirst = audioData.GetValue(scene.talkData_First.talkVoice);
                if (voiceFirst == null) throw new AudioNotFoundException(scene.talkData_First.talkVoice);
                l2DController.modelL.PlayVoice(voiceFirst);
            }
            catch (System.Exception ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            talkWindow.ShowWords(scene.talkData_First.talkText, ConstData.characters[scene.charFirstID].namae, scene.talkData_First.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.GetValue(scene.talkData_First.talkVoice)?.length??0) + waitTime_Voice);

            //播放动画并试图捕获SEKAI异常
            try
            {
                l2DController.modelL.PlayAnimation(scene.talkData_Second.motionCharFirst, scene.talkData_Second.facialCharFirst);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            try
            {
                l2DController.modelR.PlayAnimation(scene.talkData_Second.motionCharSecond, scene.talkData_Second.facialCharSecond);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }

            //播放语音并试图捕获SEKAI异常
            try
            {
                if (string.IsNullOrEmpty(scene.talkData_Second.talkVoice)) throw new EmptyAudioNameException();
                AudioClip voiceSecond = audioData.GetValue(scene.talkData_Second.talkVoice);
                if (voiceSecond == null) throw new AudioNotFoundException(scene.talkData_First.talkVoice);
                l2DController.modelR.PlayVoice(voiceSecond);
            }
            catch (System.Exception ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            talkWindow.ShowWords(scene.talkData_Second.talkText, ConstData.characters[scene.charSecondID].namae, scene.talkData_Second.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.GetValue(scene.talkData_Second.talkVoice)?.length??0) + waitTime_Scene);

            talkWindow.Clear();
            l2DController.FadeOutAll();
        }
    }
}