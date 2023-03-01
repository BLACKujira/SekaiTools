using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class SysL2DShowData : ISaveData
    {
        public List<SysL2DShow> sysL2DShows;
        public int[] AppearCharacters
        {
            get
            {
                HashSet<int> charIds = new HashSet<int>();
                foreach (var sysL2DShow in sysL2DShows)
                {
                    charIds.Add(sysL2DShow.systemLive2D.CharacterId);
                }
                return new List<int>(charIds).ToArray();
            }
        }

        public SysL2DShowData(List<SysL2DShow> sysL2DShows)
        {
            this.sysL2DShows = sysL2DShows;
        }

        public string SavePath { get; set; }

        public void SaveData()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(this,true));
        }

        public class AudioMatchInfo
        {
            public int matchingCount = 0;
            public int missingCount = 0;
        }

        [System.Obsolete]
        public AudioMatchInfo GetAudioMatchInfo(SerializedAudioData serializedAudioData)
        {
            AudioMatchInfo audioMatchInfo = new AudioMatchInfo();
            foreach (var sysL2DShow in sysL2DShows)
            {
                string key = $"{sysL2DShow.systemLive2D.AssetbundleName}-{sysL2DShow.systemLive2D.Voice}";
                if (serializedAudioData.HasKey(key))
                    audioMatchInfo.matchingCount++;
                else
                    audioMatchInfo.missingCount++;
            }
            return audioMatchInfo;
        }
    }
}