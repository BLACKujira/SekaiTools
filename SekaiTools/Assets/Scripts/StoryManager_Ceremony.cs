using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using SekaiTools.DecompiledClass.Core.VirtualLive;
using System.Collections.Generic;
using System.IO;

namespace SekaiTools
{
    public class StoryManager_Ceremony : StoryManager
    {
        public MasterOfCeremonyData storyData;

        public int[] characterIdInTalkEvents;

        public StoryManager_Ceremony(string filePath, StoryType storyType, MasterOfCeremonyData masterOfCeremonyData, MasterCharacter3D[] character3ds)
        {
            this.filePath = filePath;
            fileName = Path.GetFileNameWithoutExtension(filePath);
            this.storyType = storyType;
            storyData = masterOfCeremonyData;
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
            for (int i = 0; i < storyData.characterTalkEvents.Length; i++)
            {
                CharacterTalkEvent characterTalkEvent = storyData.characterTalkEvents[i];
                BaseTalkData baseTalkData = new BaseTalkData();
                baseTalkData.referenceIndex = i;
                baseTalkData.characterId = characterIdInTalkEvents[i];
                baseTalkData.windowDisplayName = ConstData.characters[baseTalkData.characterId].namae;
                baseTalkData.serif = characterTalkEvent.Serif;
                baseTalkDatas.Add(baseTalkData);
            }
            return baseTalkDatas.ToArray();
        }
    }
}