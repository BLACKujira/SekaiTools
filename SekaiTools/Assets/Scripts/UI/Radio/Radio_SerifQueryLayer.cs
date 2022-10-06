using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using UnityEngine.UI;
using SekaiTools.StringConverter;
using System.Linq;
using System.Text.RegularExpressions;
using SekaiTools.DecompiledClass;
using System.Threading;
using System;
using DG.Tweening;

namespace SekaiTools.UI.Radio
{

    public class Radio_SerifQueryLayer : Radio_OptionalLayer , IReturnableWindow
    {
        [Header("Components")]
        public RadioReturnableWindowController returnableWindowController;
        public RectTransform targetRectTransform;
        public Text textTitle;
        public AutoScroll autoScroll;
        [Header("Prefab")]
        public TalkLogItem talkLogItemPrefab;
        public RectTransform talkLogLabelPrefab;

        NicknameCountData nicknameCountData;

        StringConverter_StringAlias filterNames;
        StringConverter_StringAlias areaNames;
        StringConverter_CharacterName charNames;

        MasterArea[] masterAreas;
        MasterActionSet[] masterActionSets;
        MasterVirtualLive[] masterVirtualLives;
        MasterEvent[] masterEvents;
        MasterCard[] masterCards;

        List<GameObject> displayItems = new List<GameObject>();
        Dictionary<NicknameCountMatrix, string> labelTexts = new Dictionary<NicknameCountMatrix, string>();

        public ReturnPermission ReturnPermission => returnableWindowController.ReturnPermission;
        public string SenderUserName { get; set; }

        public delegate bool ApplyFilter(List<NicknameCountMatrix> countMatricesIn, int charA, int charB, string fitter, out List<NicknameCountMatrix> countMatricesOut);

