using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress : MonoBehaviour
    {
        public DynamicBarChart dynamicBarChart;
        [Header("Components")]
        public RectTransform targetRectTransform;
        public RectTransform lineRectTransform;
        [Header("Settings")]
        public float distancePerDataFrame = 20;
        [Header("Prefab")]
        public DynamicBarChart_Progress_EventItem eventItemPrefab;
        public DynamicBarChart_Progress_MonthItem monthItemPrefab;

        List<GameObject> itemGObjs = new List<GameObject>();

        public void Initialize(DataFrame[] dataFrames)
        {
            float cursor = 0;
            string lastFrameEvent = string.Empty;
            string lastFrameMonth = string.Empty;
            foreach (var dataFrame in dataFrames)
            {
                string frameEvent = dataFrame.EventGroup;
                string frameMonth = $"{dataFrame.DateTime.Year}.{dataFrame.DateTime.Month}";

                if (!string.IsNullOrEmpty(frameEvent) && !lastFrameEvent.Equals(frameEvent))
                {
                    DynamicBarChart_Progress_EventItem eventItem = Instantiate(eventItemPrefab, lineRectTransform);
                    eventItem.RectTransform.anchoredPosition = new Vector2(cursor, eventItem.RectTransform.anchoredPosition.y);
                    itemGObjs.Add(eventItem.gameObject);
                    Sprite sprite = dynamicBarChart.player.imageData.GetValue($"{frameEvent}_logo");
                    if (sprite != null) eventItem.imgEvIcon.sprite = sprite;

                    lastFrameEvent = frameEvent;
                }
                if (!string.IsNullOrEmpty(frameMonth) && !lastFrameMonth.Equals(frameMonth))
                {
                    DynamicBarChart_Progress_MonthItem monthItem = Instantiate(monthItemPrefab, lineRectTransform);
                    monthItem.RectTransform.anchoredPosition = new Vector2(cursor, monthItem.RectTransform.anchoredPosition.y);
                    monthItem.txtMonth.text = frameMonth;
                    itemGObjs.Add(monthItem.gameObject);

                    lastFrameMonth = frameMonth;
                }

                cursor += distancePerDataFrame;
            }

            lineRectTransform.sizeDelta = new Vector2(distancePerDataFrame * dataFrames.Length, lineRectTransform.sizeDelta.y);
        }

        public void SetProgress(float itemIndex)
        {
            targetRectTransform.anchoredPosition = new Vector2(-distancePerDataFrame * itemIndex, targetRectTransform.anchoredPosition.y);
        }

        public void ClearItems()
        {
            foreach (var gameObject in itemGObjs)
            {
                Destroy(gameObject);
            }
            itemGObjs = new List<GameObject>();
        }
    }
}