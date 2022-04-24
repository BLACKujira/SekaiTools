using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SekaiTools.UI.ImageCapturer
{
    public class ImageCapturer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public List<ImageCapturer_Page> pages;
        public Canvas targetCanvas;
        public PerecntBar perecntBar;
        public UnityEvent onFinish;

        public int itemCount { get 
            {
                int count = 0;
                foreach (var page in pages)
                {
                    count += page.captureItems.Count;
                }
                return count;
            } 
        }

        Texture2D Capture(RectTransform rectTransform)
        {
            return ExtensionTools.Capture(rectTransform, targetCanvas);
        }

        public void StartCapture(string saveFolder)
        {
            StopAllCoroutines();
            StartCoroutine(IStartCapture(saveFolder));
        }

        IEnumerator IStartCapture(string saveFolder)
        {
            float countAll = itemCount;
            float count = 0;
            if (perecntBar) perecntBar.priority = 0;

            foreach (var page in pages)
            {
                foreach (var p in pages)
                {
                    p.gameObject.SetActive(false);
                }
                page.gameObject.SetActive(true);
                foreach (var captureItem in page.captureItems)
                {
                    yield return new WaitForEndOfFrame();
                    Texture2D texture2D = Capture(captureItem.rectTransform);

                    if (captureItem.mask != null) texture2D = ExtensionTools.ApplyMask(texture2D, captureItem.mask);

                    string path = Path.Combine(saveFolder, captureItem.name + ".png");
                    File.WriteAllBytes(path, texture2D.EncodeToPNG());

                    if (perecntBar) perecntBar.priority = ++count / countAll;
                }
            }
            onFinish.Invoke();
            window.Close();
        }
    }
}