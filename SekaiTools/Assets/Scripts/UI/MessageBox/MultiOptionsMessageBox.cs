using System;
using UnityEngine.UI;

namespace SekaiTools.UI.MessageBox
{
    public class MultiOptionsMessageBox : MessageBox
    {
        public Button[] closeButtons;

        public void Initialize(string title, string message,params Action[] onCloseActions)
        {
            this.title = title;
            this.message = message;
            if (closeButtons.Length != onCloseActions.Length)
                throw new NumberOfActionMismatchException();
            for (int i = 0; i < closeButtons.Length; i++)
            {
                int id = i;
                closeButtons[id].onClick.AddListener(() =>
                {
                    if (onCloseActions[id] != null)
                        window.OnClose.AddListener(() => onCloseActions[id]());
                    window.Close();
                });
            }
        }


        [Serializable]
        public class NumberOfActionMismatchException : System.Exception
        {
            public NumberOfActionMismatchException() { }
            public NumberOfActionMismatchException(string message) : base(message) { }
            public NumberOfActionMismatchException(string message, System.Exception inner) : base(message, inner) { }
            protected NumberOfActionMismatchException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}