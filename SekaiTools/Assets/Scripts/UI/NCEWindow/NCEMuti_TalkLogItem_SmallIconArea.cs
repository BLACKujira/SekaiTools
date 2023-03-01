using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    public class NCEMuti_TalkLogItem_SmallIconArea : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform targetRt;
        [Header("Settings")]
        public float startX = 30;
        public float distance = 60;
        public IconSet iconSet;
        [Header("Prefab")]
        public Image imgIconPrefab;

        List<GameObject> iconList = new List<GameObject>();

        public void SetData(int[] characters)
        {
            foreach (var gobj in iconList) 
            {
                if(gobj != null) { Object.Destroy(gobj); }
            }

            iconList = new List<GameObject>();
            List<int> charList = new List<int>(characters);
            charList.Sort();
            for (int i = 0; i < charList.Count; i++)
            {
                Image image = Object.Instantiate(imgIconPrefab, targetRt);
                image.rectTransform.anchoredPosition = new Vector2(startX - i * distance, image.rectTransform.anchoredPosition.y);
                image.sprite = iconSet.icons[charList[i]];
                iconList.Add(image.gameObject);
            }
        }
    }
}