using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.ImageCapturer
{
    public class ImageCapturer_Page : MonoBehaviour
    {
        public List<CaptureItem> captureItems;

        [System.Serializable]
        public class CaptureItem
        {
            public string name;
            public RectTransform rectTransform;
            public Texture2D mask;
        }

    }
}