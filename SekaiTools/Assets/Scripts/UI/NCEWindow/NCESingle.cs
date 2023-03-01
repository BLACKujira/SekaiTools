using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCESingle : NCEBase
    {
        public Window window;
        [Header("Components")]
        public RectTransform contentTransform;
        public VerticalLayoutGroup verticalLayoutGroup;
        public Image countNumberBG;
        public Text countNumber;
        public Text lblStoryName;
        public Text lblPublishedAt;
        public Toggle toggleScreening;
        [Header("Prefab")]
        public NCESingle_TalkLogItem talkLogItemPrefab;
        public NCESingle_TalkLogItem talkLogItemEmptyPrefab;

        protected NicknameCountMatrix nicknameCountMatrix;
        protected int talkerId;
        protected int nameId;
        protected List<NCESingle_TalkLogItem> talkLogItems = new List<NCESingle_TalkLogItem>();

        public void Initialize(NicknameCountMatrix nicknameCountMatrix, int talkerId, int nameId, StoryDescriptionGetter storyDescriptionGetter)
        {
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.talkerId = talkerId;
            this.nameId = nameId;
            if (lblStoryName != null) lblStoryName.text = storyDescriptionGetter.GetStroyDescription(nicknameCountMatrix.storyType, nicknameCountMatrix.fileName);
            if (lblPublishedAt != null) lblPublishedAt.text = $"剧情开始时间 {nicknameCountMatrix.PublishedAt}";
            countNumberBG.color = ConstData.characters[talkerId].imageColor;
            Refresh();
        }

        public void RefreshCountNumber()
        {
            countNumber.text = nicknameCountMatrix[talkerId, nameId].matchedIndexes.Count.ToString();
        }

        public void Refresh()
        {
            foreach (var item in talkLogItems)
            {
                Destroy(item.gameObject);
            }
            talkLogItems = new List<NCESingle_TalkLogItem>();

            BaseTalkData[] baseTalkDatas = toggleScreening.isOn?
                nicknameCountMatrix.GetTalkDatas(talkerId,nameId) : nicknameCountMatrix.GetTalkDatas(talkerId);
            foreach (var talkData in baseTalkDatas)
            {
                NCESingle_TalkLogItem talkLogItem = Instantiate(talkLogItemPrefab, contentTransform);
                talkLogItem.Initialize(talkData, nicknameCountMatrix,talkerId, nameId);
                talkLogItem.targetToggle.onValueChanged.AddListener((value) => RefreshCountNumber());
                talkLogItems.Add(talkLogItem);
            }
            if (talkLogItems.Count == 0) talkLogItems.Add(Instantiate(talkLogItemEmptyPrefab, contentTransform));

            verticalLayoutGroup.enabled = false;
            verticalLayoutGroup.enabled = true;

            RefreshCountNumber();
        }
    }
}