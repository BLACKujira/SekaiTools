using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI
{
    public class MobInfoCounter
    {
        List<StoryManager> stories;
        public List<StoryManager> Stories => stories;

        Dictionary<int, MobInfo> mobInfos = new Dictionary<int, MobInfo>();
        public Dictionary<int, MobInfo> MobInfos => mobInfos;

        Settings settings;
        MasterCharacter2D[] character2Ds;

        public class Settings
        {
            public string[] UnitStoryFiles;
            public string[] EventStoryFiles;
            public string[] CardStoryFiles;
            public string[] OtherStoryFiles;
        }

        public void Count(Settings settings)
        {
            this.settings = settings;
            character2Ds = EnvPath.GetTable<MasterCharacter2D>("character2Ds");

            InitMobInfos();
            CountNicknames();
        }

        /// <summary>
        /// 返回或创建并返回一个角色信息
        /// </summary>
        MobInfo GetOrCreateMobInfo(int characterId)
        {
            if (!mobInfos.ContainsKey(characterId))
            {
                mobInfos[characterId] = new MobInfo(characterId);
            }
            return mobInfos[characterId];
        }

        /// <summary>
        /// 初始化mobInfos字典
        /// </summary>
        void InitMobInfos()
        {
            foreach (var character2D in character2Ds)
            {
                if (!character2D.characterType.Equals("mob")) return;
                MobInfo mobInfo = GetOrCreateMobInfo(character2D.characterId);
                mobInfo.character2dIds.Add(character2D.id);
                if (!string.IsNullOrEmpty(character2D.assetName))
                {
                    mobInfo.assetNames.Add(character2D.assetName);
                }
            }
        }

        void CountNicknames()
        {
            List<string> storyFilePaths = new List<string>();
            storyFilePaths.AddRange(settings.UnitStoryFiles);
            storyFilePaths.AddRange(settings.EventStoryFiles);
            storyFilePaths.AddRange(settings.CardStoryFiles);
            storyFilePaths.AddRange(settings.OtherStoryFiles);

            foreach (var path in storyFilePaths)
            {
                string json = File.ReadAllText(path);
                ScenarioSceneData scenarioSceneData = JsonUtility.FromJson<ScenarioSceneData>(json);
                StoryManager_Scenario storyManager = new StoryManager_Scenario(path, StoryType.UnitStory, scenarioSceneData);
                CountNicknames_ScenarioSceneData(storyManager);
            }
        }

        void CountNicknames_ScenarioSceneData(StoryManager_Scenario storyManager)
        {
            for (int i = 0; i < storyManager.storyData.TalkData.Length; i++)
            {
                CountNicknames_ScenarioSnippet(storyManager, i);
            }
        }

        void CountNicknames_ScenarioSnippet(StoryManager_Scenario storyManager, int idx)
        {
            ScenarioSnippetTalk scenarioSnippetTalk = storyManager.storyData.TalkData[idx];
            if (scenarioSnippetTalk.TalkCharacters.Length == 0) return;
            int character2dId = scenarioSnippetTalk.TalkCharacters[0].Character2dId;
            if (mobInfos.ContainsKey(character2dId))
            {
                if(!mobInfos[character2dId].nicknames.ContainsKey(scenarioSnippetTalk.WindowDisplayName))
                {
                    mobInfos[character2dId].nicknames[scenarioSnippetTalk.WindowDisplayName] = 0;
                }

                mobInfos[character2dId].nicknames[scenarioSnippetTalk.WindowDisplayName]++;
                mobInfos[character2dId].AddSerif(storyManager.fileName, idx);
            }
        }

        public List<StoryManager> GetStories(int characterId)
        {
            List<StoryManager> storyManagers = stories
                .Where(s => mobInfos[characterId].serifs.ContainsKey(s.fileName))
                .ToList();

            foreach (var storyManager in storyManagers)
            {
                storyManager.highLightRefIdx = mobInfos[characterId].serifs[storyManager.fileName];
            }

            return storyManagers;
        }
    }
}
