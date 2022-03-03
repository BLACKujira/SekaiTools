using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;

namespace SekaiTools
{
    /// <summary>
    /// 声音资料
    /// </summary>
    [System.Serializable]
    public class AudioData
    {
        public string savePath;
        public List<AudioClip> audioClipList = new List<AudioClip>();
        public Dictionary<string, AudioClip> audioClips;
        Dictionary<string,string> paths;

        /// <summary>
        /// 从声音资料存档中读取声音资料
        /// </summary>
        /// <param name="serializedAudioData"></param>
        /// <returns></returns>
        public IEnumerator LoadAudioData(string serializedAudioData)
        {
            SerializedAudioData data = JsonUtility.FromJson<SerializedAudioData>(File.ReadAllText(serializedAudioData));
            savePath = serializedAudioData;
            yield return LoadDatas(data.paths.ToArray());
        }

        /// <summary>
        /// 将files中找到的所有音频读取到此音频资料中，需要借助组件启动协程
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public IEnumerator LoadDatas(string[] files)
        {
            paths = new Dictionary<string, string>();
            for (int i = 0; i < files.Length; i++)
            {
                yield return LoadData(files[i]);
            }
            audioClips = new Dictionary<string, AudioClip>();
            foreach (var audioClip in audioClipList)
            {
                audioClips[audioClip.name] = audioClip;
            }
        }

        /// <summary>
        /// 读取单个音频文件到此音频资料中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerator LoadData(string file)
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
                audioClip.name = Path.GetFileNameWithoutExtension(file);
                audioClipList.Add(audioClip);
                paths[audioClip.name] = file;
            }
        }

        /// <summary>
        /// 在此音频资料中寻找对应名称的音频片段，若未找到则返回null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AudioClip GetAudioClip(string name)
        {
            if (string.IsNullOrEmpty(name)||!audioClips.ContainsKey(name)) return null;
            return audioClips[name];
        }

        /// <summary>
        /// 音频资料存档，保存每一段音频的位置
        /// </summary>
        [System.Serializable]
        public class SerializedAudioData
        {
            public List<string> paths;

            public SerializedAudioData(Dictionary<string, string> paths)
            {
                this.paths = new List<string>();
                foreach (var keyValuePair in paths)
                {
                    this.paths.Add(keyValuePair.Value);
                }
            }
        }

        /// <summary>
        /// 保存音频资料存档到path
        /// </summary>
        /// <param name="path"></param>
        public void SaveData(string path)
        {
            SerializedAudioData serializedAudioData = new SerializedAudioData(paths);
            string json = JsonUtility.ToJson(serializedAudioData,true);
            File.WriteAllText(path, json);
        }
    }
    
}
