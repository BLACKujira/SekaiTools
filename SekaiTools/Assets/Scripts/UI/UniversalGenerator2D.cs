using System;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class UniversalGenerator2D : MonoBehaviour
    {
        List<GameObject> items = new List<GameObject>();
        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public GameObject itemPrefab;
        [Header("Settings")]
        public int numberPerLine;
        public float distanceX;
        public float distanceY;

        public void Generate(int count, Action<GameObject, int> initialize)
        {
            scorllContent.sizeDelta = new Vector2(
                scorllContent.sizeDelta.x,
                distanceY * ((count / numberPerLine) + ((count % numberPerLine) == 0 ? 0 : 1)));

            for (int i = 0; i < count; i++)
            {
                int id = i;

                GameObject gobj = Instantiate(itemPrefab, scorllContent);

                initialize(gobj, id);

                gobj.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    distanceX * (id % numberPerLine),
                    -distanceY * (id / numberPerLine));

                items.Add(gobj);
            }
        }

        public void ClearItems()
        {
            foreach (var gobj in items)
            {
                Destroy(gobj.gameObject);
            }
            items = new List<GameObject>();
        }
    }
}