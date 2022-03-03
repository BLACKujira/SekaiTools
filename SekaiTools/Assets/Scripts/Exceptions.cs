using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SekaiTools.Exception
{
    [Serializable]
    public class WindowControlScriptException : System.Exception
    {
        const string windowControlScriptException = "窗口控制脚本缺失或类型错误";

        public WindowControlScriptException() : base(windowControlScriptException) { }
        public WindowControlScriptException(string message) : base(message) { }
        public WindowControlScriptException(string message, System.Exception inner) : base(message, inner) { }
        protected WindowControlScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}