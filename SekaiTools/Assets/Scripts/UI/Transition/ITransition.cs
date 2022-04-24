using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Transition
{
    public interface ITransition
    {
        float transitionTime { get; set; }

        IEnumerator TransitionCoroutine(IEnumerator changeCoroutine);
        IEnumerator TransitionCoroutine(Action changeAction);
        void StartTransition(IEnumerator changeCoroutine);
        void StartTransition(Action changeAction);
    }
}