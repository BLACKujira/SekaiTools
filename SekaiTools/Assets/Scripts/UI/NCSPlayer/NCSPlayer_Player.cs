using SekaiTools.UI.NCSEditor;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.UI.BackGround;
using SekaiTools.Count;
using SekaiTools.Live2D;

namespace SekaiTools.UI.NCSPlayer
{
    public class NCSPlayer_Player : NCSPlayerBase
    {
        [Header("Components")]
        public RectTransform targetTransformScene;
        public RectTransform targetTransformTransition;
        [Header("Settings")]
        public float waitTimeSafe = .3f;

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }

        IEnumerator IPlay()
        {
            Transition.Transition currentTransition = null;
            for (int i = 0; i < showcase.scenes.Count; i++)
            {
                Count.Showcase.NicknameCountShowcase.Scene scene = showcase.scenes[i];

                #region playScene
                if (scene.changeBackGround)
                    BackGroundController.backGroundController.Load(scene.backGround);

                scene.nCSScene.gameObject.SetActive(true);
                scene.nCSScene.Refresh();
                scene.nCSScene.transform.SetParent(targetTransformScene);
                scene.nCSScene.rectTransform.localScale = Vector2.one;
                scene.nCSScene.rectTransform.anchoredPosition = Vector2.zero;

                yield return new WaitForSeconds(scene.nCSScene.holdTime);
                while (!scene.nCSScene.CanMoveNext) yield return 1;

                #endregion
                if(i< showcase.scenes.Count-1&&showcase.scenes[i+1].useTransition)
                {
                    if(currentTransition!=null)
                    {
                        currentTransition.Abort();
                        Destroy(currentTransition.gameObject);
                    }
                    Transition.SerializedTransition transitionData = showcase.scenes[i + 1].transition;
                    Transition.Transition transitionPrefab = GlobalData.globalData.transitionSet.GetValue(transitionData.type);
                    currentTransition = Instantiate(transitionPrefab, transform);
                    currentTransition.targetTransform = targetTransformTransition;
                    currentTransition.LoadSettings(transitionData.serializedSettings);
                    yield return currentTransition.StartTransition(waitTimeSafe);
                    Destroy(scene.nCSScene.gameObject);
                }
            }
        }
    }
}