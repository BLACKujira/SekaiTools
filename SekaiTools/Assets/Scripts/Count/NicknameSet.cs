using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.Count
{
    [System.Serializable]
    public class NicknameSet : ISaveData
    {
        public NicknameItem[] nicknameItems;

        public string SavePath { get; set; }

        [System.Serializable]
        public class NicknameItem
        {
            public List<string> nickNames = new List<string>();

            public NicknameItem Clone()
            {
                NicknameItem nicknameItem = new NicknameItem();
                nicknameItem.nickNames = new List<string>(nickNames);
                return nicknameItem;
            }
        }

        public NicknameSet()
        {
            nicknameItems = new NicknameItem[27];
            for (int i = 1; i < 27; i++)
            {
                nicknameItems[i] = new NicknameItem();
            }
        }

        public void ReplaceValue(NicknameSet from)
        {
            for (int i = 1; i < 27; i++)
            {
                nicknameItems[i] = from.nicknameItems[i];
            }
        }

        public NicknameSet Clone()
        {
            NicknameSet nicknameSet = new NicknameSet();
            for (int i = 1; i < 27; i++)
            {
                nicknameSet.nicknameItems[i] = nicknameItems[i].Clone();
            }
            return nicknameSet;
        }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this,true);
            File.WriteAllText(SavePath, json);
        }

        public static NicknameSet LoadData(string savePath)
        {
            string data = File.ReadAllText(savePath);
            NicknameSet nicknameSet = JsonUtility.FromJson<NicknameSet>(data);
            nicknameSet.SavePath = savePath;
            return nicknameSet;
;        }

        public static NicknameSet operator +(NicknameSet a,NicknameSet b)
        {
            NicknameSet nicknameSet = a.Clone();
            for (int i = 1; i < nicknameSet.nicknameItems.Length; i++)
            {
                foreach (var str in b.nicknameItems[i].nickNames)
                {
                    nicknameSet.nicknameItems[i].nickNames.Add(str);
                }
            }
            return nicknameSet;
        }
    }
}