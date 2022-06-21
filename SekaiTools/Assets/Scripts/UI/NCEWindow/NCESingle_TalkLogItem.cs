using SekaiTools.Count;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCESingle_TalkLogItem : MonoBehaviour
    {
        [System.NonSerialized] public int referenceIndex;
        [Header("Components")]
        public Image iconImage;
        public Text nameLabel;
        public Text serifText;
        public Image edgeImage;
        public Image bgImage;
        public Toggle targetToggle;
        [Header("Settings")]
        public Color defaultBGColor = new Color32(255, 255, 238, 255);
        public Color defaultEdgeColor = new Color32(204, 238, 238, 255);
        public Color selectedBGColor = Color.white;
        public Color selectedEdgeColor = new Color32(67, 67, 101, 255);
        public IconSet iconSet;

        RectTransform _rectTransform;
        public RectTransform rectTransform { get{ if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();return _rectTransform; } }

        public void Initialize(BaseTalkData baseTalkData,NicknameCountMatrix nicknameCountMatrix,int talkerId,int nameId)
        {
            NicknameCountGrid nicknameCountGrid = nicknameCountMatrix[talkerId,nameId];

            referenceIndex = baseTalkData.referenceIndex;
            iconImage.sprite = iconSet.icons[baseTalkData.characterId];
            nameLabel.text = baseTalkData.windowDisplayName;
            serifText.text = baseTalkData.serif;
            selectedBGColor = ConstData.characters[nameId].imageColor;

            targetToggle.onValueChanged.AddListener((bool value) =>
            {
                if (value) Select();
                else UnSelect();
                nicknameCountMatrix.ifChanged = true;
            });

            if (nicknameCountGrid.matchedIndexes.Contains(referenceIndex))
                targetToggle.isOn = true;

            targetToggle.onValueChanged.AddListener((bool value) =>
            {
                if(value)
                {
                    if (!nicknameCountGrid.matchedIndexes.Contains(referenceIndex))
                        nicknameCountGrid.matchedIndexes.Add(referenceIndex);
                }
                else
                {
                    nicknameCountGrid.matchedIndexes.Remove(referenceIndex);
                }
            });
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
    }
}