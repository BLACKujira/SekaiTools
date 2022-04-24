using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    /// <summary>
    /// 生成一列单选按钮
    /// </summary>
    public class ToggleGenerator : MonoBehaviour
    {
        public List<Toggle> toggles;
        public enum Direction { Vertical, Horizontal }

        [Header("Components")]
        public RectTransform scorllContent;
        public ToggleGroup toggleGroup;
        [Header("Prefab")]
        public Toggle togglePrefab;
        [Header("Settings")]
        public float blank;
        public float distance;
        public Direction direction = Direction.Vertical;

        public void Generate(int count, Action<Toggle, int> initialize, Action<bool,int> onValueChanged)
        {
            scorllContent.sizeDelta = direction == Direction.Vertical?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2):
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);
            for (int i = 0; i < count; i++)
            {
                int id = i;
                Toggle toggle = Instantiate(togglePrefab, scorllContent);
                toggle.GetComponent<RectTransform>().anchoredPosition = direction == Direction.Vertical ?
                    new Vector2(0, -distance * i - blank) :
                    new Vector2(distance * i + blank, 0);
                initialize(toggle,id);
                toggle.onValueChanged.AddListener((bool value) => {
                    onValueChanged(value, id);
                });
                toggle.group = toggleGroup;
                toggles.Add(toggle);
            }
        }

        public void ClearToggles()
        {
            foreach (var toggle in toggles)
            {
                if(toggle)
                    Destroy(toggle.gameObject);
            }
            toggles = new List<Toggle>();
        }
    }
}