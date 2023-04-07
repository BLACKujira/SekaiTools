using SekaiTools.Count;
using SekaiTools.UI.Downloader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace SekaiTools.UI.NicknameCounter
{
    public class NicknameCounter : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NicknameCounter_CountPreviewArea countPreviewArea;
        public Downloader_PerecntBar perecntBar;

        [System.NonSerialized] public NicknameCountData nicknameCountData = new NicknameCountData();
        [System.NonSerialized] public List<Regex>[,] regices = new List<Regex>[27, 27];
        [System.NonSerialized] public Regex[] ambiguityRegices;
        [System.NonSerialized] public List<List<bool>[,]> ifPassAmbiguityNickname;
        [System.NonSerialized] List<string> excludeStrings;
        [System.NonSerialized] public string currentFileName = string.Empty;
        [System.NonSerialized] public Thread thread;
        [System.NonSerialized] public int numberOfDatas;

        bool ifUpdated = false;

        public class Settings
        {
            public NicknameCountMatrix[] rawMatrices;
            public AmbiguityNicknameSet ambiguityNicknameSet;
            public NicknameSet nicknameSetGlobal;
            public NicknameSet[] nicknameSetCharacter;
            public List<string> excludeStrings;
        }

        public void Initialize(Settings settings)
        {
            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    regices[i, j] = new List<Regex>();
                    NicknameSet nicknameSet = settings.nicknameSetCharacter[i] + settings.nicknameSetGlobal;
                    foreach (var str in nicknameSet.nicknameItems[j].nickNames)
                    {
                        regices[i, j].Add(new Regex(str));
                    }
                }
            }

            List<string> ambiguityRegices = settings.ambiguityNicknameSet.ambiguityRegices;
            this.ambiguityRegices = new Regex[ambiguityRegices.Count];
            for (int i = 0; i < ambiguityRegices.Count; i++)
            {
                this.ambiguityRegices[i] = new Regex(ambiguityRegices[i]);
            }

            ifPassAmbiguityNickname = new List<List<bool>[,]>();
            foreach (var ambiguityRegex in this.ambiguityRegices)
            {
                List<bool>[,] passLists = new List<bool>[27, 27];
                for (int i = 1; i < 27; i++)
                {
                    for (int j = 1; j < 27; j++)
                    {
                        List<bool> list = new List<bool>();
                        foreach (var regex in regices[i, j])
                        {
                            list.Add(ambiguityRegex.IsMatch(regex.ToString()));
                        }
                        passLists[i, j] = list;
                    }
                }
                ifPassAmbiguityNickname.Add(passLists);
            }

            numberOfDatas = settings.rawMatrices.Length;
            excludeStrings = settings.excludeStrings;

            countPreviewArea.Initialize();

            thread = new Thread(() => Count(settings.rawMatrices));
            thread.Start();
        }

        public void Count(NicknameCountMatrix[] rawMatrices)
        {
            //#region Temp01
            //List<string> outputStrings = new List<string>();
            //#endregion

            foreach (var countMatrix in rawMatrices)
            {
                currentFileName = countMatrix.fileName;
                BaseTalkData[] baseTalkDatas = countMatrix.GetTalkDatas();
                foreach (var talkData in baseTalkDatas)
                {
                    if (talkData.characterId <= 0 || talkData.characterId >= 27) continue;
                    bool[] passAmbiguityFlags = new bool[ambiguityRegices.Length];
                    for (int i = 1; i < 27; i++)
                    {
                        for (int j = 0; j < regices[talkData.characterId, i].Count; j++)
                        {
                            Regex regex = regices[talkData.characterId, i][j];

                            string matchSerif = talkData.serif;
                            if (excludeStrings != null)
                            {
                                foreach (var excludeString in excludeStrings)
                                {
                                    matchSerif = matchSerif.Replace(excludeString, string.Empty);
                                }
                            }

                            if (regex.IsMatch(matchSerif))
                            {
                                //#region Temp01
                                //if (!regex.IsMatch(talkData.serif))
                                //{
                                //    string outStr = $"{countMatrix.fileName}:{talkData.windowDisplayName}:{talkData.serif}";
                                //    outputStrings.Add(outStr);
                                //}
                                //#endregion

                                List<int> matchedIndexes = countMatrix.nicknameCountRows[talkData.characterId].nicknameCountGrids[i].matchedIndexes;
                                if (!matchedIndexes.Contains(talkData.referenceIndex))
                                {
                                    matchedIndexes.Add(talkData.referenceIndex);
                                }

                                for (int k = 0; k < ambiguityRegices.Length; k++)
                                {
                                    if (ifPassAmbiguityNickname[k][talkData.characterId, i][j])
                                        passAmbiguityFlags[k] = true;
                                }
                            }
                        }
                    }
                    countMatrix.nicknameCountRows[talkData.characterId].serifCount.Add(talkData.referenceIndex);

                    for (int k = 0; k < ambiguityRegices.Length; k++)
                    {
                        Regex regex = ambiguityRegices[k];
                        if (passAmbiguityFlags[k])
                        {
                            continue;
                        }

                        if (regex.IsMatch(talkData.serif))
                        {
                            countMatrix.AddAmbiguitySerif(talkData.referenceIndex, regex.ToString());
                        }
                    }
                }
                nicknameCountData.Add(countMatrix);
                string saveFolder = Path.GetDirectoryName(countMatrix.SavePath);
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
                countMatrix.SaveData();
                ifUpdated = true;
            }

            //#region Temp01
            //File.WriteAllText(@"C:\\Users\\KUROKAWA_KUJIRA\\Desktop\\1\\out.txt", string.Join("\n\n", outputStrings));
            //#endregion

            currentFileName = "完成";
        }

        private void Update()
        {
            if (ifUpdated)
            {
                ifUpdated = false;
                Refresh();
            }
        }

        public void Refresh()
        {
            countPreviewArea.Refresh();
            perecntBar.priority = (float)nicknameCountData.NicknameCountMatrices.Length / numberOfDatas;
            perecntBar.info = currentFileName;
        }

        private void OnDestroy()
        {
            if (thread != null) thread.Abort();
        }
    }
}