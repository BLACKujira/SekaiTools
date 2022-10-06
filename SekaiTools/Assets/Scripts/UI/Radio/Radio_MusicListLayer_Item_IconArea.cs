using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    public class Radio_MusicListLayer_Item_IconArea : MonoBehaviour
    {
        [Header("Settings")]
        public float distance;
        public IconSet iconSet;
        [Header("Prafab")]
        public Image iconPrefab;

        public void SetIcons(MusicTagData musicTagData)
        {
            MusicTag[] sortedTags = musicTagData.SortedTags;
            float posX = 0;
            foreach (var musicTag in sortedTags)
            {
                if (musicTag == MusicTag.all) continue;
                Image image = Instantiate(iconPrefab, transform);
                image.sprite = iconSet.icons[(int)musicTag];
                image.rectTransform.anchoredPosition = new Vector2
                    (posX,image.rectTransform.anchoredPosition.y);
                posX -= distance;
            }
        }
    }
}