using SekaiTools.Count;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class TalkLogItem : MonoBehaviour
    {
        [System.NonSerialized] public int referenceIndex;
        [Header("Components")]
        public Image iconImage;
        public Text nameLabel;
        public Text serifText;
        [Header("Settings")]
        public IconSet iconSet;

        RectTransform _rectTransform;
        public RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

        public void Initialize(BaseTalkData baseTalkData)
        {
            nameLabel.text = baseTalkData.windowDisplayName;
            iconImage.sprite = iconSet.icons[baseTalkData.characterId];
            serifText.text = baseTalkData.serif;
        }
    }
}