using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI.MessageBox
{
    public class MessageBox : MonoBehaviour
    {
        public Window window;

        [SerializeField] protected Text _title;
        [SerializeField] protected Text _message;
        
        public virtual string title { get => _title.text; set => _title.text = value; }
        public virtual string message { get => _message.text; set => _message.text = value; }

        public virtual void Initialize(string title,string message,Action onClose = null)
        {
            this.title = title;
            this.message = message;
            if (onClose != null) window.OnClose.AddListener(()=>{ onClose();});
        }
    }
}