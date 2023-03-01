using SekaiTools.Count;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCEMuti : NCEBase
    {
        public Window window;
        [Header("Components")]
        public RectTransform contentTransform;
        public VerticalLayoutGroup verticalLayoutGroup;
        public Text lblStoryName;
        public Text lblPublishedAt;
        public Toggle toggleScreening;
        public NCEMuti_Number numberArea;
        public Button btnMarkAll;
        [Header("Prefab")]
        public NCEMuti_TalkLogItem talkLogItemPrefab;
        public NCEMuti_TalkLogItem talkLogItemEmptyPrefab;
        public Window idMaskSelectorPrefab;

        protected NicknameCountMatrix nicknameCountMatrix;
        protected int talkerId;
        protected string ambiguityRegex;

        enum Mode { muti, full, ambiguity }
        Mode mode;

        protected List<NCEMuti_TalkLogItem> talkLogItems = new List<NCEMuti_TalkLogItem>();

        public void Initialize(NicknameCountMatrix nicknameCountMatrix, int talkerId, StoryDescriptionGetter storyDescriptionGetter)
        {
            mode = Mode.muti;
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.talkerId = talkerId;
            InitInfo(nicknameCountMatrix, storyDescriptionGetter);
        }

        public void Initialize(NicknameCountMatrix nicknameCountMatrix, string ambiguityRegex, StoryDescriptionGetter storyDescriptionGetter)
        {
            mode = Mode.ambiguity;
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.ambiguityRegex = ambiguityRegex;
            InitInfo(nicknameCountMatrix, storyDescriptionGetter);

            btnMarkAll.onClick.AddListener(() => MarkAll());
            Text btnText = btnMarkAll.GetComponentInChildren<Text>();
            if (btnText) btnText.text = $"将此剧情中的所有 \"{ambiguityRegex}\" 标记为...";
        }

        public void Initialize(NicknameCountMatrix nicknameCountMatrix, StoryDescriptionGetter storyDescriptionGetter)
        {
            mode = Mode.full;
            this.nicknameCountMatrix = nicknameCountMatrix;
            InitInfo(nicknameCountMatrix, storyDescriptionGetter);
        }

        private void InitInfo(NicknameCountMatrix nicknameCountMatrix, StoryDescriptionGetter storyDescriptionGetter)
        {
            if (lblStoryName != null) lblStoryName.text = storyDescriptionGetter.GetStroyDescription(nicknameCountMatrix.storyType, nicknameCountMatrix.fileName);
            if (lblPublishedAt != null) lblPublishedAt.text = $"剧情开始时间 {nicknameCountMatrix.PublishedAt}";
            Refresh();
        }

        public void RefreshCountNumber()
        {
            numberArea.SetData(nicknameCountMatrix[talkerId].NameCountArray);
        }

        public void Refresh()
        {
            foreach (var item in talkLogItems)
            {
                Destroy(item.gameObject);
            }
            talkLogItems = new List<NCEMuti_TalkLogItem>();

            BaseTalkData[] baseTalkDatas;
            if (mode == Mode.muti) baseTalkDatas = nicknameCountMatrix.GetTalkDatas(talkerId);
            else baseTalkDatas = nicknameCountMatrix.GetTalkDatas();

            HashSet<int> usedRefIdx = new HashSet<int>(
                nicknameCountMatrix[talkerId].nicknameCountGrids
                .SelectMany(ncg => ncg.matchedIndexes));

            if (toggleScreening.isOn)
            {
                if (mode == Mode.muti)
                {
                    baseTalkDatas = baseTalkDatas.Where((btd) => usedRefIdx.Contains(btd.referenceIndex)).ToArray();
                }

                if (mode == Mode.ambiguity)
                {
                    baseTalkDatas = baseTalkDatas
                        .Where((btd) => nicknameCountMatrix
                            .GetAmbiguitySerifSet(ambiguityRegex)?.matchedIndexes
                            .Contains(btd.referenceIndex) ?? false)
                        .ToArray();
                }
            }

            foreach (var talkData in baseTalkDatas)
            {
                NCEMuti_TalkLogItem talkLogItem = Instantiate(talkLogItemPrefab, contentTransform);
                talkLogItem.Initialize(talkData, nicknameCountMatrix);
                talkLogItem.onDataChanged.AddListener(() => RefreshCountNumber());
                talkLogItems.Add(talkLogItem);
            }
            if (talkLogItems.Count == 0) talkLogItems.Add(Instantiate(talkLogItemEmptyPrefab, contentTransform));
            verticalLayoutGroup.enabled = false;
            verticalLayoutGroup.enabled = true;
            RefreshCountNumber();
        }

        public void MarkAll()
        {
            CharIDMaskSelect.CharIDMaskSelect charIDMaskSelect
                = window.OpenWindow<CharIDMaskSelect.CharIDMaskSelect>(idMaskSelectorPrefab);

            charIDMaskSelect.Initialize((bool[] addCharIds) =>
            {
                IEnumerable<int> refIds = nicknameCountMatrix.GetTalkDatas()
                    .Where((btd) => nicknameCountMatrix
                        .GetAmbiguitySerifSet(ambiguityRegex)?.matchedIndexes
                         .Contains(btd.referenceIndex) ?? false)
                    .Select(btd => btd.referenceIndex);
                HashSet<int> refIdxSet = new HashSet<int>(refIds);

                foreach (var talkLogItem in talkLogItems)
                {
                    if (refIdxSet.Contains(talkLogItem.ReferenceIndex))
                    {
                        talkLogItem.MarkCharacterAdd(addCharIds);
                        talkLogItem.RefreshInfo();
                    }
                }
            });
            RefreshCountNumber();
        }
    }
}