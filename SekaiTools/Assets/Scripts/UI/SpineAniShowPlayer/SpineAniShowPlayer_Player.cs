using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SekaiTools.UI.Transition;
using System;

namespace SekaiTools.UI.SpineAniShowPlayer
{

    public class SpineAniShowPlayer_Player : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform targetTransformTransition;
        [Header("Settings")]
        public float waitTimeSafe = .3f;
        [Header("Prefab")]
        public BackGroundPart bgpSpinePrefab;
        public SpineControllerTypeA spineControllerPrefab;

        SpineControllerTypeA spineController;
        BackGroundPart bgpSpine;
        SpineAniShowData spineAniShowData;

        public BackGroundController backGroundController => BackGroundController.backGroundController;

        private void Awake()
        {
            spineController = Instantiate(spineControllerPrefab);
        }

        private void OnDestroy()
        {
            if (spineController) Destroy(spineController.gameObject);
            if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
        }

        private void OnEnable()
        {
            if (spineController) spineController.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (spineController) spineController.gameObject.SetActive(false);
        }

        public void Initialize(SpineAniShowData spineAniShowData)
        {
            this.spineAniShowData = spineAniShowData;
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }

        IEnumerator IPlay()
        {
            Transition.Transition currentTransition = null;
            for (int i = 0; i < spineAniShowData.spineScenes.Count; i++)
            {
                SpineSceneWithMeta spineSceneWithMeta = spineAniShowData.spineScenes[i];

                #region playScene
                if (spineSceneWithMeta.changeBackGround)
                    BackGroundController.backGroundController.Load(spineSceneWithMeta.backGround);

                if (bgpSpine) backGroundController.RemoveDecoration(bgpSpine);
                bgpSpine = backGroundController.AddDecoration(bgpSpinePrefab, Mathf.Min(spineSceneWithMeta.spineScene.spineLayerID, backGroundController.Decorations.Count));
                spineController.ClearModel();
                spineController.LoadData(spineSceneWithMeta.spineScene);

                yield return new WaitForSeconds(spineSceneWithMeta.holdTime);
                #endregion
                if (i < spineAniShowData.spineScenes.Count - 1 && spineAniShowData.spineScenes[i + 1].useTransition)
                {
                    if (currentTransition != null)
                    {
                        currentTransition.Abort();
                        Destroy(currentTransition.gameObject);
                    }
                    Transition.SerializedTransition transitionData = spineAniShowData.spineScenes[i + 1].transition;
                    Transition.Transition transitionPrefab = GlobalData.globalData.transitionSet.GetValue(transitionData.type);
                    currentTransition = Instantiate(transitionPrefab, transform);
                    currentTransition.targetTransform = targetTransformTransition;
                    currentTransition.LoadSettings(transitionData.serializedSettings);
                    yield return currentTransition.StartTransition(waitTimeSafe);
                }
            }
        }
    }
}