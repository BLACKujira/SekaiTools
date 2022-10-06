using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI
{
    public class SaveTipCloseWindowButton : MonoBehaviour
    {
        public Window window;
        Func <string> saveFilePathGetter;

        public Func<string> SaveFilePathGetter { get => saveFilePathGetter; set => saveFilePathGetter = value; }

        public void Initialize(Func<string> saveFilePathGetter)
        {
            this.SaveFilePathGetter = saveFilePathGetter;
        }

        public void Close()
        {
            string message;
            if(!File.Exists(saveFilePathGetter()))
            {
                message = "您还没有保存文件";
            }
            else
            {
                message = $"上次保存于{File.GetLastWriteTime(saveFilePathGetter()):F}\n未保存的更改将丢失";
            }

            WindowController.ShowCancelOK("确定要退出吗", message,()=>window.Close());
        }
    }
}