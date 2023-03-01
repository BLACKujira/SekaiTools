using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SekaiTools.Count
{
    public class NicknameCountData : ISaveData
    {
        public const string unitStoriesFolder = "UnitStories";
        public const string eventStoriesFolder = "EventStories";
        public const string cardStoriesFolder = "CardStories";
        public const string mapTalkFolder = "MapTalk";
        public const string liveTalkFolder = "LiveTalk";
        public const string otherStoriesFolder = "OtherStories";

        public List<NicknameCountMatrix_Scenario> countMatrix_Unit = new List<NicknameCountMatrix_Scenario>();
        public List<NicknameCountMatrix_Scenario> countMatrix_Event = new List<NicknameCountMatrix_Scenario>();
        public List<NicknameCountMatrix_Scenario> countMatrix_Card = new List<NicknameCountMatrix_Scenario>();
        public List<NicknameCountMatrix_Scenario> countMatrix_Map = new List<NicknameCountMatrix_Scenario>();
        public List<NicknameCountMatrix_Ceremony> countMatrix_Live = new List<NicknameCountMatrix_Ceremony>();
        public List<NicknameCountMatrix_Scenario> countMatrix_Other = new List<NicknameCountMatrix_Scenario>();
        public NicknameCountMatrix[] NicknameCountMatrices
        {
            get
            {
                List<NicknameCountMatrix> nicknameCountMatrices = new List<NicknameCountMatrix>();
                nicknameCountMatrices.AddRange(countMatrix_Unit);
                nicknameCountMatrices.AddRange(countMatrix_Event);
                nicknameCountMatrices.AddRange(countMatrix_Card);
                nicknameCountMatrices.AddRange(countMatrix_Map);
                nicknameCountMatrices.AddRange(countMatrix_Live);
                nicknameCountMatrices.AddRange(countMatrix_Other);
                return nicknameCountMatrices.ToArray();
            }
        }
        public int ChangedFileCount => NicknameCountMatrices.Where(ncm => ncm.ifChanged).Count();

        public void AddCountMatrix(StoryType storyType, NicknameCountMatrix nicknameCountMatrix)
        {
            switch (storyType)
            {
                case StoryType.UnitStory:
                    countMatrix_Unit.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.EventStory:
                    countMatrix_Event.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.CardStory:
                    countMatrix_Card.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.MapTalk:
                    countMatrix_Map.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.LiveTalk:
                    countMatrix_Live.Add((NicknameCountMatrix_Ceremony)nicknameCountMatrix);
                    break;
                case StoryType.OtherStory:
                    countMatrix_Other.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.SystemVoice:
                    break;
                default:
                    break;
            }
        }

        public string SavePath { get; set; }

        public NicknameCountItem this[int talkerId, int nameId]
        {
            get => new NicknameCountItem(NicknameCountMatrices, talkerId, nameId);
        }
        public int GetSerifCount(int talkerId)
        {
            int count = 0;
            NicknameCountMatrix[] nicknameCountMatrices = this.NicknameCountMatrices;
            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                count += nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count;
            }
            return count;
        }
        public int GetCountTotal(int talkerId, bool excludeSelf)
        {
            int total = 0;
            for (int i = 1; i < 27; i++)
            {
                if (excludeSelf && i == talkerId) continue;
                NicknameCountItem nicknameCountItem = this[talkerId, i];
                total += nicknameCountItem.Total;
            }
            return total;
        }

        public NicknameCountItemByEvent GetCountItemByEvent(int talkerId, int nameId)
        {
            return new NicknameCountItemByEvent(NicknameCountMatrices, talkerId, nameId);
        }

        public NicknameCountMatrixByEvent GetCountMatrixByEvent() => new NicknameCountMatrixByEvent(countMatrix_Event.ToArray());

        public static NicknameCountData Load_Classic(string folder)
        {
            NicknameCountData nicknameCountData = new NicknameCountData();
            nicknameCountData.SavePath = folder;
            void Load<T>(List<T> targetList, string folderName) where T : NicknameCountMatrix
            {
                string path = Path.Combine(folder, folderName);
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        T t = JsonUtility.FromJson<T>(File.ReadAllText(file));
                        t.fileName = Path.GetFileNameWithoutExtension(file);
                        t.SavePath = file;
                        targetList.Add(t);
                    }
                }
            }
            Load(nicknameCountData.countMatrix_Unit, unitStoriesFolder);
            Load(nicknameCountData.countMatrix_Event, eventStoriesFolder);
            Load(nicknameCountData.countMatrix_Card, cardStoriesFolder);
            Load(nicknameCountData.countMatrix_Map, mapTalkFolder);
            Load(nicknameCountData.countMatrix_Live, liveTalkFolder);
            Load(nicknameCountData.countMatrix_Other, otherStoriesFolder);
            return nicknameCountData;
        }

        public static NicknameCountData Load(string folder)
        {
            NicknameCountData nicknameCountData = new NicknameCountData();
            string[] files = ExtensionTools.GetAllFiles(folder);
            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                NicknameCountMatrix nicknameCountMatrix = null;
                switch (extension)
                {
                    case ".ncmsce":
                        nicknameCountMatrix = LoadMatrix<NicknameCountMatrix_Scenario>(file);
                        break;
                    case ".ncmcer":
                        nicknameCountMatrix = LoadMatrix<NicknameCountMatrix_Ceremony>(file);
                        break;
                    default:
                        break;
                }
                if (nicknameCountMatrix != null) nicknameCountData.Add(nicknameCountMatrix);
            }
            nicknameCountData.SavePath = folder;
            return nicknameCountData;
        }

        static T LoadMatrix<T>(string file) where T : NicknameCountMatrix
        {
            T t = JsonUtility.FromJson<T>(File.ReadAllText(file));
            t.fileName = Path.GetFileNameWithoutExtension(file);
            t.SavePath = file;
            return t;
        }

        public void Add(NicknameCountMatrix nicknameCountMatrix)
        {
            switch (nicknameCountMatrix.storyType)
            {
                case StoryType.UnitStory:
                    countMatrix_Unit.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.EventStory:
                    countMatrix_Event.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.CardStory:
                    countMatrix_Card.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.MapTalk:
                    countMatrix_Map.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                case StoryType.LiveTalk:
                    countMatrix_Live.Add((NicknameCountMatrix_Ceremony)nicknameCountMatrix);
                    break;
                case StoryType.OtherStory:
                    countMatrix_Other.Add((NicknameCountMatrix_Scenario)nicknameCountMatrix);
                    break;
                default:
                    break;
            }
        }

        public int SaveChangedFiles()
        {
            int changedFileCount = 0;
            foreach (var nicknameCountMatrix in NicknameCountMatrices)
            {
                if (nicknameCountMatrix.ifChanged)
                {
                    nicknameCountMatrix.SaveData();
                    nicknameCountMatrix.ifChanged = false;
                    changedFileCount++;
                    Debug.Log(nicknameCountMatrix.fileName);
                }
            }
            return changedFileCount;
        }

        public void SaveData()
        {
            //HACK 全部保存
            SaveChangedFiles();
        }

        public NicknameCountMatrix[] GetMatricesBefore(DateTime untilDateTime)
        {
            return NicknameCountMatrices.Where(mat=>mat.PublishedAt<untilDateTime).ToArray();
        }
    }
}