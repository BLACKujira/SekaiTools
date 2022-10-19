using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools;
using DG.Tweening;
using System;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// B型Live2D模型控制器，能同时显示两个模型，用于互动语音和牵绊语音的展示
    /// </summary>
    public class L2DControllerTypeB : MonoBehaviour
    {
        public SekaiLive2DModel[] live2DModels = new SekaiLive2DModel[32];
        [Header("Components")]
        public RawImage imageL;
        public RawImage imageR;
        [Header("Prefabs")]
        public Camera l2DCameraPrefab;

        public SekaiLive2DModel modelL { get; private set; }
        public SekaiLive2DModel modelR { get; private set; }

        readonly Vector2 unusedModelPosition = Vector2.zero;
        readonly Vector2 modelLPosition = new Vector2(-32, 0);
        readonly Vector2 modelRPosition = new Vector2(32, 0);

        Camera l2DCameraL;
        Camera l2DCameraR;

        public void SetModel(SekaiLive2DModel model, Character character)
        {
            live2DModels[(int)character] = model;
        }
        public void SetModels()
        {
            //TODO 重置此函数
            throw new NotImplementedException();
            //for (int i = 1; i < 27; i++)
            //{
            //    foreach (var model in L2DModelLoader.ModelList)
            //    {
            //        if (model.Contains(((Character)i).ToString()))
            //        {
            //            live2DModels[i] = model;
            //            break;
            //        }
            //    }
            //}
        }

        public void SetModel(SekaiLive2DModel model, int talkerId)
        {
            SetModel(model, (Character)talkerId);
        }

        public SekaiLive2DModel ShowModelLeft(Character character)
        {
            SekaiLive2DModel sekaiLive2DModel = live2DModels[(int)character];
            if (!sekaiLive2DModel) { Debug.LogError($"没有加载 {ConstData.characters[character].Name} 的模型"); return null; }
            if (modelL)
            {
                modelL.transform.position = unusedModelPosition;
                modelL.gameObject.SetActive(false);
            }
            modelL = sekaiLive2DModel;
            modelL.gameObject.SetActive(true);
            return sekaiLive2DModel;
        }
        public SekaiLive2DModel ShowModelRight(Character character)
        {
            SekaiLive2DModel sekaiLive2DModel = live2DModels[(int)character];
            if (!sekaiLive2DModel) { Debug.LogError($"没有加载 {ConstData.characters[character].Name} 的模型"); return null; }
            if (modelR)
            {
                modelR.transform.position = unusedModelPosition;
                modelR.gameObject.SetActive(false);
            }
            modelR = sekaiLive2DModel;
            modelR.gameObject.SetActive(true);
            return sekaiLive2DModel;
        }

        public SekaiLive2DModel HideModelLeft()
        {
            SekaiLive2DModel sekaiLive2DModel = null;
            if (modelL)
            {
                modelL.transform.position = unusedModelPosition;
                modelL.gameObject.SetActive(false);
                sekaiLive2DModel = modelL;
                modelL = null;
            }
            return sekaiLive2DModel;
        }
        public SekaiLive2DModel HideModelRight()
        {
            SekaiLive2DModel sekaiLive2DModel = null;
            if (modelR)
            {
                modelR.transform.position = unusedModelPosition;
                modelR.gameObject.SetActive(false);
                sekaiLive2DModel = modelR;
                modelR = null;
            }
            return sekaiLive2DModel;
        }
        public void HideModelAll()
        {
            HideModelLeft();
            HideModelRight();
        }

        public void FadeInLeft(float time = .15f)
        {
            imageL.DOFade(1, time);
        }
        public void FadeInRight(float time = .15f)
        {
            imageR.DOFade(1, time);
        }
        public void FadeInAll(float time = .15f)
        {
            FadeInLeft(time);
            FadeInRight(time);
        }

        public void FadeOutLeft(float time = .15f)
        {
            imageL.DOFade(0, time);
        }
        public void FadeOutRight(float time = .15f)
        {
            imageR.DOFade(0, time);
        }
        public void FadeOutAll(float time = .15f)
        {
            FadeOutLeft(time);
            FadeOutRight(time);
        }

        public void SetModelPositionLeft(Vector2 offset)
        {
            modelL.transform.position = modelLPosition + offset;
        }
        public void SetModelPositionRight(Vector2 offset)
        {
            modelR.transform.position = modelRPosition + offset;
        }

        public void MoveModelLeft(Vector2 toOffset,float time)
        {
            modelL.transform.DOMove(modelLPosition + toOffset,time);
        }
        public void MoveModelRight(Vector2 toOffset, float time)
        {
            modelR.transform.DOMove(modelRPosition + toOffset, time);
        }

        private void Awake()
        {
            RenderTexture renderTextureL = new RenderTexture(1920, 1080,24);
            RenderTexture renderTextureR = new RenderTexture(1920, 1080,24);
            renderTextureL.Create();
            renderTextureR.Create();
            l2DCameraL = Instantiate(l2DCameraPrefab, (Vector3)modelLPosition - Vector3.forward * 10, Quaternion.identity);
            l2DCameraR = Instantiate(l2DCameraPrefab, (Vector3)modelRPosition - Vector3.forward * 10, Quaternion.identity);
            l2DCameraL.targetTexture = renderTextureL;
            l2DCameraR.targetTexture = renderTextureR;
            imageL.texture = renderTextureL;
            imageR.texture = renderTextureR;
        }
        private void OnDestroy()
        {
            RenderTexture renderTextureL = l2DCameraL.targetTexture;
            RenderTexture renderTextureR = l2DCameraR.targetTexture;
            Destroy(l2DCameraL.gameObject);
            Destroy(l2DCameraR.gameObject);
            renderTextureL.Release();
            renderTextureR.Release();
            ResetAllModels();
        }

        public void ResetAllModels()
        {
            HideModelAll();
            foreach (var model in live2DModels)
            {
                if (model)
                {
                    model.transform.position = unusedModelPosition;
                    if (model.gameObject.activeSelf) model.gameObject.SetActive(false);
                }
            }
        }
    }
}