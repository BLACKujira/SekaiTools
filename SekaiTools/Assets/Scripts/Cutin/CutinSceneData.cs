using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools;
using System;
using System.IO;
using SekaiTools.Kizuna;

namespace SekaiTools.Cutin
{
    /// <summary>
    /// 互动语音场景资料
    /// </summary>
    [Serializable]
    public class CutinSceneData : ISaveData
    {
        public List<CutinScene> cutinScenes = new List<CutinScene>();

        public string SavePath { get; set; }

        /// <summary>
        /// 判断一个文件的名字是否符合互动语音的格式(不支持早期不分前后的语音)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CutinSceneInfo IsCutinVoice(string name)
        {
            int charFirstID;
            int charSecondID;
            string[] nameArray = name.Split('_');
            if (nameArray.Length < 6) return null;
            if (!nameArray[0].Equals("system")) return null;
            if (!nameArray[1].Equals("bondscp")) return null;
            if (!int.TryParse(nameArray[2], out charFirstID)) return null;
            if (!int.TryParse(nameArray[3], out charSecondID)) return null;
            if (!nameArray[4].Equals("first") && !nameArray[4].Equals("second")) return null;
            if (nameArray[5][0]<'1'|| nameArray[5][0]>'9') return null;
            return new CutinSceneInfo(
                charFirstID,
                charSecondID, 
                nameArray[5][0] - '0',
                nameArray[4].Equals("first")?CutinSceneInfo.ClipType.first:CutinSceneInfo.ClipType.second);
        }

        /// <summary>
        /// 使用cutinSceneInfos生成互动语音场景，音频文件的名称为标准化的名称
        /// </summary>
        /// <param name="cutinSceneInfos"></param>
        public CutinSceneData(params CutinSceneInfoBase[] cutinSceneInfos)
        {
            foreach (var cutinSceneInfo in cutinSceneInfos)
            {
                bool flag = false;
                foreach (var cutinSceneItem in cutinScenes)
                {
                    if(cutinSceneInfo.IsInScene(cutinSceneItem))
                    {
                        flag = true;
                        break;
                    }
                }

                if(!flag)
                {
                    CutinScene cutinScene = new CutinScene(cutinSceneInfo);
                    cutinScene.talkData_First.talkVoice = StandardizeName(new CutinSceneInfo(cutinSceneInfo, CutinSceneInfo.ClipType.first));
                    cutinScene.talkData_Second.talkVoice = StandardizeName(new CutinSceneInfo(cutinSceneInfo, CutinSceneInfo.ClipType.second));
                    cutinScenes.Add(cutinScene);
                }
            }
        }

        /// <summary>
        /// 构造函数，生成只含有一个片段的互动语音场景资料(用于编辑器的预览)
        /// </summary>
        /// <param name="cutinScene"></param>
        public CutinSceneData(CutinScene cutinScene)
        {
            cutinScenes.Add(cutinScene);
        }

        public CutinSceneData(List<CutinScene> cutinScenes)
        {
            this.cutinScenes = cutinScenes;
        }

