using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SekaiTools.Exception
{
    public interface IExceptionPrinter
    {
        void PrintException(string exception, string message);
        void PrintException(System.Exception exception);
    }

    [Serializable]
    public class SekaiException : System.Exception
    {
        public SekaiException() { }
        public SekaiException(string message) : base(message) { }
        public SekaiException(string message, System.Exception inner) : base(message, inner) { }
        protected SekaiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class WindowControlScriptException : SekaiException
    {
        const string description = "窗口控制脚本缺失或类型错误";

        public WindowControlScriptException() : base(description) { }
        public WindowControlScriptException(string message) : base(message) { }
        public WindowControlScriptException(string message, System.Exception inner) : base(message, inner) { }
        protected WindowControlScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class AnimationNotFoundException : SekaiException
    {
        const string description = "在模型的动画集未找到指定的动画";

        public AnimationNotFoundException() : base(description) { }
        public AnimationNotFoundException(string message) : base(description+' '+message) { }
        public AnimationNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected AnimationNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class AudioNotFoundException : SekaiException
    {
        const string description = "未找到指定的音频";

        public AudioNotFoundException() : base(description) { }
        public AudioNotFoundException(string message) : base(description + ' ' + message) { }
        public AudioNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected AudioNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class EmptyAudioNameException : SekaiException
    {
        const string description = "音频缺失";

        public EmptyAudioNameException() : base(description) { }
        public EmptyAudioNameException(string message) : base(message) { }
        public EmptyAudioNameException(string message, System.Exception inner) : base(message, inner) { }
        protected EmptyAudioNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class AnimationSetNotSetException : SekaiException
    {
        const string description = "动画集合未设置";

        public AnimationSetNotSetException() : base(description) { }
        public AnimationSetNotSetException(string message) : base(description + ' ' + message) { }
        public AnimationSetNotSetException(string message, System.Exception inner) : base(message, inner) { }
        protected AnimationSetNotSetException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UninitializedException : SekaiException
    {
        const string description = "未初始化";

        public UninitializedException() : base(description) { }
        public UninitializedException(string message) : base(message) { }
        public UninitializedException(string message, System.Exception inner) : base(message, inner) { }
        protected UninitializedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}