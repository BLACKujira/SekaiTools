using SekaiTools.UI.MessageBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class WindowController : MonoBehaviour
    {
        public Window currentWindow;
        [Header("Prefab")]
        public Window messageBoxWindow;
        public Window logWindow;
        public Window nowLoadingTypeAWindow;
        public Window cancelOkBoxWindow;

        public static WindowController windowController;

        private void Awake()
        {
            windowController = this;
        }

        public static void ShowMessage(string title,string message,Action onClose = null)
        {
            MessageBox.MessageBox messageBox = windowController.currentWindow.OpenWindow<MessageBox.MessageBox>(windowController.messageBoxWindow);
            messageBox.Initialize(title, message, onClose);
        }
        public static void ShowLog(string title,string log, Action onClose = null)
        {
            LogWindow.LogWindow logWindow = windowController.currentWindow.OpenWindow<LogWindow.LogWindow>(windowController.logWindow);
            logWindow.Initialize(title, log, onClose);
        }
        public static void ShowCancelOK(string title, string message, Action onOk,Action onCancel = null)
        {
            MultiOptionsMessageBox multiOptionsMessageBox = windowController.currentWindow.OpenWindow<MultiOptionsMessageBox>(windowController.cancelOkBoxWindow);
            multiOptionsMessageBox.Initialize(title, message, onOk, onCancel);
        }
        public static NowLoadingTypeA ShowNowLoadingCenter(string message, IEnumerator coroutine)
        {
            NowLoadingTypeA nowLoadingTypeA = windowController.currentWindow.OpenWindow<NowLoadingTypeA>(windowController.nowLoadingTypeAWindow);
            nowLoadingTypeA.TitleText = message;
            nowLoadingTypeA.StartProcess(coroutine);
            return nowLoadingTypeA;
        }
        public static NowLoadingTypeA ShowNowLoadingCenter(string message,Func<bool> keepWaiting)
        {
            NowLoadingTypeA nowLoadingTypeA = windowController.currentWindow.OpenWindow<NowLoadingTypeA>(windowController.nowLoadingTypeAWindow);
            nowLoadingTypeA.TitleText = message;
            nowLoadingTypeA.StartProcess(KeepWaiting(keepWaiting));
            return nowLoadingTypeA;
        }

        static IEnumerator KeepWaiting(Func<bool> keepWaiting)
        {
            while (keepWaiting())
            {
                yield return 1;
            }
        }
    }
}