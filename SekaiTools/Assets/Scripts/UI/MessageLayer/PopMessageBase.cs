using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.MessageLayer
{
    /// <summary>
    /// 基本的弹出消息
    /// </summary>
    public class PopMessageBase : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform targetTransform;
        public Image targetImage;
        [SerializeField] Text _message;

        public string message { get => _message.text; set => _message.text = value; }
    }
}