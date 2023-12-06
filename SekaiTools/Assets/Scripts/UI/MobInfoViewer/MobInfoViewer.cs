using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI
{
    public class MobInfoCounter : MonoBehaviour
    {
        Dictionary<int, MobInfo> mobInfos = new Dictionary<int, MobInfo>();
        Settings settings;

        public class MobInfo
        {
            public int characterId;
            public HashSet<int> character2dIds = new HashSet<int>();
            public HashSet<string> assetNames = new HashSet<string>();
            public HashSet<string> nicknames = new HashSet<string>();
            public HashSet<string> serifs = new HashSet<string>();

            public MobInfo(int characterId)
            {
                this.characterId = characterId;
            }
        }

        public class Settings
        {
            public string[] UnitStoryFiles;
            public string[] EventStoryFiles;
            public string[] CardStoryFiles;
            public string[] OtherStoryFiles;
            public MasterCharacter2D[] character2Ds;
        }

        public void Initialize(Settings settings)
        {
            this.settings = settings;

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
            foreach (var character2D in settings.character2Ds)
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
                CountNicknames_ScenarioSceneData(scenarioSceneData);
            }
        }

        void CountNicknames_ScenarioSceneData(ScenarioSceneData scenarioSceneData)
        {
            foreach (var scenarioSnippetTalk in scenarioSceneData.TalkData)
            {
                CountNicknames_ScenarioSnippet(scenarioSnippetTalk);
            }
        }

        void CountNicknames_ScenarioSnippet(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            if (scenarioSnippetTalk.TalkCharacters.Length == 0) return;
            int character2dId = scenarioSnippetTalk.TalkCharacters[0].Character2dId;
            if (mobInfos.ContainsKey(character2dId))
            {
                mobInfos[character2dId].nicknames.Add(scenarioSnippetTalk.WindowDisplayName);
            }
        }
    }

    /// <summary>
    /// 用于显示配角信息的UI组件
    /// </summary>
    public class MobInfoViewer : MonoBehaviour
    {


    }
}
