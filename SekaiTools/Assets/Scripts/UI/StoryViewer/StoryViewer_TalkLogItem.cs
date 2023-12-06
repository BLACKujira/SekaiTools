using SekaiTools.Count;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.StoryViewer
{
    public class StoryViewer_TalkLogItem : MonoBehaviour
    {
        [System.NonSerialized] public int referenceIndex;
        [Header("Components")]
        public Image iconImage;
        public Text nameLabel;
        public Text serifText;
        public Image edgeImage;
        public Image bgImage;
        [Header("Settings")]
        public Color defaultBGColor = new Color32(255, 255, 238, 255);
        public Color defaultEdgeColor = new Color32(204, 238, 238, 255);
        public Color highLightBGColor = Color.white;
        public Color highLightEdgeColor = new Color32(67, 67, 101, 255);
        public float selectedBGLerpT = 0.6f;
        public IconSet iconSet;

        RectTransform _rectTransform;
        public RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

        public void Initialize(BaseTalkData baseTalkData, bool highLight)
        {
            referenceIndex = baseTalkData.referenceIndex;
            iconImage.sprite = iconSet.icons[baseTalkData.characterId];
            nameLabel.text = baseTalkData.windowDisplayName;
            serifText.text = baseTalkData.serif;

            if(highLight)
            {
                edgeImage.color = highLightEdgeColor;
                bgImage.color = highLightBGColor;
            }
            else
            {
                edgeImage.color = defaultEdgeColor;
                bgImage.color = defaultBGColor;
            }
        }

        public void RefreshLayout()
        {
            ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();
            contentSizeFitter.enabled = false;
            contentSizeFitter.enabled = true;
        }
    }
}