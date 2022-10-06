using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class UniversalGeneratorV2 : MonoBehaviour
    {
        List<RectTransform> items = new List<RectTransform>();
        public List<RectTransform> Items => items;
        public enum Direction { Vertical, Horizontal }

        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Settings")]
        public float blankUp = 0;
        public float blankDown = 0;
        public float distance = 10;
        public Direction direction = Direction.Vertical;

        public float GetLength(int startAt,int endBefore)
        {
            float length = 0;
            bool flag = false;
            for (int i = startAt; i < endBefore; i++)
            {
                length += direction == Direction.Vertical ? items[i].sizeDelta.y : items[i].sizeDelta.x;
                length += distance;
                flag = true;
            }
            if (flag) length -= distance;
            return length;
        }

        public float GetLength(int startAt = 0)
        {
            return GetLength(startAt, items.Count - startAt);
        }

        public GameObject AddItem(GameObject prefab, Action<GameObject> initialize = null)
        {
            GameObject gameObject = Instantiate(prefab, scorllContent);
            if (initialize != null) initialize(gameObject);
            items.Add(gameObject.GetComponent<RectTransform>());
            RecalculatePositions();
            return gameObject;
        }

        public bool RemoveItem(GameObject item)
        {
            int index = -1;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].gameObject.Equals(item))
                {
                    index = i;
                    break;
                }
            }
            if(index!=-1)
            {
                Destroy(items[index].gameObject);
                items.RemoveAt(index);
                RecalculatePositions();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearItems()
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            items = new List<RectTransform>();
            RecalculatePositions();
        }

        public void RecalculatePositions()
        {
            float length = 0;
            bool flag = false;
            for (int i = 0; i < items.Count; i++)
            {
                flag = true;
                if(direction == Direction.Vertical)
                {
                    items[i].anchoredPosition = new Vector2(items[i].anchoredPosition.x, -length);
                    length += items[i].sizeDelta.y;
                }
                else
                {
                    items[i].anchoredPosition = new Vector2(length, items[i].anchoredPosition.y);
                    length += items[i].sizeDelta.x;
                }
                length += distance;
            }
            if (flag) length -= distance;
            scorllContent.sizeDelta = 
                direction == Direction.Vertical?
                new Vector2(scorllContent.sizeDelta.x, length):new Vector2(length,scorllContent.sizeDelta.y);
        }
    }
}