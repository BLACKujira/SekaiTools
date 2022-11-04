using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SekaiTools.UI
{
    public class Window : MonoBehaviour, IEventSystemHandler
    {
        public MonoBehaviour controlScript;
        public bool mainCameraRender = false;

        public ParentWindowEffect parentWindowEffect = ParentWindowEffect.Hide;
        public ParentWindowRaycast parentWindowRaycast = ParentWindowRaycast.Disable;

        public enum ParentWindowEffect { None,Hide }
        public enum ParentWindowRaycast { None, Disable }

        public UnityEvent OnClose;
        public UnityEvent OnShow;
        public UnityEvent OnHide;
        public UnityEvent OnReShow;

        public Window parentWindow { get; private set; } = null;
        WindowController windowController => WindowController.windowController;

        Stack<Window> referenceWindows;

        GraphicRaycaster graphicRaycaster;
        Canvas canvas;

        public GraphicRaycaster GraphicRaycaster { get { if (!graphicRaycaster) graphicRaycaster = GetComponent<GraphicRaycaster>(); return graphicRaycaster; } }
        public Canvas Canvas { get { if (!canvas) canvas = GetComponent<Canvas>();return canvas; } }

        static int windowCount = 0;

        private void Awake()
        {
            Canvas.sortingOrder = windowCount++;
            if (mainCameraRender)
            {
                Canvas.worldCamera = CameraController.MainCamera;
                gameObject.layer = 0;
            }
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="parentWindow"></param>
        public virtual void Initialize(Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// 显示窗口，并隐藏/关闭射线检测上级窗口
        /// </summary>
        public virtual void Show()
        {
            windowController.currentWindow = this;
            if (parentWindow)
            {
                switch (parentWindowEffect)
                {
                    case ParentWindowEffect.None:
                        break;
                    case ParentWindowEffect.Hide:
                        parentWindow.Hide();
                        break;
                    default:
                        break;
                }
                switch (parentWindowRaycast)
                {
                    case ParentWindowRaycast.None:
                        break;
                    case ParentWindowRaycast.Disable:
                        parentWindow.GraphicRaycaster.enabled = false;
                        break;
                    default:
                        break;
                }
            }
            gameObject.SetActive(true);
            OnShow.Invoke();
        }

        /// <summary>
        /// 隐藏窗口，但不关闭
        /// </summary>
        public virtual void Hide()
        {
            OnHide.Invoke();
            if (parentWindowEffect == ParentWindowEffect.None) parentWindow.Hide();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 重新显示隐藏的窗口
        /// </summary>
        public virtual void ReShow()
        {
            windowController.currentWindow = this;
            if (parentWindowEffect == ParentWindowEffect.None) parentWindow.ReShow();
            gameObject.SetActive(true);
            OnReShow.Invoke();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public virtual void Close()
        {
            if (parentWindow)
            {
                switch (parentWindowEffect)
                {
                    case ParentWindowEffect.None:
                        break;
                    case ParentWindowEffect.Hide:
                        parentWindow.ReShow();
                        break;
                    default:
                        break;
                }
                switch (parentWindowRaycast)
                {
                    case ParentWindowRaycast.None:
                        break;
                    case ParentWindowRaycast.Disable:
                        parentWindow.GraphicRaycaster.enabled = true;
                        break;
                    default:
                        break;
                }
            }
            OnClose.Invoke();
            Destroy(gameObject);
        }

        /// <summary>
        /// 打开一个控制脚本为T的窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T OpenWindow<T>(Window window) where T: MonoBehaviour
        {
            if (windowController.currentWindow != this)
                Debug.LogError("窗口不一致，可能导致错误");
            Window openWindow = Instantiate(window);
            T controlScript = openWindow.controlScript as T;
            if (!controlScript) throw new Exception.WindowControlScriptException();
            openWindow.Initialize(this);
            openWindow.Show();
            return controlScript;
        }

        /// <summary>
        /// 打开一个不需要操作控制脚本的窗口
        /// </summary>
        /// <param name="window"></param>
        public void OpenWindow(Window window)
        {
            Window openWindow = Instantiate(window);
            openWindow.Initialize(this);
            openWindow.Show();
        }

        public void ShowMessageBox(string title, string message, Action onClose = null)
        {
            MessageBox.MessageBox messageBox = OpenWindow<MessageBox.MessageBox>(WindowController.windowController.messageBoxWindow);
            messageBox.Initialize(title, message, onClose);
        }

        public void ShowLogWindow(string title, string log, Action onClose = null)
        {
            MessageBox.MessageBox messageBox = OpenWindow<MessageBox.MessageBox>(WindowController.windowController.logWindow);
            messageBox.Initialize(title, log, onClose);
        }

        private void OnDestroy()
        {
            windowCount--;
        }
    }
}