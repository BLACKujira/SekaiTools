using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;

namespace SekaiTools
{
    public interface ISaveData
    {
        string SavePath { get; set; }
        void SaveData();
    }
    public interface IData<T>
    {
        string savePath { get; set; }
        void SaveData();
        T[] valueArray { get; }
        KeyValuePair<T,string>[] valuePathPairArray { get; }
        T GetValue(string name);
        bool RemoveValue(string name);
        bool ContainsValue(string name);
        bool RemoveValue(T value);
        bool AppendValue(T value, string savePath = null);
        IEnumerator LoadData(string serializedData);
    }

    /// <summary>
    /// 声音资料
    /// </summary>
    [System.Serializable]
    public class AudioData : IData<AudioClip>
    {
        public string savePath { get; set; }

        public AudioData(string savePath)
        {
            this.savePath = savePath;
        }
        public AudioData()
        {
            this.savePath = null;
        }
        public AudioData(IData<AudioClip> data)
        {
            savePath = data.savePath;
            AudioClip[] valueArray = data.valueArray;
            foreach (var value in valueArray)
            {
                AppendValue(value, data.savePath);
            }
        }

        public AudioClip[] valueArray
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

        public KeyValuePair<AudioClip, string>[] valuePathPairArray
        {
            get
            {
                return new List<KeyValuePair<AudioClip, string>>(paths).ToArray();
            }
        }

        Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        Dictionary<AudioClip,string> paths = new Dictionary<AudioClip, string>();

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
        IEnumerator LoadFile(string file,string name = null)
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
            string url = ConstData.WebRequestLocalFileHead + file;
            using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(url, (AudioType)audioType))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    yield break;
                }
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
                audioClip.name = string.IsNullOrEmpty(name)? Path.GetFileNameWithoutExtension(file):name;
                audioClips[audioClip.name] = audioClip;
                paths[audioClip] = file;
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
            if (string.IsNullOrEmpty(name)||!audioClips.ContainsKey(name)) return null;
            return audioClips[name];
        }

        public bool RemoveValue(string name)
        {
            if (!audioClips.ContainsKey(name)) return false;
            paths.Remove(audioClips[name]);
            audioClips.Remove(name);
            return true;
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
            SerializedAudioData serializedAudioData = new SerializedAudioData(paths);
            string json = JsonUtility.ToJson(serializedAudioData,true);
            File.WriteAllText(savePath, json);
        }

        public bool AppendValue(AudioClip value, string savePath = null)
        {
            if (audioClips.ContainsKey(value.name)) return false;
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
            AudioClip[] valueArray = data.valueArray;
            foreach (var value in valueArray)
            {
                AppendValue(value,data.savePath);
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

        public SerializedAudioData(Dictionary<AudioClip, string> items)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair));
            }
        }

        public SerializedAudioData(Dictionary<string, string> items)
        {
            this.items = new List<DataItem>();
            foreach (var keyValuePair in items)
            {
                this.items.Add(new DataItem(keyValuePair.Key,keyValuePair.Value));
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
    }

    [System.Serializable]
    public class SerializedData<T> where T:UnityEngine.Object
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
