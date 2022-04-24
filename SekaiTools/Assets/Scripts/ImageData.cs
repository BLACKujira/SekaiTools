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
    public class ImageData : IData<Sprite>
    {
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        Dictionary<Sprite, string> paths = new Dictionary<Sprite, string>();

        public ImageData(string savePath)
        {
            this.savePath = savePath;
        }

        public Sprite[] valueArray 
        {
            get
            {
                List<Sprite> sprites = new List<Sprite>();
                foreach (var keyValuePair in this.sprites)
                {
                    sprites.Add(keyValuePair.Value);
                }
                return sprites.ToArray();
            }
        }

        public string savePath { get; set; }

        public IEnumerator LoadFile(params string[] files)
        {
            foreach (var file in files)
            {
                yield return LoadFile(file);
            }
        }
        public IEnumerator LoadFile(SerializedImageData serializedImageData)
        {
            for (int i = 0; i < serializedImageData.items.Count; i++)
            {
                yield return LoadFile(serializedImageData.items[i]);
            }
        }
        public IEnumerator LoadFile(string file, string name = null)
        {
            string extension = Path.GetExtension(file);
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
            string url = ConstData.WebRequestLocalFileHead + file;
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    yield break;
                }
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                texture.name = Path.GetFileNameWithoutExtension(file);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                sprite.name = texture.name;
                sprites[sprite.name] = sprite;
                paths[sprite] = file;
            }
        }
        IEnumerator LoadFile(SerializedImageData.DataItem imageDataItem)
        {
            yield return LoadFile(imageDataItem.path, imageDataItem.name);
        }

        public Sprite GetValue(string name)
        {
            if (string.IsNullOrEmpty(name) || !sprites.ContainsKey(name)) return null;
            return sprites[name];
        }

        public bool RemoveValue(string name)
        {
            if (!sprites.ContainsKey(name)) return false;
            paths.Remove(sprites[name]);
            sprites.Remove(name);
            return true;
        }

        public bool RemoveValue(Sprite value)
        {
            if (!sprites.ContainsValue(value)) return false;
            paths.Remove(value);
            sprites.Remove(value.name);
            return true;
        }

        public bool AppendValue(Sprite value, string savePath = null)
        {
            if (sprites.ContainsKey(value.name)) return false;
            sprites[value.name] = value;
            paths[value] = savePath;
            return true;
        }

        public string GetSavePath(Sprite value)
        {
            if (!paths.ContainsKey(value)) return null;
            return paths[value];
        }

        public IEnumerator LoadData(string serializedData)
        {
            SerializedImageData data = JsonUtility.FromJson<SerializedImageData>(serializedData);
            yield return LoadFile(data);
        }

        public void SaveData()
        {
            SerializedImageData serializedImageData = new SerializedImageData(paths);
            string json = JsonUtility.ToJson(serializedImageData, true);
            File.WriteAllText(savePath, json);
        }

        [System.Serializable]
        public class SerializedImageData
        {
            [System.Serializable]
            public class DataItem
            {
                public string name;
                public string path;

                public DataItem(KeyValuePair<Sprite, string> keyValuePair)
                {
                    this.name = keyValuePair.Key.name;
                    this.path = keyValuePair.Value;
                }

                public DataItem(string name, string path)
                {
                    this.name = name;
                    this.path = path;
                }
            }
            public List<DataItem> items = new List<DataItem>();

            public SerializedImageData(Dictionary<Sprite, string> items)
            {
                this.items = new List<DataItem>();
                foreach (var keyValuePair in items)
                {
                    this.items.Add(new DataItem(keyValuePair));
                }
            }
        }
    }
}