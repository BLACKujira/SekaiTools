using SekaiTools.Count;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_LoveMT : NCSScene
    {
        [Header("Components")]
        public RectTransform targetRectTransform;
        [Header("Settings")]
        public Vector2 origin;
        public float itemMinDistance = 20;
        public float detectRadiusDelta = 20;
        public int detectAngleDelta = 1;
        public float percentSizeChange = 80;
        public float yBoundary = 500;
        public float fadeInDelay = 1f;
        public float fadeInTime = 0.5f;
        public float fadeInInterval = 0.1f;
        [Header("Prefab")]
        public NCSScene_CountMT_Item prefabBig;
        public NCSScene_CountMT_Item prefabMid;
        public NCSScene_CountMT_Item prefabSmall;

        List<NCSScene_CountMT_Item> items = new List<NCSScene_CountMT_Item>();

        void Clear()
        {
            foreach (var item in items)
            {
                if (item != null) Destroy(item.gameObject);
            }
            items = new List<NCSScene_CountMT_Item>();
        }

        public override void Refresh()
        {
            Stop();
            Clear();

            KeyValuePair<int, int>[] countRank = new KeyValuePair<int, int>[26];
            for (int i = 1; i <= 26; i++)
            {
                int count = 0;
                for (int j = 1; j <= 26; j++)
                {
                    if (j == i) continue;
                    count += countData[j, i].Total;
                }
                countRank[i-1] = new KeyValuePair<int, int>(i, count); 
            }
            countRank = countRank.OrderBy(x => -x.Value).ToArray();

            float max = countRank[0].Value;

            float lastDetectRadius = 0;
            int lastDetectAngleCount = 1;
            for (int i = 0; i < countRank.Length; i++)
            {
                NCSScene_CountMT_Item item;
                KeyValuePair<int, int> keyValuePair = countRank[i];

                if (i == 0) item = Instantiate(prefabBig, targetRectTransform);
                else
                {
                    float percent = (float)keyValuePair.Value / max;
                    percent *= 100;
                    if (percent >= percentSizeChange)
                    {
                        item = Instantiate(prefabMid, targetRectTransform);
                    }
                    else
                    {
                        item = Instantiate(prefabSmall, targetRectTransform);
                    }
                }

                while (true)
                {
                    float randomAngle = Random.Range(0, 360);

                    bool breakFlag = false;
                    int[] randomArray = MathTools.GetRandomArray(lastDetectAngleCount);
                    foreach (var rdmInt in randomArray)
                    {
                        float anglePercent = (float)rdmInt / randomArray.Length;

                        float spawnAngle = 360 * anglePercent + randomAngle;
                        Vector2 spawnPosition = MathTools.AngleToRadiusOne(spawnAngle);
                        spawnPosition *= lastDetectRadius;

                        bool canSpawn = true;
                        if (Mathf.Abs(spawnPosition.y) + item.Radius > yBoundary)
                        {
                            canSpawn = false;
                        }
                        else
                        {
                            foreach (var countMT_Item in items)
                            {
                                if (countMT_Item.Radius + itemMinDistance + item.Radius > Vector2.Distance(spawnPosition, countMT_Item.Position))
                                {
                                    canSpawn = false;
                                    break;
                                }
                            }
                        }

                        if (canSpawn)
                        {
                            item.Position = spawnPosition;
                            lastDetectRadius -= item.Radius;
                            breakFlag = true;
                            break;
                        }
                    }

                    if (breakFlag) break;

                    lastDetectAngleCount += detectAngleDelta;
                    lastDetectRadius += detectRadiusDelta;
                }

                item.SetData(keyValuePair.Key, keyValuePair.Value, keyValuePair.Key);
                items.Add(item);
            }

            if (gameObject.activeSelf)
            {
                Coroutine coroutine = StartCoroutine(CoPlay());
                playCoroutines.Add(coroutine);
            }
        }

        List<Coroutine> playCoroutines = new List<Coroutine>();
        IEnumerator CoPlay()
        {
            foreach (var item in items)
            {
                item.targetRectTransform.localScale = new Vector3(0, 0, 1);
            }
            yield return new WaitForSeconds(fadeInDelay);

            int[] rdmArray = MathTools.GetRandomArray(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                if (gameObject.activeSelf)
                {
                    Coroutine coroutine = StartCoroutine(CoFadeInItem(items[rdmArray[i]]));
                    playCoroutines.Add(coroutine);
                }
                yield return new WaitForSeconds(fadeInInterval);
            }
        }

        IEnumerator CoFadeInItem(NCSScene_CountMT_Item item)
        {
            for (float i = 0; i < fadeInTime; i += Time.deltaTime)
            {
                float scale = i / fadeInTime;
                scale *= Mathf.PI / 2;
                scale = Mathf.Sin(scale);
                item.targetRectTransform.localScale = new Vector3(scale, scale, 1);
                yield return 1;
            }
        }

        void Stop()
        {
            foreach (var coroutine in playCoroutines)
            {
                if (coroutine != null) StopCoroutine(coroutine);
            }
            playCoroutines = new List<Coroutine>();
        }
    }
}