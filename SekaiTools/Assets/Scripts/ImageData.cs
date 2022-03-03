using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace SekaiTools
{
    /// <summary>
    /// 图像资料
    /// </summary>
    public class ImageData : MonoBehaviour
    {
        public List<Sprite> spriteList = new List<Sprite>();
        public Dictionary<string, Sprite> sprites;

        public IEnumerator LoadImages(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                yield return LoadImage(file);
            }
            sprites = new Dictionary<string, Sprite>();
            foreach (var sprite in spriteList)
            {
                sprites[sprite.texture.name] = sprite;
            }
        }
        public IEnumerator LoadImage(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            extension = extension.ToLower();
            bool flag = false;
            switch (extension)
            {
                case ".png":
                    flag = true;
                    break;
                case ".jpg":
                    flag = true;
                    break;
            }
            if (!flag) yield break;
            string url = ConstData.WebRequestLocalFileHead + filePath;
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    yield break;
                }
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                texture.name = Path.GetFileNameWithoutExtension(filePath);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                sprite.name = texture.name;
                spriteList.Add(sprite);
            }
        }
    }
}