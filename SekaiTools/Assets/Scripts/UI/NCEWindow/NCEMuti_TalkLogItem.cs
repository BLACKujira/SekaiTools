using SekaiTools.Count;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCEMuti_TalkLogItem : MonoBehaviour
    {
        [Header("Components")]
        public Image iconImage;
        public Text nameLabel;
        public Text serifText;
        public Image edgeImage;
        public Image bgImage;
        public Button targetButton;
        public NCEMuti_TalkLogItem_SmallIconArea smallIconArea;
        [Header("Settings")]
        public Color defaultBGColor = new Color32(255, 255, 238, 255);
        public Color defaultEdgeColor = new Color32(204, 238, 238, 255);
        public Color selectedBGColor = Color.white;
        public Color selectedEdgeColor = new Color32(67, 67, 101, 255);
        public Color ambiguityBGColor = Color.white;
        public Color ambiguityEdgeColor = new Color32(67, 67, 101, 255);
        public IconSet iconSet;
        [Header("Prefab")]
        public Window idMaskSelectorPrefab;
        [Header("Events")]
        public UnityEvent onDataChanged;

        RectTransform _rectTransform;
        public RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

        [System.NonSerialized] public BaseTalkData baseTalkData;
        [System.NonSerialized] public NicknameCountMatrix nicknameCountMatrix;
        [System.NonSerialized] public string ambiguityRegex = null;

        public int ReferenceIndex => baseTalkData.referenceIndex;
        public int TalkerId => baseTalkData.characterId;
        public int[] MatchedCharIds
        {
            get
            {
                List<int> indexes = new List<int>();
                for (int i = 1; i < 27; i++)
                {
                    if (nicknameCountMatrix[TalkerId, i].matchedIndexes.Contains(ReferenceIndex))
                        indexes.Add(i);
                }
                return indexes.ToArray();
            }
        }

        public void Initialize(BaseTalkData baseTalkData, NicknameCountMatrix nicknameCountMatrix, string ambiguityRegex)
        {
            this.ambiguityRegex = ambiguityRegex;
            Initialize(baseTalkData, nicknameCountMatrix);
        }

        public void Initialize(BaseTalkData baseTalkData, NicknameCountMatrix nicknameCountMatrix)
        {
            this.baseTalkData = baseTalkData;
            this.nicknameCountMatrix = nicknameCountMatrix;

            iconImage.sprite = iconSet.icons[TalkerId <= 0 || TalkerId > 26 ? 0 : TalkerId];
            nameLabel.text = baseTalkData.windowDisplayName;
            serifText.text = baseTalkData.serif;

            targetButton.onClick.AddListener(() =>
            {
                if (TalkerId <= 0 || TalkerId > 26) return;

                CharIDMaskSelect.CharIDMaskSelect charIDMaskSelect
                    = WindowController.CurrentWindow.OpenWindow<CharIDMaskSelect.CharIDMaskSelect>(idMaskSelectorPrefab);
                charIDMaskSelect.Initialize(MatchedCharIds,
                    MarkCharacterOverride);
            });
            RefreshInfo();
        }

        void Select()
        {
            edgeImage.color = selectedEdgeColor;
            bgImage.color = selectedBGColor;
        }

        void UnSelect()
        {
            edgeImage.color = defaultEdgeColor;
            bgImage.color = defaultBGColor;
        }

        void Ambiguity()
        {
            edgeImage.color = ambiguityEdgeColor;
            bgImage.color = ambiguityBGColor;
        }

        public void RefreshInfo()
        {
            if (TalkerId <= 0 || TalkerId > 26) return;

            List<int> charIds = new List<int>();
            for (int i = 1; i < 27; i++)
            {
                NicknameCountGrid nicknameCountGrid = nicknameCountMatrix[TalkerId, i];
                if (nicknameCountGrid.matchedIndexes.Contains(ReferenceIndex))
                    charIds.Add(i);
            }

            if (nicknameCountMatrix.GetMatchedTimesOfAnyAmbiguitySerifSet(ReferenceIndex) > 0) { Ambiguity(); }
            else if (charIds.Count > 0) { Select(); }
            else { UnSelect(); }

            smallIconArea.SetData(charIds.ToArray());
        }

        public void MarkCharacterOverride(bool[] fixedCharIds)
        {
            if (fixedCharIds.Length != 27) throw new System.Exception("角色ID遮罩的长度不为27");
            for (int i = 1; i < 27; i++)
            {
                if (fixedCharIds[i] && !nicknameCountMatrix[TalkerId, i].matchedIndexes.Contains(ReferenceIndex))
                    nicknameCountMatrix[TalkerId, i].matchedIndexes.Add(ReferenceIndex);
                if (!fixedCharIds[i] && nicknameCountMatrix[TalkerId, i].matchedIndexes.Contains(ReferenceIndex))
                    nicknameCountMatrix[TalkerId, i].matchedIndexes.Remove(ReferenceIndex);

                if (string.IsNullOrEmpty(ambiguityRegex))
                {
                    foreach (var ambiguitySerifSet in nicknameCountMatrix.ambiguitySerifSets)
                    {
                        if (ambiguitySerifSet.matchedIndexes.Contains(ReferenceIndex))
                            ambiguitySerifSet.matchedIndexes.Remove(ReferenceIndex);
                    }
                }
                else
                {
                    AmbiguitySerifSet ambiguitySerifSet = nicknameCountMatrix.GetAmbiguitySerifSet(ambiguityRegex);
                    if (ambiguitySerifSet != null && ambiguitySerifSet.matchedIndexes.Contains(ReferenceIndex))
                    {
                        ambiguitySerifSet.matchedIndexes.Remove(ReferenceIndex);
                    }
                }

                nicknameCountMatrix.ifChanged = true;
                RefreshInfo();
                onDataChanged.Invoke();
            }
        }

        public void MarkCharacterAdd(bool[] addCharIds)
        {
            List<int> matchedCharIds = new List<int>(MatchedCharIds);
            addCharIds = new List<bool>(addCharIds).ToArray();
            for (int i = 0; i < addCharIds.Length; i++) 
            {
                if (matchedCharIds.Contains(i))
                    addCharIds[i] = true;
            }
            MarkCharacterOverride(addCharIds);
        }

        public void RefreshLayout()
        {
            ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();
            contentSizeFitter.enabled = false;
            contentSizeFitter.enabled = true;
        }
    }
}