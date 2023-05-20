using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SekaiTools.Count
{
    [Serializable]
    public abstract class NicknameCountMatrix : ISaveData
    {
        [NonSerialized] public string fileName;
        [NonSerialized] public bool ifChanged = false;
        public StoryType storyType;
        public long publishedAt;
        public NicknameCountRow[] nicknameCountRows;
        public List<AmbiguitySerifSet> ambiguitySerifSets = new List<AmbiguitySerifSet>();

        public DateTime PublishedAt => ExtensionTools.UnixTimeMSToDateTimeTST(publishedAt);

        public NicknameCountMatrix(StoryType storyType, long publishedAt)
        {
            this.storyType = storyType;
            this.publishedAt = publishedAt;
            nicknameCountRows = new NicknameCountRow[27];
            for (int i = 1; i < 27; i++)
            {
                nicknameCountRows[i] = new NicknameCountRow();
            }
        }

        public NicknameCountGrid this[int talkerId, int nameId] => nicknameCountRows[talkerId].nicknameCountGrids[nameId];
        public NicknameCountRow this[int talkerId] => nicknameCountRows[talkerId];

        public string SavePath { get; set; }

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
        public BaseTalkData[] GetTalkDatas(int talkerId, int nameId)
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
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
        }

        public static NicknameCountMatrix[] Sort(NicknameCountMatrix[] nicknameCountMatrices)
        {
            List<StoryType> storyTypePriority = new List<StoryType>()
            {
                StoryType.UnitStory,
                StoryType.OtherStory,
                StoryType.SystemVoice,
                StoryType.EventStory,
                StoryType.CardStory,
                StoryType.LiveTalk,
                StoryType.MapTalk
            };
            return nicknameCountMatrices
                .OrderBy(m => m.publishedAt)
                .ThenBy(m => storyTypePriority.IndexOf(m.storyType))
                .ThenBy(m => m.fileName)
                .ToArray();
        }

        public int GetMatchedTimesOfAnyCharacter(int referenceIndex)
        {
            int count = 0;
            foreach (var grid in this)
            {
                if (grid.matchedIndexes.Contains(referenceIndex)) count++;
            }
            return count;
        }

        public IEnumerator<NicknameCountGrid> GetEnumerator()
        {
            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    yield return this[i, j];
                }
            }
        }

        public AmbiguitySerifSet GetAmbiguitySerifSet(string ambiguityRegex)
        {
            foreach (var ambiguitySerifSet in ambiguitySerifSets)
            {
                if (ambiguitySerifSet.ambiguityRegex.Equals(ambiguityRegex))
                    return ambiguitySerifSet;
            }
            return null;
        }

        public int GetMatchedTimesOfAnyAmbiguitySerifSet(int referenceIndex)
        {
            int count = 0;
            foreach (var ambiguitySerifSet in ambiguitySerifSets)
            {
                if (ambiguitySerifSet.matchedIndexes.Contains(referenceIndex)) count++;
            }
            return count;
        }

        public void AddAmbiguitySerif(int referenceIndex, string regex)
        {
            AmbiguitySerifSet ambiguitySerifSet = null;
            foreach (var item in ambiguitySerifSets)
            {
                if (item.ambiguityRegex.Equals(regex))
                    ambiguitySerifSet = item;
            }
            if (ambiguitySerifSet == null)
            {
                ambiguitySerifSet = new AmbiguitySerifSet(regex);
                ambiguitySerifSets.Add(ambiguitySerifSet);
            }
            ambiguitySerifSet.matchedIndexes.Add(referenceIndex);
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

        public Vector2Int[] NameCountArray
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
        public int Times => matchedIndexes.Count;
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

    public class NicknameCountMatrixByEvent
    {
        public Dictionary<int, NicknameCountMatrixByEvent_Event> events = new Dictionary<int, NicknameCountMatrixByEvent_Event>();

        public NicknameCountMatrixByEvent(NicknameCountMatrix[] nicknameCountMatrices)
        {
            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                AddItem(nicknameCountMatrix);
            }
        }

        void AddItem(NicknameCountMatrix nicknameCountMatrix)
        {
            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(nicknameCountMatrix.fileName);
            if (eventStoryInfo == null) return;
            if (!events.ContainsKey(eventStoryInfo.eventId))
            {
                events[eventStoryInfo.eventId] = new NicknameCountMatrixByEvent_Event(eventStoryInfo.eventId);
            }
            events[eventStoryInfo.eventId].nicknameCountMatrices.Add(nicknameCountMatrix);
        }
    }

    public class NicknameCountMatrixByEvent_Event
    {
        public readonly int eventId;

        public NicknameCountMatrixByEvent_Event(int eventId)
        {
            this.eventId = eventId;
        }

        public List<NicknameCountMatrix> nicknameCountMatrices = new List<NicknameCountMatrix>();

        public int GetTotal(int talkerId, int nameId)
        {
            int count = 0;
            foreach (var countMatrix in nicknameCountMatrices)
            {
                count += countMatrix[talkerId, nameId].Times;
            }
            return count;
        }
    }

    [Serializable]
    public class NicknameCountItemByEvent
    {
        public Dictionary<int, int> countDictionary = new Dictionary<int, int>();
        public readonly int talkerId;
        public readonly int nameId;

        void AddItem(NicknameCountMatrix nicknameCountMatrix)
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

    [Serializable]
    public class AmbiguitySerifSet
    {
        public string ambiguityRegex;

        public AmbiguitySerifSet(string ambiguityRegex)
        {
            this.ambiguityRegex = ambiguityRegex;
        }

        public List<int> matchedIndexes = new List<int>();
    }
}