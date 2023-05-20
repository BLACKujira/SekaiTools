using SekaiTools.Count;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChartCharacter : DynamicBarChart
    {
        [Header("Settings")]
        public HDRColorSet charHDRColorSet;

        int characterId = 1;
        public override string Information => $@"角色 {ConstData.characters[characterId].Name}
数据帧长度 {frameHoldTime}, 数据帧数量 {dataFrames.Length}
最小持续时间 {holdTime}, 预计持续时间 {GetHoldTime()}";

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","场景",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Character("角色","场景",()=>characterId,(value)=>characterId = value),
                new ConfigUIItem_Float("数据帧持续时间","场景",()=>frameHoldTime,(value)=>frameHoldTime = value),
            };

        public override void Refresh()
        {
            canMoveNext = false;
            Stop();
            AbortAllItems();

            List<DataFrameCharacter> dataFrames = GetDataFrameCharacter(countData, characterId, player);

            this.dataFrames = dataFrames.ToArray();
            progressBar.Clear();
            progressBar.Initialize(this.dataFrames);
            progressBar.SetPortalColor(charHDRColorSet.colors[characterId]);

            Play();
        }

        public static List<DataFrameCharacter> GetDataFrameCharacter(NicknameCountData countData, int characterId, NCSPlayerBase player)
        {
            List<DataFrameCharacter> dataFrames = new List<DataFrameCharacter>();
            NicknameCountMatrix[] sortedNicknameCountMatrices = countData.SortedNicknameCountMatrices;
            Dictionary<string, int> count = new Dictionary<string, int>();

            string currentEventGroup = null;
            foreach (var countMatrix in sortedNicknameCountMatrices)
            {
                //判断是否不存在台词
                BaseTalkData[] baseTalkDatas = countMatrix.GetTalkDatas();
                bool hasSerif = false;
                foreach (var baseTalkData in baseTalkDatas)
                {
                    if (baseTalkData.characterId == characterId)
                    {
                        hasSerif = true;
                        break;
                    }
                }
                if (!hasSerif) continue;

                //记录活动组
                if (countMatrix.storyType == StoryType.EventStory)
                {
                    EventStoryInfo eventStoryInfo = ConstData.IsEventStory(countMatrix.fileName);
                    if (eventStoryInfo != null)
                    {
                        foreach (var ev in player.events)
                        {
                            if (ev.id == eventStoryInfo.eventId)
                            {
                                currentEventGroup = ev.assetbundleName;
                            }
                        }
                    }
                }

                //记录数据
                Dictionary<string, float> data = new Dictionary<string, float>();
                for (int i = 1; i < countMatrix[characterId].nicknameCountGrids.Length; i++)
                {
                    NicknameCountGrid nicknameCountGrid = countMatrix[characterId].nicknameCountGrids[i];
                    string key = $"{characterId:00}_{i:00}";
                    count[key] = count.ContainsKey(key) ? count[key] + nicknameCountGrid.Times : nicknameCountGrid.Times;
                    if (count[key] > 0) data[key] = count[key];
                }

                DataFrameCharacter dataFrameCharacter = new DataFrameCharacter(data, countMatrix, currentEventGroup);
                dataFrames.Add(dataFrameCharacter);
            }

            return dataFrames;
        }

        public new class Settings : DynamicBarChart.Settings
        {
            public int characterId;

            public Settings(DynamicBarChartCharacter dynamicBarChartCharacter) : base(dynamicBarChartCharacter)
            {
                characterId = dynamicBarChartCharacter.characterId;
            }
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            holdTime = settings.holdTime;
            frameHoldTime = settings.frameHoldTime;
            requireImageKeys = new HashSet<string>(settings.requireImageKeys);
            characterId = settings.characterId;
        }
    }
}