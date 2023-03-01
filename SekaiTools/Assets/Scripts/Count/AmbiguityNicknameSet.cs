using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.Count
{
    [System.Serializable]
    public class AmbiguityNicknameSet : ISaveData
    {
        public List<string> ambiguityRegices;

        public string SavePath { get; set; }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
        }

        public AmbiguityNicknameSet Clone()
        {
            AmbiguityNicknameSet ambiguityNicknameSet = new AmbiguityNicknameSet();
            ambiguityNicknameSet.ambiguityRegices = new List<string>(ambiguityRegices);
            return ambiguityNicknameSet;
        }

        public void ReplaceValue(AmbiguityNicknameSet fromSet)
        {
            ambiguityRegices = fromSet.ambiguityRegices;
        }

        public static AmbiguityNicknameSet LoadData(string savePath)
        {
            string data = File.ReadAllText(savePath);
            AmbiguityNicknameSet nicknameSet = JsonUtility.FromJson<AmbiguityNicknameSet>(data);
            nicknameSet.SavePath = savePath;
            return nicknameSet;
            ;
        }
    }
}