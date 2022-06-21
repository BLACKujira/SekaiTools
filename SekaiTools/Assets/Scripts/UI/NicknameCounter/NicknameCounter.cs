using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using SekaiTools.Count;
using System.Text.RegularExpressions;
using SekaiTools.UI.Downloader;

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
        [System.NonSerialized] public string currentFileName = string.Empty;
        [System.NonSerialized] public Thread thread;
        [System.NonSerialized] public int numberOfDatas;

        bool ifUpdated = false;

        public class NicknameCounterSettings
        {
            public NicknameCountMatrix[] rawMatrices;
            public NicknameSet nicknameSetGlobal;
            public NicknameSet[] nicknameSetCharacter;
        }

        public void Initialize(NicknameCounterSettings settings)
        {
            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    regices[i, j] = new List<Regex>();
                    NicknameSet nicknameSet = settings.nicknameSetCharacter[i] + settings.nicknameSetGlobal;
                    foreach (var str in nicknameSet.nicknameItems[j].nickNames)
                    {
                        regices[i,j].Add(new Regex(str));
                    }
                }
            }

            numberOfDatas = settings.rawMatrices.Length;

            countPreviewArea.Initialize();

            thread = new Thread(() => Count(settings.rawMatrices));
            thread.Start();
        }


        public void Count(NicknameCountMatrix[] rawMatrices)
        {
            foreach (var countMatrix in rawMatrices)
            {
                currentFileName = countMatrix.fileName;
                BaseTalkData[] baseTalkDatas = countMatrix.GetTalkDatas();
                foreach (var talkData in baseTalkDatas)
                {
                    if (talkData.characterId <= 0 || talkData.characterId >= 27) continue;
                    for (int i = 1; i < 27; i++)
                    {
                        foreach (var regex in regices[talkData.characterId, i])
                        {
                            if (regex.IsMatch(talkData.serif))
                            {
                                countMatrix.nicknameCountRows[talkData.characterId].nicknameCountGrids[i].matchedIndexes.Add(talkData.referenceIndex);
                                break;
                            }
                        }
                    }
                    countMatrix.nicknameCountRows[talkData.characterId].serifCount.Add(talkData.referenceIndex);
                }
                nicknameCountData.Add(countMatrix);
                countMatrix.SaveData();
                ifUpdated = true;
            }
            currentFileName = "完成";
        }

        private void Update()
        {
            if(ifUpdated)
            {
                ifUpdated = false;
                Refresh();
            }
        }

        public void Refresh()
        {
            countPreviewArea.Refresh();
            perecntBar.priority = (float)nicknameCountData.nicknameCountMatrices.Length / numberOfDatas;
            perecntBar.info = currentFileName;
        }

        private void OnDestroy()
        {
            if (thread != null) thread.Abort();
        }
    }
}