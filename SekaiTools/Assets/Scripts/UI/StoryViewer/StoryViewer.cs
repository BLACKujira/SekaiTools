using SekaiTools.Count;
using SekaiTools.UI.NCEWindow;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

namespace SekaiTools.UI.StoryViewer
{
    public class StoryViewer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform contentTransform;
        public VerticalLayoutGroup verticalLayoutGroup;
        public Text lblStoryName;
        public Text lblPublishedAt;
        public Toggle toggleScreening;
        [Header("Prefab")]
        public StoryViewer_TalkLogItem talkLogItemPrefab;
        public StoryViewer_TalkLogItem talkLogItemEmptyPrefab;

        StoryManager storyManager;
        protected List<StoryViewer_TalkLogItem> talkLogItems = new List<StoryViewer_TalkLogItem>();

        public void Initialize(StoryManager storyManager)
        {
            this.storyManager = storyManager;
        }

        public void Refresh()
        {
            foreach (var item in talkLogItems)
            {
                Destroy(item.gameObject);
            }
            talkLogItems = new List<StoryViewer_TalkLogItem>();
            BaseTalkData[] baseTalkDatas = storyManager.GetTalkDatas();
            if(toggleScreening.isOn)
            {
                baseTalkDatas = baseTalkDatas
                    .Where(btd => storyManager.highLightRefIdx.Contains(btd.referenceIndex))
                    .ToArray();
            }

            foreach (var talkData in baseTalkDatas)
            {
                StoryViewer_TalkLogItem talkLogItem = Instantiate(talkLogItemPrefab, contentTransform);
                talkLogItem.Initialize(talkData, storyManager.highLightRefIdx.Contains(talkData.referenceIndex));
                talkLogItems.Add(talkLogItem);
            }
            if (talkLogItems.Count == 0) talkLogItems.Add(Instantiate(talkLogItemEmptyPrefab, contentTransform));

            verticalLayoutGroup.enabled = false;
            verticalLayoutGroup.enabled = true;

            StartCoroutine(CoRefresh());
        }

        IEnumerator CoRefresh()
        {
            toggleScreening.interactable = false;
            yield return 1;
            yield return 1;
            toggleScreening.interactable = true;
            talkLogItems[0].RefreshLayout();
        }

        public void OpenSVUrl()
        {
            System.Diagnostics.Process.Start(storyManager.sekaiViewerUrl);
        }
    }
}