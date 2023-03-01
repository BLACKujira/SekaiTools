using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Dictionary<string, string> abstractValues = new Dictionary<string, string>();

        public ImageData()
        {
        }

        public ImageData(string savePath)
        {
            this.SavePath = savePath;
        }

        public Sprite[] ValueArray
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

        public string SavePath { get; set; }

        public KeyValuePair<Sprite, string>[] ValuePathPairArray => new List<KeyValuePair<Sprite, string>>(paths).ToArray();

        public KeyValuePair<string, string>[] AbstractValuePathPairArray => new List<KeyValuePair<string, string>>(abstractValues).ToArray();

        public string[] AbstractValueArray => abstractValues.Select(kvp => kvp.Key).ToArray();

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
            if (File.Exists(file))
            {
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
                    sprite.name = string.IsNullOrEmpty(name) ? texture.name : name;
                    sprites[sprite.name] = sprite;
                    paths[sprite] = file;
                }
            }
            else
            {
                abstractValues[name] = file;
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
            if (sprites.ContainsKey(name))
            {
                paths.Remove(sprites[name]);
                sprites.Remove(name);
                return true;
            }
            if (abstractValues.ContainsKey(name))
            {
                abstractValues.Remove(name);
                return true;
            }
            return false;
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
            if (abstractValues.ContainsKey(value.name)) abstractValues.Remove(value.name);
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
            SerializedImageData serializedImageData = new SerializedImageData(paths, abstractValues);
            string json = JsonUtility.ToJson(serializedImageData, true);
            File.WriteAllText(SavePath, json);
        }

        public bool ContainsValue(string name)
        {
            return sprites.ContainsKey(name);
        }

        public int RemoveUnusedValue(IEnumerable<string> keys)
        {
            int count = 0;
            Sprite[] valueArray = this.ValueArray;
            foreach (var sprite in valueArray)
            {
                if (!keys.Contains(sprite.name))
                {
                    RemoveValue(sprite);
                    count++;
                }
            }
            string[] oldAbstractValues = abstractValues.Select(value => value.Key).ToArray();
            foreach (var key in oldAbstractValues)
            {
                if(!keys.Contains(key)) 
                {
                    RemoveValue(key);
                    count++;
                }
            }
            return count;
        }

        public bool AppendAbstractValue(string value, string savePath = null)
        {
            if (abstractValues.ContainsKey(value) || sprites.ContainsKey(value)) return false;
            abstractValues[value] = savePath;
            return true;
        }

        public bool ContainsAbstractValue(string name)
        {
            return abstractValues.ContainsKey(name);
        }
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

        public SerializedImageData(Dictionary<Sprite, string> items, Dictionary<string, string> abstractValues)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair));
            }
            foreach (var keyValuePair in abstractValues)
            {
                this.items.Add(new DataItem(keyValuePair.Key, keyValuePair.Value));
            }
        }

        public SerializedImageData(Dictionary<string, string> items)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair.Key, keyValuePair.Value));
            }
        }

        public bool HasKey(string key)
        {
            foreach (var item in items)
            {
                if (item.name.Equals(key))
                    return true;
            }
            return false;
        }

        public static SerializedImageData LoadData(string serializedData)
        {
            return JsonUtility.FromJson<SerializedImageData>(serializedData);
        }

        public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in items)
            {
                dictionary[item.name] = item.path;
            }
            return dictionary;
        }

        public MediaMatchInfo GetImageMatchInfo(IEnumerable<string> requireKeys)
        {
            Dictionary<string, string> dictionary = GetDictionary();
            MediaMatchInfo audioMatchInfo = new MediaMatchInfo();
            foreach (var key in requireKeys)
            {
                if (dictionary.ContainsKey(key))
                {
                    if (File.Exists(dictionary[key]))
                    {
                        audioMatchInfo.matchcing++;
                    }
                    else
                    {
                        audioMatchInfo.missingFile++;
                    }
                }
                else
                {
                    audioMatchInfo.missingKey++;
                }
            }
            return audioMatchInfo;
        }
    }
}