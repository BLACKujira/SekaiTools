using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Windows.Forms;
using SekaiTools.DecompiledClass;
using System;

namespace SekaiTools
{
    public static class ExtensionTools
    {
        public static void CopyComponentValues(this Transform to,Transform from)
        {
            to.position = from.position;
            to.rotation = from.rotation;
            to.localScale = from.localScale;
        }

        public static Texture2D ApplyMask(Texture2D texture, Texture2D mask)
        {
            if (texture.width != mask.width || texture.height != mask.height) { throw new System.Exception("Size error"); }
            Texture2D newTex = new Texture2D(texture.width, texture.height);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    UnityEngine.Color color = texture.GetPixel(x, y);
                    color.a = mask.GetPixel(x, y).a;
                    newTex.SetPixel(x, y, color);
                }
            }
            return newTex;
        }

        public static Texture2D Capture(RectTransform rectTransform,Canvas targetCanvas)
        {
            int width = (int)(rectTransform.rect.width * targetCanvas.scaleFactor);
            int height = (int)(rectTransform.rect.height * targetCanvas.scaleFactor);

            float x = rectTransform.position.x + rectTransform.rect.xMin * targetCanvas.scaleFactor;
            float y = rectTransform.position.y + rectTransform.rect.yMin * targetCanvas.scaleFactor;

            Texture2D texture2D = new Texture2D((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
            texture2D.ReadPixels(new Rect(x, y, width, height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

        public static DateTime UnixTimeMSToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime.ToLocalTime();
        }

        public static bool IsAllElemTrue(this bool[] array)
        {
            foreach (var elem in array)
            {
                if (elem == false)
                    return false;
            }
            return true;
        }

        public static bool IsAllElemFalse(this bool[] array)
        {
            foreach (var elem in array)
            {
                if (elem == true)
                    return false;
            }
            return true;
        }

        public static bool IsAudioFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension.Equals(".ogg")
                || extension.Equals(".wav")
                || extension.Equals(".mp3"))
                return true;
            return false;
        }
    }
}