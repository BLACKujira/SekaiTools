using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;

namespace SekaiTools.UI.NCEWindow
{
    public class NCESingle : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform contentTransform;
        public VerticalLayoutGroup verticalLayoutGroup;
        public Image countNumberBG;
        public Text countNumber;
        public Toggle toggleScreening;
        [Header("Prefab")]
        public NCESingle_TalkLogItem talkLogItemPrefab;
        public NCESingle_TalkLogItem talkLogItemEmptyPrefab;

        NicknameCountMatrix nicknameCountMatrix;
        int talkerId;
        int nameId;
        List<NCESingle_TalkLogItem> talkLogItems = new List<NCESingle_TalkLogItem>();

        public void Initialize(NicknameCountMatrix nicknameCountMatrix, int talkerId,int nameId)
        {
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.talkerId = talkerId;
            this.nameId = nameId;
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