        #region 查询
        void QueryProcess(int charA, int charB, string[] filters, ref List<NicknameCountMatrix> nicknameCountMatrices)
        {
            nicknameCountMatrices = ApplyFilter_NotZero(nicknameCountMatrices, charA, charB);

            ApplyFilter[] applyFilters =
            {
                ApplyFilter_Base,
                ApplyFilter_MapArea,
                ApplyFilter_Extra
            };
            foreach (var filter in filters)
            {
                bool flag = false;
                foreach (var applyFilter in applyFilters)
                {
                    if (applyFilter(nicknameCountMatrices, charA, charB, filter, out List<NicknameCountMatrix> filteredCountMatrixNext))
                    {
                        nicknameCountMatrices = filteredCountMatrixNext;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    nicknameCountMatrices = new List<NicknameCountMatrix>();
                    break;
                }
            }
        }

        public SerifQueryResult Query(SerifQueryInfo serifQueryInfo)
        {
            int charA = charNames.GetValue(serifQueryInfo.charA);
            int charB = charNames.GetValue(serifQueryInfo.charB);
            if (charA == -1 || charB == -1)
            {
                return new SerifQueryResult(-1, -1, serifQueryInfo, new List<NicknameCountMatrix>());
            }

            List<NicknameCountMatrix> processCountMatrices = new List<NicknameCountMatrix>(nicknameCountData.nicknameCountMatrices);
            QueryProcess(charA, charB, serifQueryInfo.filters, ref processCountMatrices);
            return new SerifQueryResult(charA, charB, serifQueryInfo, processCountMatrices);
        }

        public List<NicknameCountMatrix> ApplyFilter_NotZero(List<NicknameCountMatrix> nicknameCountMatrices, int charA, int charB)
        {
            return new List<NicknameCountMatrix>(
                from NicknameCountMatrix countMatrix in nicknameCountMatrices
                where countMatrix[charA, charB].matchedIndexes.Count > 0
                select countMatrix);
        }

        public bool ApplyFilter_Base(List<NicknameCountMatrix> countMatricesIn, int charA, int charB, string fitter, out List<NicknameCountMatrix> countMatricesOut)
        {
            fitter = filterNames.GetValue(fitter);
            if (!string.IsNullOrEmpty(fitter))
            {
                switch (fitter)
                {
                    case "unit":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.UnitStory
                            select countMatrix);
                        break;
                    case "event":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.EventStory
                            select countMatrix);
                        break;
                    case "card":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.CardStory
                            select countMatrix);
                        break;
                    case "map":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.MapTalk
                            select countMatrix);
                        break;
                    case "live":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.LiveTalk
                            select countMatrix);
                        break;
                    case "other":
                        countMatricesIn = new List<NicknameCountMatrix>(
                            from NicknameCountMatrix countMatrix in countMatricesIn
                            where countMatrix.storyType == StoryType.OtherStory
                            select countMatrix);
                        break;
                    case "maxev":
                        NicknameCountItemByEvent nicknameCountItemByEvent = nicknameCountData.GetCountItemByEvent(charA, charB);
                        KeyValuePair<int, int> eventMost = new KeyValuePair<int, int>(-1, 0);
                        foreach (var keyValuePair in nicknameCountItemByEvent.countDictionary)
                        {
                            if (keyValuePair.Value > eventMost.Value) eventMost = keyValuePair;
                        }
                        if (eventMost.Key == -1) countMatricesIn = new List<NicknameCountMatrix>();
                        else
                        {
                            countMatricesIn = new List<NicknameCountMatrix>(
                                from NicknameCountMatrix countMatrix in countMatricesIn
                                where countMatrix.storyType == StoryType.EventStory &&
                                ConstData.IsEventStory(countMatrix.fileName)?.eventId == eventMost.Key
                                select countMatrix);
                        }
                        break;
                    case "maxep":
                        KeyValuePair<NicknameCountMatrix, int> episodeMost = new KeyValuePair<NicknameCountMatrix, int>(null, 0);
                        foreach (var nicknameCountMatrix in countMatricesIn)
                        {
                            int count = nicknameCountMatrix[charA, charB].matchedIndexes.Count;
                            if (count > episodeMost.Value)
                                episodeMost = new KeyValuePair<NicknameCountMatrix, int>(nicknameCountMatrix, count);
                        }
                        countMatricesIn = new List<NicknameCountMatrix>() { episodeMost.Key };
                        break;
                    default:
                        countMatricesOut = null;
                        return false;
                }
            }
            else
            {
                countMatricesOut = null;
                return false;
            }

            countMatricesOut = countMatricesIn;
            return true;
        }

        public bool ApplyFilter_MapArea(List<NicknameCountMatrix> countMatricesIn, int charA, int charB, string fitter, out List<NicknameCountMatrix> countMatricesOut)
        {
            string areaIdStr = areaNames.GetValue(fitter);
            if (areaIdStr != null)
            {
                int areaId = int.Parse(areaIdStr);
                HashSet<string> conversationsInArea = new HashSet<string>();
                foreach (var masterActionSet in masterActionSets)
                {
                    if (masterActionSet.areaId == areaId)
                        conversationsInArea.Add(masterActionSet.scenarioId);
                }
                countMatricesIn = new List<NicknameCountMatrix>(
                    from NicknameCountMatrix countMatrix in countMatricesIn
                    where countMatrix.storyType == StoryType.MapTalk
                    && conversationsInArea.Contains(countMatrix.fileName)
                    select countMatrix);
                countMatricesOut = countMatricesIn;
                return true;
            }
            else
            {
                countMatricesOut = null;
                return false;
            }
        }

        /// <summary>
        ///分类-活动ID 
        /// </summary>
        Regex regex_EvStory = new Regex("ev_[0-9]+");
        /// <summary>
        ///分类-活动ID-集数 
        /// </summary>
        Regex regex_EvStoryEpisode = new Regex("ev_[0-9]+_[0-9]+");
        /// <summary>
        ///分类-角色卡牌 
        /// </summary>
        Regex regex_CharacterCard = new Regex("card_[0-9]+");

        public bool ApplyFilter_Extra(List<NicknameCountMatrix> countMatricesIn, int charA, int charB, string fitter, out List<NicknameCountMatrix> countMatricesOut)
        {
            if (regex_EvStoryEpisode.IsMatch(fitter))
            {
                string[] ev_ep = fitter.Split('_');
                if (int.TryParse(ev_ep[1], out int ev) && int.TryParse(ev_ep[2], out int ep))
                {
                    foreach (var nicknameCountMatrix in countMatricesIn)
                    {
                        if (nicknameCountMatrix.storyType == StoryType.EventStory)
                        {
                            EventStoryInfo eventStoryInfo = ConstData.IsEventStory(nicknameCountMatrix.fileName);
                            if (eventStoryInfo != null && eventStoryInfo.eventId == ev && eventStoryInfo.chapter == ep)
                                countMatricesIn = new List<NicknameCountMatrix>() { nicknameCountMatrix };
                        }
                    }
                }
                else countMatricesIn = new List<NicknameCountMatrix>();
            }
            else if (regex_EvStory.IsMatch(fitter))
            {
                string[] evStr = fitter.Split('_');
                if (int.TryParse(evStr[1], out int ev))
                {
                    countMatricesIn = new List<NicknameCountMatrix>(
                        from NicknameCountMatrix countMatrix in countMatricesIn
                        where countMatrix.storyType == StoryType.EventStory
                        && ConstData.IsEventStory(countMatrix.fileName)?.eventId == ev
                        select countMatrix
                        );
                }
                else countMatricesIn = new List<NicknameCountMatrix>();
            }
            else if (regex_CharacterCard.IsMatch(fitter))
            {
                string[] charIdStr = fitter.Split('_');
                if (int.TryParse(charIdStr[1], out int charId))
                {
                    countMatricesIn = new List<NicknameCountMatrix>(
                    from NicknameCountMatrix countMatrix in countMatricesIn
                    where countMatrix.storyType == StoryType.CardStory
                    && ConstData.IsCardStory(countMatrix.fileName)?.charId == charId
                    select countMatrix);
                }
                else countMatricesIn = new List<NicknameCountMatrix>();
            }
            else
            {
                countMatricesOut = null;
                return false;
            }
            countMatricesOut = countMatricesIn;
            return true;
        }
        #endregion

        public void Initialize(Settings settings)
        {
            base.Initialize(settings);

            nicknameCountData = settings.nicknameCountData;

            filterNames = settings.filterNames;
            areaNames = settings.areaNames;
            charNames = settings.charNames;

            masterAreas = settings.masterAreas;
            masterActionSets = settings.masterActionSets;
            masterVirtualLives = settings.masterVirtualLives;
            masterEvents = settings.masterEvents;
            masterCards = settings.masterCards;

            InitializeLabelText();
        }

        private void InitializeLabelText()
        {
            const string strVS = " 虚拟歌手";
            foreach (var countMatrix in nicknameCountData.countMatrix_Unit)
            {
                UnitStoryInfo unitStoryInfo = ConstData.IsUnitStory(countMatrix.fileName);
                string valueStr;
                if (unitStoryInfo != null)
                {
                    string unitStr = string.Empty;
                    switch (unitStoryInfo.unit)
                    {
                        case "leo": unitStr = ConstData.units[Unit.Leoneed].name; break;
                        case "mmj": unitStr = ConstData.units[Unit.MOREMOREJUMP].name; break;
                        case "street": unitStr = ConstData.units[Unit.VividBADSQUAD].name; break;
                        case "wonder": unitStr = ConstData.units[Unit.WonderlandsShowtime].name; break;
                        case "nightcode": unitStr = ConstData.units[Unit.NightCord].name; break;
                        case "vs": unitStr = ConstData.units[Unit.VirtualSinger].name; break;
                        case "vsleo": unitStr = ConstData.units[Unit.Leoneed].name + strVS; break;
                        case "vsmmj": unitStr = ConstData.units[Unit.MOREMOREJUMP].name + strVS; break;
                        case "vsstreet": unitStr = ConstData.units[Unit.VividBADSQUAD].name + strVS; break;
                        case "vswonder": unitStr = ConstData.units[Unit.WonderlandsShowtime].name + strVS; break;
                        case "vsnightcode": unitStr = ConstData.units[Unit.NightCord].name + strVS; break;
                        default: unitStr = "无法识别的组合"; break;
                    }
                    valueStr = $"主线剧情 {unitStr} 第{unitStoryInfo.chapter}话 [{countMatrix.fileName}]";
                }
                else
                {
                    valueStr = $"未知主线剧情 [{countMatrix.fileName}]";
                }
                labelTexts[countMatrix] = valueStr;
            }
            foreach (var countMatrix in nicknameCountData.countMatrix_Event)
            {
                EventStoryInfo eventStoryInfo = ConstData.IsEventStory(countMatrix.fileName);
                string valueStr;
                if (eventStoryInfo != null)
                {
                    string evStr = null;
                    foreach (var masterEvent in masterEvents)
                    {
                        if (masterEvent.id == eventStoryInfo.eventId)
                        {
                            evStr = masterEvent.name;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(evStr))
                        valueStr = $"未知活动 第{eventStoryInfo.eventId}期 第{eventStoryInfo.chapter}话 [{countMatrix.fileName}]";
                    else
                        valueStr = $"第{eventStoryInfo.eventId}期活动 {evStr} 第{eventStoryInfo.chapter}话 [{countMatrix.fileName}]";
                }
                else
                {
                    valueStr = $"未知活动剧情 [{countMatrix.fileName}]";
                }
                labelTexts[countMatrix] = valueStr;
            }
            foreach (var countMatrix in nicknameCountData.countMatrix_Card)
            {
                CardStoryInfo cardStoryInfo = ConstData.IsCardStory(countMatrix.fileName);
                MasterCard card = null;
                string valueStr;
                if (cardStoryInfo != null)
                {
                    string assetbundleName = $"res{cardStoryInfo.charId.ToString("000")}_no{cardStoryInfo.cardId.ToString("000")}";
                    foreach (var masterCard in masterCards)
                    {
                        if (masterCard.assetbundleName.Equals(assetbundleName))
                        {
                            card = masterCard;
                            break;
                        }
                    }
                }
                if (card != null)
                    valueStr = $"卡片剧情 {ConstData.characters[cardStoryInfo.charId].Name} {card.prefix} {(cardStoryInfo.chapter == 1 ? "前篇" : "后篇")} [{countMatrix.fileName}]";
                else
                    valueStr = $"未知卡片剧情 [{countMatrix.fileName}]";
                labelTexts[countMatrix] = valueStr;
            }
            foreach (var countMatrix in nicknameCountData.countMatrix_Map)
            {
                MasterActionSet actionSet = null;
                MasterArea area = null;
                string valueStr;
                foreach (var masterActionSet in masterActionSets)
                {
                    if (countMatrix.fileName.Equals(masterActionSet.scenarioId))
                    {
                        actionSet = masterActionSet;
                        break;
                    }
                }

                if (actionSet != null)
                {
                    foreach (var masterArea in masterAreas)
                    {
                        if (masterArea.id == actionSet.areaId)
                        {
                            area = masterArea;
                            break;
                        }
                    }
                    List<string> charNames = new List<string>();
                    foreach (var charId in actionSet.characterIds)
                    {
                        int mergedId = ConstData.MergeVirtualSinger(charId);
                        if (mergedId >= 1 && mergedId <= 26)
                        {
                            charNames.Add(ConstData.characters[mergedId].namae);
                        }
                        else
                        {
                            charNames.Add($"未知角色{mergedId}");
                        }
                    }

                    valueStr = $"地图对话 {string.Join("、", charNames)} 在{(area == null ? "未知区域" : area.name)} [{countMatrix.fileName}]";
                }
                else
                {
                    valueStr = $"未知区域对话 [{countMatrix.fileName}]";
                }
                labelTexts[countMatrix] = valueStr;
            }
            foreach (var countMatrix in nicknameCountData.countMatrix_Live)
            {
                string valueStr;
                MasterVirtualLive virtualLive = null;
                foreach (var masterVirtualLive in masterVirtualLives)
                {
                    foreach (var virtualLiveSetlist in masterVirtualLive.virtualLiveSetlists)
                    {
                        bool flag = false;
                        if (virtualLiveSetlist.assetbundleName.Equals(countMatrix.fileName))
                        {
                            virtualLive = masterVirtualLive;
                            flag = true;
                            break;
                        }
                        if (flag) break;
                    }
                }

                if (virtualLive != null)
                    valueStr = $"Live对话 {virtualLive.name} [{countMatrix.fileName}]";
                else
                    valueStr = $"未知Live对话 [{countMatrix.fileName}]";
                labelTexts[countMatrix] = valueStr;
            }
            foreach (var countMatrix in nicknameCountData.countMatrix_Other)
            {
                string valueStr;
                valueStr = $"其他剧情 [{countMatrix.fileName}]";
                labelTexts[countMatrix] = valueStr;
            }
        }

        public void SetDisplayQuery(SerifQueryResult serifQueryResult)
        {
            int count = 0;
            foreach (var nicknameCountMatrix in serifQueryResult.resultCountMatrices)
            {
                count += nicknameCountMatrix[serifQueryResult.charAId, serifQueryResult.charBId].matchedIndexes.Count;
            }

            string title = $"对话查询 {ConstData.characters[serifQueryResult.charAId].namae} → {ConstData.characters[serifQueryResult.charBId].namae}";
            if (serifQueryResult.serifQueryInfo.filters.Length != 0)
                title += $" 筛选器{string.Join(" ", serifQueryResult.serifQueryInfo.filters)}";
            title += $" 总计{count}次";
            textTitle.text = title;


            foreach (var displayItem in displayItems)
            {
                Destroy(displayItem);
            }
            displayItems = new List<GameObject>();

            int displayCount = 0;
            foreach (var countMatrix in serifQueryResult.resultCountMatrices)
            {
                RectTransform labelTF = Instantiate(talkLogLabelPrefab, targetRectTransform);
                labelTF.GetComponentInChildren<Text>().text = labelTexts[countMatrix];
                displayItems.Add(labelTF.gameObject);

                BaseTalkData[] baseTalkDatas = countMatrix.GetTalkDatas(serifQueryResult.charAId, serifQueryResult.charBId);
                foreach (var baseTalkData in baseTalkDatas)
                {
                    TalkLogItem talkLogItem = Instantiate(talkLogItemPrefab, targetRectTransform);
                    talkLogItem.Initialize(baseTalkData);
                    displayItems.Add(talkLogItem.gameObject);
                    displayCount++;
                }
                //if (displayCount > maxDisplaySerifs)
                //    break;
            }

            //if (displayCount < count)
            //{
            //    RectTransform labelTF = Instantiate(talkLogLabelPrefab, targetRectTransform);
            //    labelTF.GetComponentInChildren<Text>().text = $"仅显示前{displayCount}次，还有{count - displayCount}次";
            //    displayItems.Add(labelTF.gameObject);
            //}

            targetRectTransform.anchoredPosition = new Vector2(targetRectTransform.anchoredPosition.x, 0);
        }

        public void Play(string userName,Action onComplete)
        {
            SenderUserName = userName;
            StartCoroutine(IPlay(onComplete));
            returnableWindowController.ResetReturnPermission();
        }

        IEnumerator IPlay(Action onComplete)
        {
            yield return 1;
            yield return autoScroll.IPlay(onComplete);
        }

        public new class Settings : Radio_OptionalLayer.Settings
        {
            public NicknameCountData nicknameCountData;

            public StringConverter_StringAlias filterNames;
            public StringConverter_StringAlias areaNames;
            public StringConverter_CharacterName charNames;

            public MasterArea[] masterAreas;
            public MasterActionSet[] masterActionSets;
            public MasterVirtualLive[] masterVirtualLives;
            public MasterEvent[] masterEvents;
            public MasterCard[] masterCards;
        }
    }
}