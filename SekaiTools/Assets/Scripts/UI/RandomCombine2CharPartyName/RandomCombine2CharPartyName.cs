using UnityEngine;
using SekaiTools.DecompiledClass;
using SekaiTools.DecompiledClass.CheerfulCarnival;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace SekaiTools.UI.RandomCombine2CharPartyName
{
    public class RandomCombine2CharPartyName : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RandomCombine2CharPartyName_Item[] items;
        public RandomCombine2CharPartyName_Mask[] masks;
        public RandomCombine2CharPartyName_Particle[] particles;

        MasterCheerfulCarnivalPartyName[] carnivalPartyNames;
        MasterBondsHonor[] bondsHonors;
        MasterBondsHonorWord[] bondsHonorWords;

        Dictionary<CharIdSet, List<string>> canUseGroups;

        Settings settings;

        public class Settings
        {
            public bool useBondsHonor;
            public bool useCarnivalPartyName;

            public MasterCheerfulCarnivalPartyName[] carnivalPartyNames;
            public MasterBondsHonor[] bondsHonors;
            public MasterBondsHonorWord[] bondsHonorWords;

            public bool useRefreshEffect = true;
            public float refreshEffectPlayAfter = 0;
            public float refreshEffectDelay = 0.25f;
        }

        public void Initialize(Settings settings)
        {
            this.settings = settings;

            bondsHonors = settings.bondsHonors;
            carnivalPartyNames = settings.carnivalPartyNames;
            bondsHonorWords = settings.bondsHonorWords;

            canUseGroups = new Dictionary<CharIdSet, List<string>>();

            if(settings.useCarnivalPartyName)
            {
                foreach (var cheerfulCarnivalPartyName in carnivalPartyNames)
                {
                    HashSet<int> charIds = new HashSet<int>()
                    {
                        cheerfulCarnivalPartyName.gameCharacterUnitId1,
                        cheerfulCarnivalPartyName.gameCharacterUnitId2,
                        cheerfulCarnivalPartyName.gameCharacterUnitId3,
                        cheerfulCarnivalPartyName.gameCharacterUnitId4,
                        cheerfulCarnivalPartyName.gameCharacterUnitId5,
                    };

                    foreach (var id in charIds.ToArray())
                    {
                        if (id < 1 || id > 20)
                            charIds.Remove(id);
                    }

                    if(charIds.Count>=2)
                    {
                        int[] idArray = charIds.ToArray();
                        for (int a = 0; a < idArray.Length; a++)
                        {
                            for (int b = a+1; b < idArray.Length; b++)
                            {
                                AddToDictionary(
                                    idArray[a], idArray[b],
                                    cheerfulCarnivalPartyName.partyName);
                            }
                        }
                    }
                }
            }

            if(settings.useBondsHonor)
            {
                foreach (var bondsHonorWord in bondsHonorWords)
                {
                    MasterBondsHonor[] selectedBH = bondsHonors
                        .Where((bh) => bh.bondsGroupId == bondsHonorWord.bondsGroupId).ToArray();
                    if (selectedBH.Length > 0
                        && (selectedBH[0].gameCharacterUnitId1 > 0 && selectedBH[0].gameCharacterUnitId1 < 21)
                        && (selectedBH[0].gameCharacterUnitId2 > 0 && selectedBH[0].gameCharacterUnitId2 < 21))
                        AddToDictionary(selectedBH[0].gameCharacterUnitId1, selectedBH[0].gameCharacterUnitId2,
                            bondsHonorWord.name);
                }
            }
        }

        void AddToDictionary(int charAId,int charBId,string groupName)
        {
            CharIdSet set = new CharIdSet(charAId, charBId);
            if (canUseGroups.ContainsKey(set))
            {
                canUseGroups[set].Add(groupName);
            }
            else
            {
                canUseGroups[set] = new List<string>() { groupName };

            }

        }

        /// <summary>
        /// 弃用，理论可解，实际没完成过
        /// </summary>
        /// <param name="charOrder"></param>
        /// <returns></returns>
        List<HashSet<int>> GetAvailableSequence(int[] charOrder)
        {
            //如果元素个数少于2个或为单数则不可解
            if (charOrder.Length <= 1 || charOrder.Length % 2 == 1)
                return null;

            //当集合元素仅有两个时判断这两个元素是否存在可用组合中
            if(charOrder.Length == 2)
            {
                CharIdSet set = new CharIdSet(charOrder);
                if (canUseGroups.ContainsKey(set))
                    return new List<HashSet<int>>() { new HashSet<int>(set.charIds)};
            }
            else
            {
                //每次从剩余角色从按顺序抽出两名角色
                for (int a = 0; a < charOrder.Length; a++)
                {
                    for (int b = a + 1; b < charOrder.Length; b++)
                    {
                        int charAId = charOrder[a];
                        int charBId = charOrder[b];
                        List<int> leftCharIds = new List<int>(charOrder);
                        leftCharIds.Remove(charAId);
                        leftCharIds.Remove(charBId);

                        //判断剩余角色构成的序列是否可用
                        List<HashSet<int>> outList = GetAvailableSequence(leftCharIds.ToArray());
                        if (outList != null)
                        {
                            //如果剩余角色构成的序列可用，则判断a、b是否可构成可用序列
                            CharIdSet set = new CharIdSet( charAId, charBId );
                            if (canUseGroups.ContainsKey(set))
                            {
                                outList.Add(new HashSet<int>(set.charIds));
                                return outList;
                            }
                        }
                    }
                }
            }


            return null;
        }

        List<HashSet<int>> GetAvailableSequence_Ver2()
        {
            List<HashSet<int>> canUseGroups = new  List<HashSet<int>>(
                this.canUseGroups.Select(cis => cis.Key.charIds));
            int[] rdmArray = MathTools.Getrandomarray(canUseGroups.Count);
            canUseGroups = new List<HashSet<int>>(rdmArray.Select(id => canUseGroups[id]));

            HashSet<int> usedCharId = new HashSet<int>();
            HashSet<int> unusedCharId = new HashSet<int>();
            for (int i = 1; i < 21; i++)
            {
                unusedCharId.Add(i);
            }

            List<int> list = GetAvailableSequence_Ver2_Iter(canUseGroups, 0, usedCharId, unusedCharId);
            return list == null ? null : new List<HashSet<int>>(list.Select((id) => canUseGroups[id]));
        }

        List<int> GetAvailableSequence_Ver2_Iter(List<HashSet<int>> canUseGroups, int startIndex, HashSet<int> usedCharId, HashSet<int> unusedCharId)
        {
            for (int i = startIndex; i < canUseGroups.Count; i++)
            {
                if (canUseGroups[i].Overlaps(usedCharId))
                    continue;

                usedCharId.UnionWith(canUseGroups[i]);
                unusedCharId.ExceptWith(canUseGroups[i]);

                if (unusedCharId.Count == 0)
                    return new List<int>() { i };

                List<int> list = GetAvailableSequence_Ver2_Iter(canUseGroups, i, usedCharId, unusedCharId);
                if (list == null)
                {
                    usedCharId.ExceptWith(canUseGroups[i]);
                    unusedCharId.UnionWith(canUseGroups[i]);
                }
                else
                {
                    list.Add(i);
                    return list;
                }
            }
            return null;
        }

        int FindNextAvailableGroup(List<HashSet<int>> canUseGroups,int startIndex, HashSet<int> usedCharId)
        {
            for (int i = startIndex; i < canUseGroups.Count; i++)
            {
                if (!canUseGroups[i].Overlaps(usedCharId))
                    return i;
            }
            return -1;
        }

        int[] GetRdmCharOrder()
        {
            int charCount = 20;
            List<int> charInOrder = new List<int>();
            for (int i = 0; i < charCount; i++)
            {
                charInOrder.Add(i + 1);
            }

            List<int> outList = new List<int>();
            while (charInOrder.Count != 0)
            {
                int rdmIdx = Random.Range(0, charInOrder.Count);
                outList.Add(charInOrder[rdmIdx]);
                charInOrder.RemoveAt(rdmIdx);
            }
            return outList.ToArray();

            //return charInOrder.ToArray();
        }

        public void Apply()
        {
            int[] rdmCharOrder = GetRdmCharOrder();
            List<HashSet<int>> availableSequences = GetAvailableSequence_Ver2();
            if (availableSequences == null)
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, "未能找到符合条件的序列");
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int charAId, charBId;
                    int[] pair = availableSequences[i].ToArray();
                    if (Random.Range(0, 2) == 0)
                    {
                        charAId = pair[0]; charBId = pair[1];
                    }
                    else
                    {
                        charAId = pair[1]; charBId = pair[0];
                    }

                    List<string> wordsList = canUseGroups[new CharIdSet(pair)];
                    string bondsWorld = wordsList[Random.Range(0, wordsList.Count)];
                    if (bondsWorld.EndsWith("チーム"))
                    {
                        bondsWorld = bondsWorld.Substring(0, bondsWorld.Length - 3);
                    }
                    else if (bondsWorld.EndsWith("隊"))
                    {
                        bondsWorld = bondsWorld.Substring(0, bondsWorld.Length - 1);
                    }
                    else if (bondsWorld.StartsWith("チーム"))
                    {
                        bondsWorld = bondsWorld.Substring(3, bondsWorld.Length - 3);
                    }

                    items[i].SetData(charAId, charBId, bondsWorld);
                    if (settings.useRefreshEffect)
                    {
                        masks[i].SetData(charAId, charBId);
                        particles[i].SetData(charAId, charBId);
                    }
                    if (settings.useRefreshEffect)
                    {
                        StopAllCoroutines();
                        StartCoroutine(CoRefreshEffect());
                    }
                }
            }
        }

        IEnumerator CoRefreshEffect()
        {
            foreach (var mask in masks)
            {
                mask.ResetColor();
            }
            yield return new WaitForSeconds(settings.refreshEffectPlayAfter);
            for (int i = 0; i < 10; i++)
            {
                masks[i].Play();
                particles[i].Play();
                yield return new WaitForSeconds(settings.refreshEffectDelay);
            }
        }

        public class CharIdSet
        {
            public readonly HashSet<int> charIds;

            public CharIdSet(HashSet<int> charIds)
            {
                this.charIds = charIds;
            }

            public CharIdSet(params int[] charIds)
            {
                this.charIds = new HashSet<int>(charIds);
            }

            public override bool Equals(object obj)
            {
                CharIdSet charIdSet = obj as CharIdSet;
                if (charIdSet != null)
                    return charIdSet.charIds.SetEquals(charIds);
                return base.Equals(obj);
            }

            public override int GetHashCode() => 0;
        }
    }
}