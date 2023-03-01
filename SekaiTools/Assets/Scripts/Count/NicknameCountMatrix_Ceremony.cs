using SekaiTools.DecompiledClass;
using SekaiTools.DecompiledClass.Core.VirtualLive;
using System;
using System.Collections.Generic;

namespace SekaiTools.Count
{
    [Serializable]
    public class NicknameCountMatrix_Ceremony : NicknameCountMatrix
    {
        public MasterOfCeremonyData masterOfCeremonyData;
        public int[] characterIdInTalkEvents;

        public NicknameCountMatrix_Ceremony(StoryType storyType, long publishedAt, MasterOfCeremonyData masterOfCeremonyData, MasterCharacter3D[] character3ds) : base(storyType, publishedAt)
        {
            this.masterOfCeremonyData = masterOfCeremonyData;
            characterIdInTalkEvents = new int[masterOfCeremonyData.characterTalkEvents.Length];
            Dictionary<int, int> dicC3dId = new Dictionary<int, int>();
            foreach (var masterCharacter3D in character3ds)
            {
                dicC3dId[masterCharacter3D.id] = masterCharacter3D.characterId;
            }
            for (int i = 0; i < characterIdInTalkEvents.Length; i++)
            {
                int c3dId = masterOfCeremonyData.characterTalkEvents[i].Character3dId;
                characterIdInTalkEvents[i] = dicC3dId.ContainsKey(c3dId) ? dicC3dId[c3dId] : 0;
            }
        }

        public override BaseTalkData[] GetTalkDatas()
        {
            List<BaseTalkData> baseTalkDatas = new List<BaseTalkData>();
            for (int i = 0; i < masterOfCeremonyData.characterTalkEvents.Length; i++)
            {
                CharacterTalkEvent characterTalkEvent = masterOfCeremonyData.characterTalkEvents[i];
                BaseTalkData baseTalkData = new BaseTalkData();
                baseTalkData.referenceIndex = i;
                baseTalkData.characterId = characterIdInTalkEvents[i];
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
                talkData.characterId = characterIdInTalkEvents[i];
                talkData.serif = characterTalkEvent.Serif;
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
            return talkDatas.ToArray();
        }
    }
}