using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;
using System.Windows.Forms;

namespace SekaiTools.UI.NicknameCountEditor
{
    public class NicknameCountEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NicknameCountEditor_SelectorArea selectorArea;
        public NicknameCountEditor_AmbiguityArea ambiguityArea;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        NicknameCountData countData;
        public NicknameCountData CountData => countData;

        private void Awake()
        {
            window.OnReShow.AddListener(() => Refresh());
        }

        public void Initialize(NicknameCountData countData)
        {
            this.countData = countData;
            selectorArea.Initialize();
            Refresh();
        }

        public void Refresh()
        {
            if(selectorArea.gameObject.activeSelf) selectorArea.Refresh();
            if(ambiguityArea.gameObject.activeSelf) ambiguityArea.Refresh();
        }

        public void Save()
        {
            int count = countData.SaveChangedFiles();
            messageLayer.ShowMessage($"保存成功\n{count}个文件已更改");
        }

        public void Close()
        {
            string message = $"{countData.ChangedFileCount}个文件已发生更改\n未保存的更改将丢失";
            WindowController.ShowCancelOK("确定要退出吗", message, () => window.Close());
        }
    }
}