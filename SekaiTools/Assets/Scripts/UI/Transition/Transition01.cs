using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.BackGround;
using UnityEngine.UI;
using DG.Tweening;
using SekaiTools.Exception;

namespace SekaiTools.UI.Transition
{
    public abstract class Transition01Base : MonoBehaviour, ITransition
    {
        public Transform imageTargetTransform;
        public float _transitionTime = 2;
        public float removeBGPartAfter = 10;
        public BackGroundPart triBurstPrefab;
        public Image whiteImagePrefab;

        public float transitionTime { get => _transitionTime; set => _transitionTime = value; }
        public BackGroundController BackGroundController { get => BackGroundController.backGroundController; }


        public void Initialize(Transform imageTargetTransform)
        {
            this.imageTargetTransform = imageTargetTransform;
        }

        public void StartTransition(IEnumerator changeCoroutine)
        {
            StartCoroutine(Transition(changeCoroutine, null));
        }

        public void StartTransition(Action changeAction)
        {
            StartCoroutine(Transition(null, changeAction));
        }

        public IEnumerator TransitionCoroutine(IEnumerator changeCoroutine)
        {
            return Transition(changeCoroutine, null);
        }

        public IEnumerator TransitionCoroutine(Action changeAction)
        {
            return Transition(null, changeAction);
        }

        protected abstract IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction);
        
        protected IEnumerator TransitionBase(IEnumerator changeCoroutine, Action changeAction, BackGroundPart backGroundPart)
        {
            Image whiteImage = Instantiate(whiteImagePrefab, imageTargetTransform);
            whiteImage.color = new Color(1, 1, 1, 0);
            whiteImage.DOFade(1, transitionTime / 2);
            yield return new WaitForSeconds(transitionTime / 2);

            if (changeCoroutine != null) yield return changeCoroutine;
            if (changeAction != null) changeAction();
            whiteImage.DOFade(0, transitionTime / 2);

            yield return new WaitForSeconds(transitionTime / 2);
            Destroy(whiteImage.gameObject);
            yield return new WaitForSeconds(removeBGPartAfter - transitionTime);
            BackGroundController.RemoveDecoration(backGroundPart);
        }
    }

    public class Transition01 : Transition01Base
    {
        protected override IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction)
        {
            BackGroundPart backGroundPart = BackGroundController.AddDecoration(triBurstPrefab);
            backGroundPart.disableRemove = true;
            yield return TransitionBase(changeCoroutine, changeAction, backGroundPart);
        }
    }
}