using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class UniversalGenerator : MonoBehaviour
    {
        public List<GameObject> items;
        public enum Direction { Vertical, Horizontal }

        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public GameObject itemPrefab;
        [Header("Settings")]
        public float blank;
        public float distance;
        public Direction direction = Direction.Vertical;

        public void Generate(int count, Action<GameObject, int> initialize)
        {
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);
            for (int i = 0; i < count; i++)
            {
                int id = i;
                GameObject item = Instantiate(itemPrefab, scorllContent);
                item.GetComponent<RectTransform>().anchoredPosition = direction == Direction.Vertical ?
                    new Vector2(0, -distance * i - blank) :
                    new Vector2(distance * i + blank, 0);
                initialize(item, id);
                items.Add(item);
            }
        }

        public void ClearItems()
        {
            foreach (var item in items)
            {
                Destroy(item);
            }
            items = new List<GameObject>();
        }

        public void AddItem(GameObject prefab, Action<GameObject> initialize)
        {
            float count = items.Count + 1;
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);

            count--;
            GameObject item = Instantiate(prefab, scorllContent);
            item.GetComponent<RectTransform>().anchoredPosition = direction == Direction.Vertical ?
                new Vector2(0, -distance * count - blank) :
                new Vector2(distance * count + blank, 0);
            if (initialize != null) initialize(item);
            items.Add(item);
        }
        public void AddItem(Action<GameObject> initialize)
        {
            AddItem(itemPrefab, initialize);
        }
    }
}