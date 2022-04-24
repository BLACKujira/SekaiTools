using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SekaiTools.Exception;
using System;

namespace SekaiTools.UI.MessageLayer
{
    public class MessageLayerTypeA : MessageLayerBase,IExceptionPrinter
    {
        public List<PopSettings> popObjects;
        [Header("Prefab")]
        public PopMessageBase popMessagePrefab;

        public float popTime = 0.5f;
        public float stayTime = 2f;

        Stack<string> messageStack = new Stack<string>();

        public override void ShowMessage(string message)
        {
            base.ShowMessage(message);
            bool flag = false;
            for (int i = 0; i < popObjects.Count; i++)
            {
                if (popObjects[i].popMessage == null)
                {
                    ShowMessageAt(i,message);
                    flag = true;
                    break;
                }
            }
            if(!flag)
            {
                messageStack.Push(message);
            }
        }

        [System.Serializable]
        public class PopSettings
        {
            [System.NonSerialized] public PopMessageBase popMessage;
            public Vector2 startPosition;
            public Vector2 endPosition;
        }

        void ShowMessageAt(int index,string message)
        {
            PopSettings popObject = popObjects[index];
            popObject.popMessage = Instantiate(popMessagePrefab, transform);
            popObject.popMessage.message = message;
            popObject.popMessage.targetTransform.anchoredPosition = popObject.startPosition;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(popObject.popMessage.targetTransform.DOAnchorPos(popObject.endPosition, popTime));
            sequence.AppendInterval(stayTime);
            sequence.Append(
                popObject.popMessage.targetTransform.DOAnchorPos(popObject.startPosition, popTime)
                .OnComplete(() => { Destroy(popObject.popMessage.gameObject); popObjects[index].popMessage = null; CheckStack(); })
                );
            sequence.Play();
        }

        void CheckStack()
        {
            if (messageStack.Count <= 0) return;
            ShowMessage(messageStack.Pop());
        }

        void IExceptionPrinter.PrintException(string exception, string message)
        {
            ShowMessage(exception + ":\n" + message);
        }

        void IExceptionPrinter.PrintException(System.Exception exception)
        {
            string str = exception.GetType().ToString();
            string[] strArray = str.Split('.');
            str = strArray[strArray.Length - 1];
            ShowMessage(str + ":\n" + exception.Message);
        }
    }
}