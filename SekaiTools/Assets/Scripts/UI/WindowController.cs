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

        public static WindowController windowController;

        private void Awake()
        {
            windowController = this;
        }
    }
}