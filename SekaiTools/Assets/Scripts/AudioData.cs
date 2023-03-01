using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace SekaiTools
{
    public interface ISaveData
    {
        string SavePath { get; set; }
        void SaveData();
    }
    public interface IData<T>
    {
        string SavePath { get; set; }
        void SaveData();
        T[] ValueArray { get; }
        string[] AbstractValueArray { get; }
        KeyValuePair<T, string>[] ValuePathPairArray { get; }
        KeyValuePair<string, string>[] AbstractValuePathPairArray { get; }
        T GetValue(string name);
        bool RemoveValue(string name);
        /// <summary>
        /// 不包括抽象文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ContainsValue(string name);
        bool ContainsAbstractValue(string name);
        bool RemoveValue(T value);
        bool AppendValue(T value, string savePath = null);
        bool AppendAbstractValue(string value, string savePath = null);
        IEnumerator LoadData(string serializedData);
        int RemoveUnusedValue(IEnumerable<string> usedKeys);
    }

    /// <summary>
    /// 声音资料
    /// </summary>
    [System.Serializable]
    public class AudioData : IData<AudioClip>
    {
        public string SavePath { get; set; }

        public AudioData(string savePath)
        {
            this.SavePath = savePath;
        }
        public AudioData()
        {
            this.SavePath = null;
        }
        public AudioData(IData<AudioClip> data)
        {
            SavePath = data.SavePath;
            AudioClip[] valueArray = data.ValueArray;
            foreach (var value in valueArray)
            {
                AppendValue(value, data.SavePath);
            }
        }

        public AudioClip[] ValueArray
        {
            get
            {
                if (this.audioClips == null) return null;
                List<AudioClip> audioClips = new List<AudioClip>();
                foreach (var keyValuePair in this.audioClips)
                {
                    audioClips.Add(keyValuePair.Value);
                }
                return audioClips.ToArray();
            }
        }

        public KeyValuePair<AudioClip, string>[] ValuePathPairArray => new List<KeyValuePair<AudioClip, string>>(paths).ToArray();

        public KeyValuePair<string, string>[] AbstractValuePathPairArray => new List<KeyValuePair<string, string>>(abstractValues).ToArray();

        public string[] AbstractValueArray => abstractValues.Select(kvp => kvp.Key).ToArray();

        Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        Dictionary<AudioClip, string> paths = new Dictionary<AudioClip, string>();
        Dictionary<string, string> abstractValues = new Dictionary<string, string>();

        /// <summary>
        /// 从声音资料存档中读取声音资料
        /// </summary>
        /// <param name="serializedAudioData"></param>
        /// <returns></returns>
        public IEnumerator LoadData(string serializedData)
        {
            SerializedAudioData data = JsonUtility.FromJson<SerializedAudioData>(serializedData);
            yield return LoadFile(data);
        }

        /// <summary>
        /// 将files中找到的所有音频读取到此音频资料中，需要借助组件启动协程
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public IEnumerator LoadFile(params string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                yield return LoadFile(files[i]);
            }
        }
        public IEnumerator LoadFile(SerializedAudioData serializedAudioData)
        {
            for (int i = 0; i < serializedAudioData.items.Count; i++)
            {
                yield return LoadFile(serializedAudioData.items[i]);
            }
        }

        /// <summary>
        /// 读取单个音频文件到此音频资料中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEnumerator LoadFile(string file, string name = null)
        {
            string extension = Path.GetExtension(file);
            extension = extension.ToLower();
            AudioType? audioType = null;
            switch (extension)
            {
                case ".ogg":
                    audioType = AudioType.OGGVORBIS;
                    break;
                case ".mp3":
                    audioType = AudioType.MPEG;
                    break;
                case ".wav":
                    audioType = AudioType.WAV;
                    break;
            }
            if (audioType == null) yield break;
            if (File.Exists(file))
            {
                string url = ConstData.WebRequestLocalFileHead + file;
                using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(url, (AudioType)audioType))
                {
                    yield return webRequest.SendWebRequest();
                    if (webRequest.isHttpError || webRequest.isNetworkError)
                    {
                        yield break;
                    }
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
                    audioClip.name = string.IsNullOrEmpty(name) ? Path.GetFileNameWithoutExtension(file) : name;
                    audioClips[audioClip.name] = audioClip;
                    paths[audioClip] = file;
                }
            }
            else
            {
                abstractValues[name] = file;
            }
        }
        IEnumerator LoadFile(SerializedAudioData.DataItem audioDataItem)
        {
            yield return LoadFile(audioDataItem.path, audioDataItem.name);
        }

        /// <summary>
        /// 在此音频资料中寻找对应名称的音频片段，若未找到则返回null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AudioClip GetValue(string name)
        {
            if (string.IsNullOrEmpty(name) || !audioClips.ContainsKey(name)) return null;
            return audioClips[name];
        }

        /// <summary>
        /// 对抽象文件也有效
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveValue(string name)
        {
            if (audioClips.ContainsKey(name))
            {
                paths.Remove(audioClips[name]);
                audioClips.Remove(name);
                return true;
            }
            if (abstractValues.ContainsKey(name))
            {
                abstractValues.Remove(name);
                return true;
            }
            return false;
        }

        public bool RemoveValue(AudioClip value)
        {
            if (!audioClips.ContainsValue(value)) return false;
            audioClips.Remove(value.name);
            paths.Remove(value);
            return true;
        }

        /// <summary>
        /// 保存音频资料存档到path
        /// </summary>
        /// <param name="path"></param>
        public void SaveData()
        {
            SerializedAudioData serializedAudioData = new SerializedAudioData(paths, abstractValues);
            string json = JsonUtility.ToJson(serializedAudioData, true);
            File.WriteAllText(SavePath, json);
        }

        public bool AppendValue(AudioClip value, string savePath = null)
        {
            if (audioClips.ContainsKey(value.name)) return false;
            if (abstractValues.ContainsKey(value.name)) abstractValues.Remove(value.name);
            audioClips[value.name] = value;
            paths[value] = savePath;
            return true;
        }

        public string GetSavePath(AudioClip value)
        {
            if (!paths.ContainsKey(value)) return null;
            return paths[value];
        }

        public void Append(IData<AudioClip> data)
        {
            AudioClip[] valueArray = data.ValueArray;
            foreach (var value in valueArray)
            {
                AppendValue(value, data.SavePath);
            }
        }

        public static SerializedAudioData DeSerializeSaveData(string serializedData)
        {
            return JsonUtility.FromJson<SerializedAudioData>(serializedData);
        }

        public static string ReSerializeSaveData(SerializedAudioData serializedAudioData)
        {
            return JsonUtility.ToJson(serializedAudioData);
        }

        public bool ContainsValue(string name)
        {
            return audioClips.ContainsKey(name);
        }

        public int RemoveUnusedValue(IEnumerable<string> keys)
        {
            int count = 0;
            AudioClip[] valueArray = this.ValueArray;
            foreach (var audioClip in valueArray)
            {
                if (!keys.Contains(audioClip.name))
                {
                    RemoveValue(audioClip);
                    count++;
                }
            }
            string[] oldAbstractValues = abstractValues.Select(value => value.Key).ToArray();
            foreach (var key in oldAbstractValues)
            {
                if (!keys.Contains(key))
                {
                    RemoveValue(key);
                    count++;
                }
            }
            return count;
        }

        public bool AppendAbstractValue(string value, string savePath = null)
        {
            if (abstractValues.ContainsKey(value) || audioClips.ContainsKey(value)) return false;
            abstractValues[value] = savePath;
            return true;
        }

        public bool ContainsAbstractValue(string name)
        {
            return abstractValues.ContainsKey(name);
        }

        [Serializable]
        public class NameDuplicateException : System.Exception
        {
            public NameDuplicateException() { }
            public NameDuplicateException(string message) : base(message) { }
            public NameDuplicateException(string message, System.Exception inner) : base(message, inner) { }
            protected NameDuplicateException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }

    /// <summary>
    /// 音频资料存档，保存每一段音频的位置
    /// </summary>
    [System.Serializable]
    public class SerializedAudioData
    {
        [System.Serializable]
        public class DataItem
        {
            public string name;
            public string path;

            public DataItem(KeyValuePair<AudioClip, string> keyValuePair)
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

        public SerializedAudioData(Dictionary<AudioClip, string> items, Dictionary<string, string> abstractValues)
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

        public SerializedAudioData(Dictionary<string, string> items)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair.Key, keyValuePair.Value));
            }
        }

        public SerializedAudioData(string[] filePaths)
        {
            this.items = new List<DataItem>();
            foreach (var path in filePaths)
            {
                items.Add(new DataItem(Path.GetFileNameWithoutExtension(path), path));
            }
        }

        public SerializedAudioData()
        {
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

        public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in items)
            {
                dictionary[item.name] = item.path;
            }
            return dictionary;
        }

        public MediaMatchInfo GetAudioMatchInfo(IEnumerable<string> requireKeys)
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

        public static SerializedAudioData LoadData(string serializedData)
        {
            return JsonUtility.FromJson<SerializedAudioData>(serializedData);
        }
    }

    public class MediaMatchInfo
    {
        public int matchcing = 0;
        public int missingKey = 0;
        public int missingFile = 0;

        public int Total => matchcing + missingKey + missingFile;
        public string Description => $"{matchcing}个文件匹配,{missingKey}个文件没有记录,{missingFile}个文件丢失";
    }

    [System.Serializable]
    public class SerializedData<T> where T : UnityEngine.Object
    {
        [System.Serializable]
        public class DataItem
        {
            public string name;
            public string path;

            public DataItem(KeyValuePair<T, string> keyValuePair)
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

        public SerializedData(Dictionary<T, string> items)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair));
            }
        }

        public SerializedData()
        {
        }
    }
}
