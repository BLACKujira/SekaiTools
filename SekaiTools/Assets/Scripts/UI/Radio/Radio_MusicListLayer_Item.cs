using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    [RequireComponent(typeof(RectTransform))]
    public class Radio_MusicListLayer_Item : MonoBehaviour
    {
        [Header("Components")]
        public List<Graphic> graphics_Dark;
        public List<Graphic> graphics_Light;
        public Text text_title;
        public Text text_artist;
        public Radio_MusicListLayer_Item_IconArea iconArea;
        [Header("Settings")]
        public ColorSet colorSet_Dark;
        public ColorSet colorSet_Light;
        public float vocalItemYPosition;
        public float vocalItemDistance;
        public int maxAliasChars = 40;
        public int aliasFontSize = 23;
        [Header("Prefab")]
        public Radio_MusicListLayer_Item_VocalItem vocalItemPrafab;

        RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public void Initialize(MusicData musicData, MusicVocalData[] musicVocalDatas,string[] alias)
        {
            MasterMusic masterMusic = musicData.masterMusic;
            string titleStr = $"{masterMusic.id.ToString("000")} {masterMusic.title}";
            if (alias != null && alias.Length > 0)
                titleStr += $"  <size={aliasFontSize}>{string.Join("¡¢", alias)}</size>";
            text_title.text = titleStr;
            text_artist.text = $"±àÇú {masterMusic.arranger}  ×÷Çú {masterMusic.composer}  ×÷´Ê {masterMusic.lyricist}";
            iconArea.SetIcons(musicData.musicTag);

            int topPriorityTagId = (int)musicData.musicTag.TopPriorityTag;

            foreach (var graphic in graphics_Dark)
                graphic.color = colorSet_Dark[topPriorityTagId];
            foreach (var graphic in graphics_Light)
                graphic.color = colorSet_Light[topPriorityTagId];

            float posY = vocalItemYPosition;
            float lengthDelta = 0;
            foreach (var musicVocalData in musicVocalDatas)
            {
                Radio_MusicListLayer_Item_VocalItem radio_MusicListLayer_Item_VocalItem = Instantiate(vocalItemPrafab, transform);
                radio_MusicListLayer_Item_VocalItem.Initialize(musicVocalData);
                radio_MusicListLayer_Item_VocalItem.rectTransform.anchoredPosition = new Vector2
                    (radio_MusicListLayer_Item_VocalItem.rectTransform.anchoredPosition.x, posY);
                posY -= radio_MusicListLayer_Item_VocalItem.rectTransform.sizeDelta.y;
                posY -= vocalItemDistance;
                lengthDelta += radio_MusicListLayer_Item_VocalItem.rectTransform.sizeDelta.y;
                lengthDelta += vocalItemDistance;
            }
            rectTransform.sizeDelta = new Vector2(
                rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y + lengthDelta - vocalItemDistance);
        }
    }
}