using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.MessageLayer
{
    /// <summary>
    /// 用于显示消息的UI组件
    /// </summary>
    public class MessageLayerBase : MonoBehaviour,IMessage
    {
        public virtual void ShowMessage(string message)
        {
            
        }
    }

    /// <summary>
    /// 用于显示消息的接口
    /// </summary>
    public interface IMessage
    {
        void ShowMessage(string message);
    }
}