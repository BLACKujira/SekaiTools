using System;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress : MonoBehaviour
    {
        public DynamicBarChart dynamicBarChart;
        [Header("Components")]
        public RectTransform maskRectTransform;
        public RectTransform lineRectTransform;
        public HDRUIController portalLController;
        public HDRUIController portalRController;
        public HDRUIController lineEndController;
        [Header("Settings")]
        public float distancePerDataFrame = 20;
        [Header("Prefab")]
        public DynamicBarChart_Progress_EventItem eventItemPrefab;
        public DynamicBarChart_Progress_MonthItem monthItemPrefab;

        List<GameObject> itemGObjs = new List<GameObject>();
        List<KeyValuePair<string, float>> eventItemPositions = new List<KeyValuePair<string, float>>();
        Dictionary<float, Action> progressEvents = new Dictionary<float, Action>();
        DynamicBarChart_Progress_Portal portalL;
        DynamicBarChart_Progress_Portal portalR;
        DynamicBarChart_Progress_Line_End lineEnd;

        public void SetPortalColor(Color portalColor)
        {
            if (portalL) portalL.HDRColor = portalColor;
            if (portalR) portalR.HDRColor = portalColor;
            if (lineEnd) lineEnd.hDRColorParticle.hDRColor = portalColor;

        }

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
                    eventItemPositions.Add(new KeyValuePair<string, float>(frameEvent, cursor));
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

            portalL = portalLController.InstantiateObject.GetComponent<DynamicBarChart_Progress_Portal>();
            portalR = portalRController.InstantiateObject.GetComponent<DynamicBarChart_Progress_Portal>();

            float endProgressR = distancePerDataFrame * dataFrames.Length - maskRectTransform.sizeDelta.x;
            float endProgressL = distancePerDataFrame * dataFrames.Length;

            progressEvents[endProgressR] = () => { portalR.BreakPortal(); lineEnd.TurnOn(); };
            progressEvents[endProgressL] = () => { portalL.BreakPortal(); lineEnd.TurnOff(); };

            lineEnd = lineEndController.InstantiateObject.GetComponent<DynamicBarChart_Progress_Line_End>();
            lineEnd.TurnOff();
        }

        float lastProgress = 0;
        public void SetProgress(float itemIndex)
        {
            float progress = distancePerDataFrame * itemIndex;
            lineRectTransform.anchoredPosition = new Vector2(-progress, lineRectTransform.anchoredPosition.y);

            foreach (var keyValuePair in progressEvents)
            {
                if (keyValuePair.Key > lastProgress && keyValuePair.Key <= progress)
                {
                    keyValuePair.Value();
                }
            }

            lastProgress = progress;
        }

        public void Clear()
        {
            foreach (var gameObject in itemGObjs)
            {
                Destroy(gameObject);
            }
            itemGObjs = new List<GameObject>();
            eventItemPositions = new List<KeyValuePair<string, float>>();
            progressEvents = new Dictionary<float, Action>();
            if (portalL) portalL.ResetPortal();
            if (portalR) portalR.ResetPortal();
            if (lineEnd) lineEnd.TurnOff();
        }

        #region unused
        string GetEvItemLeft()
        {
            float positionX = lineRectTransform.anchoredPosition.x;
            return GetEvItem(positionX);
        }

        string GetEvItemRight()
        {
            float positionX = lineRectTransform.anchoredPosition.x;
            positionX += maskRectTransform.sizeDelta.x;
            return GetEvItem(positionX);
        }

        private string GetEvItem(float positionX)
        {
            for (int i = eventItemPositions.Count - 1; i >= 0; i--)
            {
                if (eventItemPositions[i].Value < positionX)
                    return eventItemPositions[i].Key;
            }
            return eventItemPositions[0].Key;
        }
        #endregion
    }
}