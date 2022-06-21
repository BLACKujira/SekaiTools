using SekaiTools.DecompiledClass.Core.VirtualLive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.DecompiledClass;
using System.IO;

namespace SekaiTools.Count
{
    public class NicknameCountData :ISaveData
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
        public NicknameCountMatrix[] nicknameCountMatrices 
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

        public string savePath { get; set; }

        public NicknameCountItem this[int talkerId, int nameId]
        {
            get => new NicknameCountItem(nicknameCountMatrices, talkerId, nameId);
        }
        public int GetSerifCount(int talkerId) 
        {
            int count = 0;
            NicknameCountMatrix[] nicknameCountMatrices = this.nicknameCountMatrices;
            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                count += nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count;  
            }
            return count;
        }
        public int GetCountTotal(int talkerId,bool excludeSelf)
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

        public NicknameCountItemByEvent GetCountItemByEvent(int talkerId,int nameId)
        {
            return new NicknameCountItemByEvent(nicknameCountMatrices, talkerId, nameId);
        }

        public static NicknameCountData Load(string folder)
        {
            NicknameCountData nicknameCountData = new NicknameCountData();
            nicknameCountData.savePath = folder;
            void Load<T>(List<T> targetList,string folderName) where T : NicknameCountMatrix
            {
                string path = Path.Combine(folder, folderName);
                if(Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        T t = JsonUtility.FromJson<T>(File.ReadAllText(file));
                        t.fileName = Path.GetFileNameWithoutExtension(file);
                        t.savePath = file;
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

        public void SaveChangedFiles()
        {
            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                if(nicknameCountMatrix.ifChanged)
                {
                    nicknameCountMatrix.SaveData();
                    nicknameCountMatrix.ifChanged = false;
                    Debug.Log(nicknameCountMatrix.fileName);
                }
            }
        }

        public void SaveData()
        {
            //HACK 全部保存
            SaveChangedFiles();
        }
    }

    [Serializable]
    public abstract class NicknameCountMatrix : ISaveData
    {
        [NonSerialized] public string fileName;
        [NonSerialized] public bool ifChanged = false;
        public StoryType storyType;
        public NicknameCountRow[] nicknameCountRows;

        public NicknameCountMatrix()
        {
            nicknameCountRows = new NicknameCountRow[27];
            for (int i = 1; i < 27; i++)
            {
                nicknameCountRows[i] = new NicknameCountRow();
            }
        }

        public NicknameCountGrid this[int talkerId, int nameId]
        {
            get
            {
                return nicknameCountRows[talkerId].nicknameCountGrids[nameId];
            }
        }

        public string savePath { get; set; }

        public abstract BaseTalkData[] GetTalkDatas();
        public BaseTalkData[] GetTalkDatas(int talkerId)
        {
            BaseTalkData[] baseTalkDatas = GetTalkDatas();
            List<BaseTalkData> talkDatas = new List<BaseTalkData>();
            foreach (var talkData in baseTalkDatas)
            {
                if (nicknameCountRows[talkerId].serifCount.Contains(talkData.referenceIndex))
                    talkDatas.Add(talkData);
            }
            return talkDatas.ToArray();
        }
        public BaseTalkData[] GetTalkDatas(int talkerId,int nameId)
        {
            BaseTalkData[] baseTalkDatas = GetTalkDatas();
            List<BaseTalkData> talkDatas = new List<BaseTalkData>();
            foreach (var talkData in baseTalkDatas)
            {
                if (this[talkerId, nameId].matchedIndexes.Contains(talkData.referenceIndex))
                    talkDatas.Add(talkData);
            }
            return talkDatas.ToArray();
        }

        public abstract TalkDataWithNicknameCount[] GetTalkDatasWithNicknameCount();

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this,true);
            File.WriteAllText(savePath, json);
        }
    }

    [Serializable]
    public class NicknameCountMatrix_Ceremony : NicknameCountMatrix
    {
        public MasterOfCeremonyData masterOfCeremonyData;

        public override BaseTalkData[] GetTalkDatas()
        {
            List<BaseTalkData> baseTalkDatas = new List<BaseTalkData>();
            for (int i = 0; i < masterOfCeremonyData.characterTalkEvents.Length; i++)
            {
                CharacterTalkEvent characterTalkEvent = masterOfCeremonyData.characterTalkEvents[i];
                BaseTalkData baseTalkData = new BaseTalkData();
                baseTalkData.referenceIndex = i;
                baseTalkData.characterId = GlobalData.Character3dIdToCharacterId(characterTalkEvent.Character3dId);
                baseTalkData.windowDisplayName = ConstData.characters[baseTalkData.characterId].namae;
                baseTalkData.serif = characterTalkEvent.Serif;
                baseTalkDatas.Add(baseTalkData);
            }
            return baseTalkDatas.ToArray();
        }

        public override TalkDataWithNicknameCount[] GetTalkDatasWithNicknameCount()
        {
            List<TalkDataWithNicknameCount> talkDatas = new List<TalkDataWithNicknameCount>();
            for (int i = 0; i < masterOfCeremonyData.characterTalkEvents.Length; i++)
            {
                CharacterTalkEvent characterTalkEvent = masterOfCeremonyData.characterTalkEvents[i];
                TalkDataWithNicknameCount talkData = new TalkDataWithNicknameCount();
                talkData.referenceIndex = i;
                talkData.characterId = GlobalData.Character3dIdToCharacterId(characterTalkEvent.Character3dId);
                talkData.serif = characterTalkEvent.Serif;
                talkDatas.Add(talkData);
            }
            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    foreach (var index in this[i,j].matchedIndexes)
                    {
                        talkDatas[index].markedCharacterIds.Add(j);
                    }
                }
            }
            return talkDatas.ToArray();
        }
    }
    
    [Serializable]
    public class NicknameCountMatrix_Scenario : NicknameCountMatrix
    {
        public ScenarioSceneData scenarioSceneData;

        public override BaseTalkData[] GetTalkDatas()
        {
            List<BaseTalkData> baseTalkDatas = new List<BaseTalkData>();
            for (int i = 0; i < scenarioSceneData.TalkData.Length; i++)
            {
                ScenarioSnippetTalk scenarioSnippetTalk = scenarioSceneData.TalkData[i];
                BaseTalkData baseTalkData = new BaseTalkData();
                baseTalkData.referenceIndex = i;
                int characterId = ConstData.GetCharacterId_Scenario(scenarioSnippetTalk);
                baseTalkData.characterId = characterId;
                baseTalkData.windowDisplayName = scenarioSnippetTalk.WindowDisplayName;
                baseTalkData.serif = scenarioSnippetTalk.Body;
                baseTalkDatas.Add(baseTalkData);
            }

            //TODO 按 Snippets 排序

            return baseTalkDatas.ToArray();
        }

        public override TalkDataWithNicknameCount[] GetTalkDatasWithNicknameCount()
        {
            List<TalkDataWithNicknameCount> talkDatas = new List<TalkDataWithNicknameCount>();
            for (int i = 0; i < scenarioSceneData.TalkData.Length; i++)
            {
                ScenarioSnippetTalk scenarioSnippetTalk = scenarioSceneData.TalkData[i];
                TalkDataWithNicknameCount talkData = new TalkDataWithNicknameCount();
                talkData.referenceIndex = i;
                int characterId;
                int character2dId = scenarioSnippetTalk.TalkCharacters[0].Character2dId;
                if (scenarioSnippetTalk.TalkCharacters.Length <= 0 || character2dId == 0)
                    characterId = ConstData.NamaeToId(scenarioSnippetTalk.WindowDisplayName);
                else
                    characterId = character2dId>0&&character2dId<27?character2dId:0;
                talkData.characterId = GlobalData.Character3dIdToCharacterId(characterId);
                talkData.serif = scenarioSnippetTalk.Body;
                talkDatas.Add(talkData);

            }

            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    foreach (var index in this[i, j].matchedIndexes)
                    {
                        talkDatas[index].markedCharacterIds.Add(j);
                    }
                }
            }

            //TODO 按 Snippets 排序

            return talkDatas.ToArray();
        }

    }
    
    [Serializable]
    public class NicknameCountRow
    {
        public List<int> serifCount = new List<int>();
        public NicknameCountGrid[] nicknameCountGrids;

        public NicknameCountRow()
        {
            nicknameCountGrids = new NicknameCountGrid[27];
            for (int i = 1; i < 27; i++)
            {
                nicknameCountGrids[i] = new NicknameCountGrid();
            }
        }

        public Vector2Int[] nameCountArray
        {
            get
            {
                List<Vector2Int> vector2Ints = new List<Vector2Int>();
                for (int i = 1; i < 27; i++)
                {
                    if (nicknameCountGrids[i] != null && nicknameCountGrids[i].matchedIndexes.Count > 0)
                    {
                        vector2Ints.Add(new Vector2Int(i, nicknameCountGrids[i].matchedIndexes.Count));
                    }
                }
                return vector2Ints.ToArray();
            }
        }

    }

    [Serializable]
    public class NicknameCountGrid
    {
        public List<int> matchedIndexes = new List<int>();
    }

    [Serializable]
    public class NicknameCountItem
    {
        public Dictionary<StoryType, int> countDictionary = new Dictionary<StoryType, int>();

        public int this[StoryType storyType]
        {
            get
            {
                if (!countDictionary.ContainsKey(storyType)) return 0;
                else return countDictionary[storyType];
            }
        }

        public readonly int talkerId;
        public readonly int nameId;

        public NicknameCountItem(NicknameCountMatrix[] nicknameCountMatrices, int talkerId, int nameId)
        {
            this.talkerId = talkerId;
            this.nameId = nameId;

            foreach (var item in Enum.GetValues(typeof(StoryType)))
            {
                countDictionary[(StoryType)item] = 0;
            }

            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                countDictionary[nicknameCountMatrix.storyType] += nicknameCountMatrix[talkerId, nameId].matchedIndexes.Count;
            }
        }

        public int GetCount(StoryType storyType)
        {
            if (!countDictionary.ContainsKey(storyType)) return 0;
            return countDictionary[storyType];
        }

        public int Total
        {
            get
            {
                int total = 0;
                foreach (var keyValuePair in countDictionary)
                {
                    total += keyValuePair.Value;
                }
                return total;
            }
        }
    }

    [Serializable]
    public class NicknameCountItemByEvent
    {
        public Dictionary<int, int> countDictionary = new Dictionary<int, int>();
        public readonly int talkerId;
        public readonly int nameId;

        public void AddItem(NicknameCountMatrix nicknameCountMatrix)
        {
            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(nicknameCountMatrix.fileName);
            if (eventStoryInfo == null) return;
            countDictionary[eventStoryInfo.eventId] = countDictionary.ContainsKey(eventStoryInfo.eventId) ?
                countDictionary[eventStoryInfo.eventId] + nicknameCountMatrix[talkerId, nameId].matchedIndexes.Count :
                nicknameCountMatrix[talkerId, nameId].matchedIndexes.Count;
        }

        public NicknameCountItemByEvent(NicknameCountMatrix[] nicknameCountMatrices, int talkerId, int nameId)
        {
            this.talkerId = talkerId;
            this.nameId = nameId;
            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                AddItem(nicknameCountMatrix);
            }
        }
    }

    public class BaseTalkData
    {
        public int referenceIndex;
        public int characterId;
        public string windowDisplayName;
        public string serif;
    }

    public class TalkDataWithNicknameCount : BaseTalkData
    {
        public List<int> markedCharacterIds;
    }
}