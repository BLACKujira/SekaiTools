using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    [RequireComponent(typeof(RectTransform))]
    public class GSW_Label : MonoBehaviour
    {
        public Text labelText;
        RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
    }
}