using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using System.IO;

namespace SekaiTools
{
    public class StoryManager_Scenario : StoryManager
    {
        public ScenarioSceneData storyData;

        public StoryManager_Scenario(string filePath, StoryType storyType, ScenarioSceneData scenarioSceneData)
        {
            this.filePath = filePath;
            fileName = Path.GetFileNameWithoutExtension(filePath);
            this.storyType = storyType;
            storyData = scenarioSceneData;
        }

        public override BaseTalkData[] GetTalkDatas()
        {
            List<BaseTalkData> baseTalkDatas = new List<BaseTalkData>();
            for (int i = 0; i < storyData.TalkData.Length; i++)
            {
                ScenarioSnippetTalk scenarioSnippetTalk = storyData.TalkData[i];
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
    }
}