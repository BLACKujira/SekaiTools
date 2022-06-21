using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class GSW_Item : MonoBehaviour
    {
        public abstract void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow);

        public GeneralSettingsWindow generalSettingsWindow;
        RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        [System.Serializable]
        public class ItemTypeMismatchException : System.Exception
        {
            public ItemTypeMismatchException() { }
            public ItemTypeMismatchException(string message) : base(message) { }
            public ItemTypeMismatchException(string message, System.Exception inner) : base(message, inner) { }
            protected ItemTypeMismatchException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}