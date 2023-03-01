using SekaiTools.DecompiledClass;
using System;
using System.Collections.Generic;

namespace SekaiTools.Count
{
    [Serializable]
    public class NicknameCountMatrix_Scenario : NicknameCountMatrix
    {
        public ScenarioSceneData scenarioSceneData;

        public NicknameCountMatrix_Scenario(StoryType storyType, long publishedAt, ScenarioSceneData scenarioSceneData) : base(storyType, publishedAt)
        {
            this.scenarioSceneData = scenarioSceneData;
        }

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
                {
                    characterId = ConstData.MergeVirtualSinger(character2dId);
                    characterId = characterId > 0 && characterId < 27 ? characterId : 0;
                }

                talkData.characterId = characterId;
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
}