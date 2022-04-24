using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public abstract class ButtonGeneratorBase : MonoBehaviour
    {
        public abstract void Generate(int count, Action<Button, int> initialize, Action<int> onClick);

        public abstract void ClearButtons();

        public abstract void AddButton(Button buttonPrefab, Action<Button> initialize, Action onClick);

        public abstract void AddButton(Action<Button> initialize, Action onClick);
    }

    public class ButtonGenerator : ButtonGeneratorBase
    {
        public List<Button> buttons = new List<Button>();
        public enum Direction { Vertical, Horizontal }

        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public Button buttonPrefab;
        [Header("Settings")]
        public float blank;
        public float distance;
        public Direction direction = Direction.Vertical;

        public override void Generate(int count, Action<Button, int> initialize, Action<int> onClick)
        {
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);
            for (int i = 0; i < count; i++)
            {
                int id = i;
                Button button = Instantiate(buttonPrefab, scorllContent);
                button.GetComponent<RectTransform>().anchoredPosition = direction == Direction.Vertical ?
                    new Vector2(0, -distance * i - blank) :
                    new Vector2(distance * i + blank, 0);
                if (initialize != null) initialize(button, id);
                button.onClick.AddListener(() =>
                {
                    onClick(id);
                });
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

        public override void AddButton(Button buttonPrefab,Action<Button> initialize, Action onClick)
        {
            float count = buttons.Count + 1;
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);

            count--;
            Button button = Instantiate(buttonPrefab, scorllContent);
            button.GetComponent<RectTransform>().anchoredPosition = direction == Direction.Vertical ?
                new Vector2(0, -distance * count - blank) :
                new Vector2(distance * count + blank, 0);
            if(initialize!=null) initialize(button);
            button.onClick.AddListener(() =>
            {
                onClick();
            });
            buttons.Add(button);
        }
        public override void AddButton(Action<Button> initialize, Action onClick)
        {
            AddButton(buttonPrefab, initialize, onClick);
        }
    }
}