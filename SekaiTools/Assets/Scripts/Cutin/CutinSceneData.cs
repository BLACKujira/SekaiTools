using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools;
using System;

namespace SekaiTools.Cutin
{
    /// <summary>
    /// 互动语音场景资料
    /// </summary>
    [Serializable]
    public class CutinSceneData
    {
        public List<CutinScene> cutinScenes = new List<CutinScene>();

        /// <summary>
        /// 判断一个文件的名字是否符合互动语音的格式(不支持早期不分前后的语音)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsCutinVoice(string name)
        {
            string[] nameArray = name.Split('_');
            if (nameArray.Length < 6) return false;
            if (!nameArray[0].Equals("system")) return false;
            if (!nameArray[1].Equals("bondscp")) return false;
            if (!int.TryParse(nameArray[2], out _)) return false;
            if (!int.TryParse(nameArray[3], out _)) return false;
            if (!nameArray[4].Equals("first") && !nameArray[4].Equals("second")) return false;
            if (nameArray[5][0]<'1'|| nameArray[5][0]>'9') return false;
            return true;
        }

        /// <summary>
        /// 构造函数，从一个语音资料中挑选出符合互动语音名称的片段并生成互动语音场景
        /// </summary>
        /// <param name="audioData"></param>
        public CutinSceneData(AudioData audioData)
        {
            List<((string, string[]) first, (string, string[]) second)> Pairing(List<(string, string[])> _fileNameData_first, List<(string, string[])> _fileNameData_second)
            {
                List<((string, string[]) first, (string, string[]) second)> outList = new List<((string, string[]) first, (string, string[]) second)>();
                foreach (var file_F in _fileNameData_first)
                {
                    foreach (var file_S in _fileNameData_second)
                    {
                        if (file_F.Item2[2].Equals(file_S.Item2[2]) &&
                            file_F.Item2[3].Equals(file_S.Item2[3]) &&
                            file_F.Item2[5][0].Equals(file_S.Item2[5][0]))
                        {
                            outList.Add((file_F, file_S));
                            break;
                        }
                    }
                }
                return outList;
            }
            List<(string, string[])> fileNameData_first = new List<(string, string[])>();
            List<(string, string[])> fileNameData_second = new List<(string, string[])>();
            foreach (var keyValuePair in audioData.audioClips)
            {
                string name = keyValuePair.Key;
                if (!IsCutinVoice(name)) continue;
                    string[] array = name.Split('_');
                if (array[4].Equals("second"))
                    fileNameData_second.Add((name, array));
                else
                    fileNameData_first.Add((name, array));
            }
            List<((string, string[]) first, (string, string[]) second)> list = Pairing(fileNameData_first, fileNameData_second);
            foreach (var item in list)
            {
                CutinScene data = new CutinScene();
                data.charFirstID = Convert.ToInt32(item.first.Item2[2]);
                data.charSecondID = Convert.ToInt32(item.first.Item2[3]);
                data.talkData_First.talkVoice = item.first.Item1;
                data.talkData_Second.talkVoice = item.second.Item1;
                data.dataID = item.first.Item2[5][0] - '0';

                cutinScenes.Add(data);
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
            foreach (var keyValuePair in audioData.audioClips)
            {
                if (IsCutinVoice(keyValuePair.Key))
                    names.Add(keyValuePair.Key.Split('_'));
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
        }
    }
}