using SekaiTools.Cutin;
using SekaiTools.Exception;
using SekaiTools.Kizuna;
using SekaiTools.Live2D;
using SekaiTools.UI.CutinScenePlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayer_Player : MonoBehaviour
    {
        [Header("Components")]
        public KizunaScenePlayerBase_Player_MainBase mainArea;
        public RectTransform rtTalkWindow;
        public L2DControllerTypeB l2DController;
        public KizunaScenePlayer_BGController bGController;
        [Header("Settings")]
        public float modelPositionY = -3.6f;
        public float modelPositionHonor = 6.25f;
        public float modelPositionCutin = 3.9f;
        public float modelScale = 16;
        public float waitTime_ShowHonor = 10;
        public float waitTime_MoveModel = 1;
        public float waitTime_Voice;
        public float waitTime_Scene;
        public float gridFadeTime = 1.5f;
        public float minHoldTime;
        public CutinScenePlayerSet cutinPlayerSet;
        [Header("Exception")]
        public MonoBehaviour _exceptionPrinter;

        public IExceptionPrinter exceptionPrinter => _exceptionPrinter as IExceptionPrinter;

        [NonSerialized] public AudioData audioData;
        [NonSerialized] public ImageData imageData;
        [NonSerialized] public KizunaSceneDataBase kizunaSceneData;

        TalkWindow talkWindow;

        public void Initialize(KizunaSceneEditor.KizunaSceneEditor.Settings settings)
        {
            audioData = settings.audioData;
            imageData = settings.imageData;
            kizunaSceneData = settings.kizunaSceneData;
            l2DController.live2DModels = settings.sekaiLive2DModels;
            l2DController.ResetAllModels();
            l2DController.FadeOutAll(0);
            bGController.Initialize(settings.backGroundParts);

            CutinScenePlayerSet_Item cutinScenePlayerSet_Item
                = cutinPlayerSet.GetItem(kizunaSceneData.cutinPlayerType) ?? cutinPlayerSet.DefaultItem;
            talkWindow = Instantiate(cutinScenePlayerSet_Item.player.talkWindow, rtTalkWindow);

            if (mainArea is KizunaScenePlayer_Player_Main)
            {
                ((KizunaScenePlayer_Player_Main)mainArea).Initialize(imageData);
            }
            mainArea.Hide();
            bGController.FadeInGrid(gridFadeTime);
            if (kizunaSceneData is KizunaSceneData)
            {
                bGController.SetScene(((KizunaSceneData)kizunaSceneData).kizunaScenes[0]);
                ((KizunaScenePlayer_Player_Main)mainArea).SetScene(((KizunaSceneData)kizunaSceneData).kizunaScenes[0]);
            }
            else
            {
                bGController.SetScene(((CustomKizunaData)kizunaSceneData).kizunaScenes[0]);
                ((KizunaScenePlayerCustom_Player_Main)mainArea).SetScene(((CustomKizunaData)kizunaSceneData).kizunaScenes[0]);
            }
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }

        public IEnumerator IPlay()
        {
            if(kizunaSceneData is KizunaSceneData)
            {
                foreach (var scene in ((KizunaSceneData)kizunaSceneData).kizunaScenes)
                {
                    yield return IPlayScene(scene);
                }
            }
            else
            {
                foreach (var scene in ((CustomKizunaData)kizunaSceneData).kizunaScenes)
                {
                    yield return IPlayScene(scene);
                }
            }
        }

        IEnumerator IPlayScene(KizunaSceneBase scene)
        {
            bGController.SetScene(scene);
            l2DController.HideModelAll();
            l2DController.ShowModelLeft((Character)scene.charAID);
            l2DController.ShowModelRight((Character)scene.charBID);
            l2DController.SetModelPositionLeft(new Vector2(-modelPositionHonor, modelPositionY));
            l2DController.SetModelPositionRight(new Vector2(modelPositionHonor, modelPositionY));
            l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DController.modelR.transform.localScale = new Vector3(modelScale, modelScale, 1);

            l2DController.modelL.StopAllAnimation();
            l2DController.modelR.StopAllAnimation();

            //播放动画
            try
            {
                l2DController.modelL.PlayAnimation(scene.motionA, scene.facialA);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            try
            {
                l2DController.modelR.PlayAnimation(scene.motionB, scene.facialB);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }

            yield return new WaitForSeconds(.15f);
            l2DController.FadeInAll(.25f);

            //展开成就牌子
            if (scene is KizunaScene)
            {
                ((KizunaScenePlayer_Player_Main)mainArea).SetScene((KizunaScene)scene);
            }
            else
            {
                ((KizunaScenePlayerCustom_Player_Main)mainArea).SetScene((KizunaSceneCustom)scene);
            }
            mainArea.Show();
            yield return new WaitForSeconds(waitTime_ShowHonor);
            mainArea.Hide();

            if (scene.cutinScenes.Count != 0)
            {
                bGController.FadeOutGrid(gridFadeTime);
                //如果有互动语音则播放互动语音
                for (float i = 0; i < waitTime_MoveModel; i+=Time.fixedDeltaTime)
                {
                    float lerpT = i / waitTime_MoveModel;
                    float x = Mathf.Lerp(modelPositionHonor, modelPositionCutin, lerpT);
                    l2DController.SetModelPositionLeft(new Vector2(-x, modelPositionY));
                    l2DController.SetModelPositionRight(new Vector2(x, modelPositionY));
                    yield return new WaitForFixedUpdate();
                }

                talkWindow.Open();

                for (int i = 0; i < scene.cutinScenes.Count; i++)
                {
                    CutinScene cutinScene = scene.cutinScenes[i];
                    l2DController.FadeInAll();
                    if (i==0)
                    {
                        if (cutinScene.charFirstID==scene.charAID&&cutinScene.charSecondID==scene.charBID)
                        {
                            yield return IPlayCutinScene(cutinScene, l2DController.modelL, l2DController.modelR, true);
                        }
                        else
                        {
                            yield return IPlayCutinScene(cutinScene, l2DController.modelR, l2DController.modelL, true);
                        }
                    }
                    else
                    {
                        l2DController.HideModelAll();
                        l2DController.ShowModelLeft((Character)cutinScene.charFirstID);
                        l2DController.ShowModelRight((Character)cutinScene.charSecondID);
                        l2DController.SetModelPositionLeft(new Vector2(-modelPositionCutin, modelPositionY));
                        l2DController.SetModelPositionRight(new Vector2(modelPositionCutin, modelPositionY));
                        l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
                        l2DController.modelR.transform.localScale = new Vector3(modelScale, modelScale, 1);
                        yield return IPlayCutinScene(cutinScene, l2DController.modelL, l2DController.modelR);
                    }
                    talkWindow.Clear();
                    if(i== scene.cutinScenes.Count-1)
                        talkWindow.Close();
                    l2DController.FadeOutAll();
                    yield return new WaitForSeconds(waitTime_Scene);
                }

                bGController.FadeInGrid(gridFadeTime);
            }
            else
            {
                //如果有互动语音则直接淡出
                talkWindow.Clear();
                l2DController.FadeOutAll();
                yield return new WaitForSeconds(waitTime_Scene);
            }

        }
        IEnumerator IPlayCutinScene(CutinScene scene, SekaiLive2DModel modelL, SekaiLive2DModel modelR,bool firstScene = false)
        {

            #region 播放互动语音
            //播放动画并试图捕获SEKAI异常
            try
            {
                modelL.PlayAnimation(scene.talkData_First.motionCharFirst, scene.talkData_First.facialCharFirst);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            try
            {
                modelR.PlayAnimation(scene.talkData_First.motionCharSecond, scene.talkData_First.facialCharSecond, firstScene?1:Mathf.Infinity);
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
                modelL.PlayVoice(voiceFirst);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            talkWindow.ShowWords(scene.talkData_First.talkText, ConstData.characters[scene.charFirstID].namae, scene.talkData_First.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.GetValue(scene.talkData_First.talkVoice)?.length ?? 0) + waitTime_Voice);

            //播放动画并试图捕获SEKAI异常
            try
            {
                modelL.PlayAnimation(scene.talkData_Second.motionCharFirst, scene.talkData_Second.facialCharFirst);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            try
            {
                modelR.PlayAnimation(scene.talkData_Second.motionCharSecond, scene.talkData_Second.facialCharSecond);
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
                modelR.PlayVoice(voiceSecond);
            }
            catch (SekaiException ex)
            {
                exceptionPrinter.PrintException(ex);
            }
            talkWindow.ShowWords(scene.talkData_Second.talkText, ConstData.characters[scene.charSecondID].namae, scene.talkData_Second.talkText_Translate);
            yield return new WaitForSeconds(Mathf.Max(minHoldTime, audioData.GetValue(scene.talkData_Second.talkVoice)?.length ?? 0) + waitTime_Scene);

            #endregion
        }
    }
}