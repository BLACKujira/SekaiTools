using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using SekaiTools.Kizuna;
using SekaiTools.Exception;

namespace SekaiTools.UI.KizunaSceneEditor
{
    public class KizunaSceneEditor_ModelArea : MonoBehaviour
    {
        public KizunaSceneEditor kizunaSceneEditor;
        [Header("Components")]
        public L2DControllerTypeB l2DController;
        public L2DAnimationSelectButton_Open l2DAnimationSelectButtonLF;
        public L2DAnimationSelectButton_Open l2DAnimationSelectButtonLM;
        public L2DAnimationSelectButton_Open l2DAnimationSelectButtonRF;
        public L2DAnimationSelectButton_Open l2DAnimationSelectButtonRM;
        [Header("Settings")]
        public float switchTime = 1.5f;
        public Vector2 modelPosL = new Vector2(-6.39f, -3.6f);
        public Vector2 modelPosR = new Vector2(6.39f, -3.6f);
        public float modelScale = 16;

        private void Awake()
        {
            l2DAnimationSelectButtonLF.Initialize((str) => { kizunaSceneEditor.currentKizunaScene.facialA = str; RefreshAnimation(); }, kizunaSceneEditor.window);
            l2DAnimationSelectButtonLM.Initialize((str) => { kizunaSceneEditor.currentKizunaScene.motionA = str; RefreshAnimation(); }, kizunaSceneEditor.window);
            l2DAnimationSelectButtonRF.Initialize((str) => { kizunaSceneEditor.currentKizunaScene.facialB = str; RefreshAnimation(); }, kizunaSceneEditor.window);
            l2DAnimationSelectButtonRM.Initialize((str) => { kizunaSceneEditor.currentKizunaScene.motionB = str; RefreshAnimation(); }, kizunaSceneEditor.window);
        }

        public void SetData(KizunaSceneBase kizunaScene)
        {
            StopAllCoroutines();
            StartCoroutine(ISetData(kizunaScene));
        }

        IEnumerator ISetData(KizunaSceneBase kizunaScene)
        {
            l2DController.FadeOutAll();
            yield return new WaitForSeconds(switchTime);

            l2DController.HideModelAll();
            l2DController.ShowModelLeft((Character)kizunaScene.charAID);
            l2DController.ShowModelRight((Character)kizunaScene.charBID);
            l2DController.SetModelPositionLeft(modelPosL);
            l2DController.SetModelPositionRight(modelPosR);
            l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DController.modelR.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DController.FadeInAll();

            RefreshAnimation();
        }

        public void RefreshAnimation()
        {
            try
            {
                l2DAnimationSelectButtonLF.L2DAnimationSelectButton.SetAnimation(l2DController.modelL.AnimationSet, kizunaSceneEditor.currentKizunaScene.facialA);
                l2DAnimationSelectButtonLM.L2DAnimationSelectButton.SetAnimation(l2DController.modelL.AnimationSet, kizunaSceneEditor.currentKizunaScene.motionA);
                l2DAnimationSelectButtonRF.L2DAnimationSelectButton.SetAnimation(l2DController.modelR.AnimationSet, kizunaSceneEditor.currentKizunaScene.facialB);
                l2DAnimationSelectButtonRM.L2DAnimationSelectButton.SetAnimation(l2DController.modelR.AnimationSet, kizunaSceneEditor.currentKizunaScene.motionB);

                l2DController.modelL.PlayAnimation(kizunaSceneEditor.currentKizunaScene.facialA, kizunaSceneEditor.currentKizunaScene.motionA);
                l2DController.modelR.PlayAnimation(kizunaSceneEditor.currentKizunaScene.facialB, kizunaSceneEditor.currentKizunaScene.motionB);
            }
            catch(SekaiException ex)
            {
                kizunaSceneEditor.exceptionPrinter.PrintException(ex);
            }
         }
    }
}