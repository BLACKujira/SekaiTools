using SekaiTools.Count;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountMT : NCSScene
    {
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Image[] imgCharIcons;
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
        public IconSet charIconSet;
        [Header("Prefab")]
        public NCSScene_CountMT_Item prefabBig;
        public NCSScene_CountMT_Item prefabMid;
        public NCSScene_CountMT_Item prefabSmall;

        List<NCSScene_CountMT_Item> items = new List<NCSScene_CountMT_Item>();

        int characterId = 1;
        public override string Information => $@"持续时间 {holdTime}, 角色 {ConstData.characters[characterId].Name}";

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
        {
            new ConfigUIItem_Float("持续时间","场景",()=>holdTime,(value)=>holdTime = value),
            new ConfigUIItem_Character("角色","场景",()=>characterId ,(value)=>characterId  = value)
        };

        //物理模拟，废弃
        //IEnumerator CoPhysicalMovement()
        //{
        //    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        //    while (true)
        //    {
        //        for (int i = 1; i < items.Count; i++)
        //        {
        //            NCSScene_CountMT_Item item = items[i];
        //            Vector2 moveVector = Vector2.zero;
        //            foreach (var otherItem in items)
        //            {
        //                if(otherItem != item)
        //                {
        //                    moveVector += item.GetForce(otherItem);
        //                }
        //            }

        //            Vector2 anchoredPosition = item.targetRectTransform.anchoredPosition;
        //            anchoredPosition += moveVector * moveSpeed;
        //            item.targetRectTransform.anchoredPosition = anchoredPosition;
        //        }
        //        yield return waitForFixedUpdate;
        //    }
        //}

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

            List<NicknameCountItem> nicknameCountItems = new List<NicknameCountItem>();
            for (int i = 1; i <= 26; i++)
            {
                if (i == characterId) continue;
                NicknameCountItem nicknameCountItem = countData[i, characterId];
                nicknameCountItems.Add(nicknameCountItem);
            }
            nicknameCountItems.Sort((x, y) => -x.Total.CompareTo(y.Total));
            float max = nicknameCountItems[0].Total;

            float lastDetectRadius = 0;
            int lastDetectAngleCount = 1;
            for (int i = 0; i < nicknameCountItems.Count; i++)
            {
                NCSScene_CountMT_Item item;
                NicknameCountItem nicknameCountItem = nicknameCountItems[i];

                if (i == 0) item = Instantiate(prefabBig, targetRectTransform);
                else
                {
                    float percent = (float)nicknameCountItem.Total / max;
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

                item.SetData(nicknameCountItem.talkerId, nicknameCountItem.Total, characterId);
                items.Add(item);
            }

            imgCharIcons[0].sprite = charIconSet.icons[characterId];
            for (int i = 0; i < imgCharIcons.Length - 1; i++)
            {
                imgCharIcons[i + 1].sprite = charIconSet.icons[nicknameCountItems[i].talkerId];
            }

            if(gameObject.activeSelf)
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

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            holdTime = settings.holdTime;
            characterId = settings.characterId;
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int characterId;

            public Settings(NCSScene_CountMT nCSScene_CountMT)
            {
                holdTime = nCSScene_CountMT.holdTime;
                characterId = nCSScene_CountMT.characterId;
            }
        }
    }
}