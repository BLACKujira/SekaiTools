using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ButtonGenerator2D : ButtonGeneratorBase
    {
        List<Button> buttons = new List<Button>();
        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public Button buttonPrefab;
        [Header("Settings")]
        public int numberPerLine;
        public float distanceX;
        public float distanceY;
    
        public override void Generate(int count, Action<Button, int> initialize, Action<int> onClick)
        {
            scorllContent.sizeDelta = new Vector2(
                scorllContent.sizeDelta.x,
                distanceY * ((count / numberPerLine) + ((count % numberPerLine) == 0 ? 0 : 1)));

            for (int i = 0; i < count; i++)
            {
                int id = i;

                Button button = Instantiate(buttonPrefab, scorllContent);

                initialize(button, id);

                button.onClick.AddListener(() => onClick(id));

                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    distanceX * (id % numberPerLine),
                    -distanceY * (id / numberPerLine));

                buttons.Add(button);
            }
        }

        public override void ClearButtons()
        {
            foreach (var button in buttons)
            {
                Destroy(button.gameObject);
            }
            buttons = new List<Button>();
        }

        public override void AddButton(Button buttonPrefab, Action<Button> initialize, Action onClick)
        {
            throw new NotImplementedException();
        }

        public override void AddButton(Action<Button> initialize, Action onClick)
        {
            throw new NotImplementedException();
        }
    }
}