        /// <summary>
        /// 统计此互动语音场景资料中出现的所有角色
        /// </summary>
        /// <returns></returns>
        public int[] CountAppearCharacters()
        {
            HashSet<int> appearCharacters = new HashSet<int>();
            foreach (var cutinScene in cutinScenes)
            {
                appearCharacters.Add(cutinScene.charFirstID);
                appearCharacters.Add(cutinScene.charSecondID);
            }

            List<int> list = new List<int>(appearCharacters);
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 统计语音匹配与缺失
        /// </summary>
        /// <param name="audioData"></param>
        /// <returns></returns>
        public AudioMatchingCount CountMatching(AudioData audioData)
        {
            List<string[]> names = new List<string[]>();
            AudioClip[] valueArray = audioData.valueArray;
            foreach (var audioClip in valueArray)
            {
                if (IsCutinVoice(audioClip.name) != null)
                    names.Add(audioClip.name.Split('_'));
            }
            AudioMatchingCount audioMatchingCount = new AudioMatchingCount();
            foreach (var cutinScene in cutinScenes)
            {
                string idFirst = cutinScene.charFirstID.ToString("000");
                string idSecond = cutinScene.charSecondID.ToString("000");
                bool existFirst = false;
                bool existSecond = false;
                foreach (var nameArray in names)
                {
                    if(nameArray[2].Equals(idFirst)&&nameArray[3].Equals(idSecond)&&nameArray[5][0] - '0' == cutinScene.dataID)
                    {
                        if (nameArray[4].Equals("first")) existFirst = true;
                        if (nameArray[4].Equals("second")) existSecond = true;
                    }
                }
                if(existFirst)
                {
                    if (existSecond) audioMatchingCount.matching++;
                    else audioMatchingCount.missingSecond++;
                }
                else
                {
                    if (existSecond) audioMatchingCount.missingFirst++;
                    else audioMatchingCount.missingBoth++;
                }
            }
            return audioMatchingCount;
        }

        public class AudioMatchingCount
        {
            public int matching = 0;
            public int missingFirst = 0;
            public int missingSecond = 0;
            public int missingBoth = 0;

            public int countAll { get => matching + missingFirst + missingSecond + missingBoth; }

            public static AudioMatchingCount operator+(AudioMatchingCount a,AudioMatchingCount b)
            {
                AudioMatchingCount audioMatchingCount = new AudioMatchingCount();
                audioMatchingCount.matching = a.matching + b.matching;
                audioMatchingCount.missingFirst = a.missingFirst + b.missingFirst;
                audioMatchingCount.missingBoth = a.missingBoth + b.missingBoth;
                audioMatchingCount.missingSecond = a.missingSecond + b.missingSecond;
                return audioMatchingCount;
            }
        }

        public class CutinSceneInfoBase
        {
            public int charFirstID;
            public int charSecondID;
            public int dataID;

            public CutinSceneInfoBase(int charFirstID, int charSecondID, int dataID)
            {
                this.charFirstID = charFirstID;
                this.charSecondID = charSecondID;
                this.dataID = dataID;
            }

            public bool IsConversationOf(int charAID, int charBID,bool mergeVirtualSinger = false)
            {
                if (mergeVirtualSinger)
                {
                    if (ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(charFirstID)
                        && ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(charSecondID)) return true;
                    if (ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(charSecondID) 
                        && ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(charFirstID)) return true;
                }
                else
                {
                    if (charAID == charFirstID && charBID == charSecondID) return true;
                    if (charAID == charSecondID && charBID == charFirstID) return true;
                }
                return false;
            }
            public bool IsSameConversation(CutinSceneInfo another)
            {
                if (charFirstID != another.charFirstID) return false;
                if (charSecondID != another.charSecondID) return false;
                if (dataID != another.dataID) return false;
                return true;
            }

            public bool IsInScene(CutinScene cutinScene)
            {
                if (charFirstID != cutinScene.charFirstID) return false;
                if (charSecondID != cutinScene.charSecondID) return false;
                if (dataID != cutinScene.dataID) return false;
                return true;
            }
        }

        public class CutinSceneInfo:CutinSceneInfoBase
        {
            public enum ClipType { first, second }
            public ClipType clipType;

            public CutinSceneInfo(int charFirstID, int charSecondID, int dataID, ClipType clipType):base(charFirstID,charSecondID,dataID)
            {
                this.clipType = clipType;
            }
            public CutinSceneInfo(CutinSceneInfoBase cutinSceneInfoBase, ClipType clipType) : base(cutinSceneInfoBase.charFirstID, cutinSceneInfoBase.charSecondID, cutinSceneInfoBase.dataID)
            {
                this.clipType = clipType;
            }
        }

        public static string StandardizeName(CutinSceneInfo cutinSceneInfo)
        {
            return $"system_bondscp_{cutinSceneInfo.charFirstID.ToString("000")}_{cutinSceneInfo.charSecondID.ToString("000")}_{cutinSceneInfo.clipType}_{cutinSceneInfo.dataID}";
        }

        public void StandardizeAudioData(AudioData audioData)
        {
            AudioClip[] valueArray = audioData.valueArray;
            foreach (var value in valueArray)
            {
                CutinSceneInfo cutinSceneInfo = IsCutinVoice(value.name);

                string savePath = audioData.GetSavePath(value);
                audioData.RemoveValue(value);

                foreach (var cutinScene in cutinScenes)
                {
                    if (cutinScene.IsConversationOf(cutinSceneInfo.charFirstID, cutinSceneInfo.charSecondID))
                    {
                        value.name = StandardizeName(cutinSceneInfo);
                        audioData.AppendValue(value, savePath);
                        break;
                    }
                }
            }
        }

        public virtual void SaveData()
        {
            string json = JsonUtility.ToJson(this,true);
            File.WriteAllText(SavePath, json);
        }

        public CutinSceneData LoadData(string savePath)
        {
            string json = File.ReadAllText(savePath);
            CutinSceneData cutinSceneData = JsonUtility.FromJson<CutinSceneData>(json);
            cutinSceneData.SavePath = savePath;
            return cutinSceneData;
        }
    }
    public class CutinSceneDataInKizunaData:CutinSceneData
    {
        public Kizuna.KizunaSceneDataBase kizunaSceneData;

        public CutinSceneDataInKizunaData(List<CutinScene> cutinScenes, KizunaSceneDataBase kizunaSceneData) : base(cutinScenes)
        {
            this.kizunaSceneData = kizunaSceneData;
        }

        public override void SaveData()
        {
            kizunaSceneData[new Vector2Int(cutinScenes[0].charFirstID, cutinScenes[0].charSecondID)].cutinScenes = cutinScenes;
            kizunaSceneData.SaveData();
        }
    }
}