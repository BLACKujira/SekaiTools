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
    public abstract class Transition01Base : Transition
    {
        public float removeBGPartAfter = 10;
        public BackGroundPart triBurstPrefab;
        public Image whiteImagePrefab;

        public BackGroundController BackGroundController { get => BackGroundController.backGroundController; }
        protected List<BackGroundPart> currentBGParts = new List<BackGroundPart>();

        public override TransitionYieldInstruction StartTransition(IEnumerator changeCoroutine)
        {
            TransitionYieldInstruction transitionYieldInstruction = new TransitionYieldInstruction();
            StartCoroutine(Transition(changeCoroutine, null,transitionYieldInstruction));
            return transitionYieldInstruction;
        }

        public override TransitionYieldInstruction StartTransition(Action changeAction)
        {
            TransitionYieldInstruction transitionYieldInstruction = new TransitionYieldInstruction();
            StartCoroutine(Transition(null, changeAction,transitionYieldInstruction));
            return transitionYieldInstruction;
        }

        public override IEnumerator TransitionCoroutine(IEnumerator changeCoroutine)
        {
            TransitionYieldInstruction transitionYieldInstruction = new TransitionYieldInstruction();
            return Transition(changeCoroutine, null, transitionYieldInstruction);
        }

        public override IEnumerator TransitionCoroutine(Action changeAction)
        {
            TransitionYieldInstruction transitionYieldInstruction = new TransitionYieldInstruction();
            return Transition(null, changeAction,transitionYieldInstruction);
        }

        protected abstract IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction, TransitionYieldInstruction transitionYieldInstruction);
        
        protected IEnumerator TransitionBase(IEnumerator changeCoroutine, Action changeAction, BackGroundPart backGroundPart, TransitionYieldInstruction transitionYieldInstruction)
        {
            List<BackGroundPart> bgps = new List<BackGroundPart>(currentBGParts);
            foreach (var bgp in currentBGParts)
            {
                if (!bgp)
                    bgps.Remove(bgp);
            }
            currentBGParts = bgps;
            currentBGParts.Add(backGroundPart);

            Image whiteImage = Instantiate(whiteImagePrefab, targetTransform);
            whiteImage.color = new Color(1, 1, 1, 0);
            whiteImage.DOFade(1, transitionTime / 2);
            yield return new WaitForSeconds(transitionTime / 2);

            transitionYieldInstruction._keepWaiting = false;
            if (changeCoroutine != null) yield return changeCoroutine;
            if (changeAction != null) changeAction();
            whiteImage.DOFade(0, transitionTime / 2);

            yield return new WaitForSeconds(transitionTime / 2);
            Destroy(whiteImage.gameObject);
            yield return new WaitForSeconds(removeBGPartAfter - transitionTime);
            BackGroundController.RemoveDecoration(backGroundPart);
        }

        public override void Abort()
        {
            foreach (var backGroundPart in currentBGParts)
            {
                if (backGroundPart) BackGroundController.RemoveDecoration(backGroundPart);
            }
            currentBGParts = new List<BackGroundPart>();
        }
    }

    public class Transition01 : Transition01Base
    {
        public override ConfigUIItem[] configUIItems
        {
            get
            {
                return new ConfigUIItem[0];
            }
        }

        public override void LoadSettings(string serialisedSettings)
        {
        }

        public override string SaveSettings()
        {
            return string.Empty;
        }

        protected override IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction, TransitionYieldInstruction transitionYieldInstruction)
        {
            BackGroundPart backGroundPart = BackGroundController.AddDecoration(triBurstPrefab);
            backGroundPart.disableRemove = true;
            yield return TransitionBase(changeCoroutine, changeAction, backGroundPart,transitionYieldInstruction);
        }
    }
}