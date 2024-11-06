using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI
{
    public class MobInfoCounter
    {
        List<StoryManager> stories = new List<StoryManager>();
        public List<StoryManager> Stories => stories;

        Dictionary<int, MobInfo> mobInfos = new Dictionary<int, MobInfo>();
        public Dictionary<int, MobInfo> MobInfos => mobInfos;

        Settings settings;
        MasterCharacter2D[] character2Ds;
        Dictionary<int, MasterCharacter2D> character2DDic = new Dictionary<int, MasterCharacter2D>();

        public static string[] RequireMasterTables = new string[]
        {
            "character2ds",
        };

        public class Settings
        {
            public string[] UnitStoryFiles;
            public string[] EventStoryFiles;
            public string[] CardStoryFiles;
            public string[] OtherStoryFiles;
        }

        public void Initialize(Settings settings)
        {
            this.settings = settings;
            character2Ds = EnvPath.GetTable<MasterCharacter2D>("character2ds");

            InitMobInfos();
            CountNicknames();

            RemoveUnusedStories();
            RemoveUnusedMobInfo();
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
                if (!character2D.characterType.Equals("mob")) continue;
                if(character2D.characterId >= 1 && character2D.characterId <= 26) continue; // 跳过主要角色
                if (character2D.characterId == 900000) continue; // 900000是多个角色同时出现的情况，跳过

                MobInfo mobInfo = GetOrCreateMobInfo(character2D.characterId);
                mobInfo.character2dIds.Add(character2D.id);
                if (!string.IsNullOrEmpty(character2D.assetName))
                {
                    mobInfo.assetNames.Add(character2D.assetName);
                }

                character2DDic[character2D.id] = character2D;
            }
        }

        void CountNicknames()
        {
            List<(string path, StoryType storyType)> storyFilePaths = new List<(string, StoryType)>();
            storyFilePaths.AddRange(settings.UnitStoryFiles.Select(s=>(s, StoryType.UnitStory)));
            storyFilePaths.AddRange(settings.EventStoryFiles.Select(s => (s, StoryType.EventStory)));
            storyFilePaths.AddRange(settings.CardStoryFiles.Select(s => (s, StoryType.CardStory)));
            storyFilePaths.AddRange(settings.OtherStoryFiles.Select(s => (s, StoryType.OtherStory)));

            foreach (var t in storyFilePaths)
            {
                string json = File.ReadAllText(t.path);
                ScenarioSceneData scenarioSceneData = JsonUtility.FromJson<ScenarioSceneData>(json);
                StoryManager_Scenario storyManager = new StoryManager_Scenario(t.path, t.storyType, scenarioSceneData);
                stories.Add(storyManager);
                CountNicknames_ScenarioSceneData(storyManager);
                AddInfo_CostumeType(storyManager);
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

            // 如果没有这个角色，就不统计 (也可能为主要角色)
            if(!character2DDic.ContainsKey(character2dId))
            {
                return;
            }

            int characterId = character2DDic[character2dId].characterId;
            if (mobInfos.ContainsKey(characterId))
            {
                MobInfo mobInfo = mobInfos[characterId];

                // 记录昵称
                if (!scenarioSnippetTalk.WindowDisplayName.Equals("？？？"))
                {
                    if (!mobInfo.nicknames.ContainsKey(scenarioSnippetTalk.WindowDisplayName))
                    {
                        mobInfo.nicknames[scenarioSnippetTalk.WindowDisplayName] = 0;
                    }
                    mobInfo.nicknames[scenarioSnippetTalk.WindowDisplayName]++;
                }

                mobInfo.AddSerif(storyManager.fileName, idx); // 记录台词
            }
        }

        void AddInfo_CostumeType(StoryManager_Scenario storyManager)
        {
            foreach (var scenarioCharacterResourceSet in storyManager.storyData.AppearCharacters)
            {
                if(character2DDic.ContainsKey(scenarioCharacterResourceSet.Character2dId) &&
                    mobInfos.ContainsKey(character2DDic[scenarioCharacterResourceSet.Character2dId].characterId))
                {
                    MobInfo mobInfo = mobInfos[character2DDic[scenarioCharacterResourceSet.Character2dId].characterId];
                    if (!mobInfo.costumeTypes.Contains(scenarioCharacterResourceSet.CostumeType))
                    {
                        mobInfo.costumeTypes.Add(scenarioCharacterResourceSet.CostumeType);
                    }
                }
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

        void RemoveUnusedMobInfo()
        {
            List<int> unusedMobInfoKeys = new List<int>();
            foreach (var mobInfo in mobInfos)
            {
                if (mobInfo.Value.nicknames.Count == 0)
                {
                    unusedMobInfoKeys.Add(mobInfo.Key);
                }
            }
            foreach (var key in unusedMobInfoKeys)
            {
                mobInfos.Remove(key);
            }
        }

        void RemoveUnusedStories()
        {
            HashSet<string> usedStories = new HashSet<string>(mobInfos.SelectMany(mi => mi.Value.serifs.Keys)
                .Distinct()); 
            stories.RemoveAll(s => !usedStories.Contains(s.fileName));
        }
    }
}
