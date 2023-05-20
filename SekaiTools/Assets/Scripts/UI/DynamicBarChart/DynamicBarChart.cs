using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public abstract class DynamicBarChart : NCSScene, IImageFileReference
    {
        [Header("Components")]
        public RectTransform targetRectTransform;
        public DynamicBarChart_Progress progressBar;
        [Header("Settings")]
        public int maxItemNumber = 10;
        public float itemDistance = 80;
        public Vector2 particleStartPosition = new Vector2(0, -1024);
        public int particleDistance = 32;
        public float startDelay = 3;
        public float moveNextAfter = 2;
        [Header("Prefab")]
        public DynamicBarChart_Item itemPrefab;

        protected List<ItemManager> items = new List<ItemManager>();
        protected DataFrame[] dataFrames;
        protected Dictionary<int, DynamicBarChart_Item_Head> usedParticlePositions = new Dictionary<int, DynamicBarChart_Item_Head>();

        protected float frameHoldTime = 0.25f;
        int UsedItems => items.Select(im => !im.item.IsFadingOut).Count();

        public HashSet<string> RequireImageKeys => requireImageKeys;
        protected HashSet<string> requireImageKeys = new HashSet<string>();

        public class ItemManager
        {
            public string key;
            public DynamicBarChart_Item item;
            public Coroutine coroutine;
            public float targetPositionY;

            MonoBehaviour monoBehaviour;

            const float LERPT = 2f;
            const float MOVE_DELTA_ADD = 150F;
            const float DESTROY_AFTER = 1.2F;

            public ItemManager(string key, DynamicBarChart_Item item, MonoBehaviour monoBehaviour)
            {
                this.key = key;
                this.item = item;
                this.monoBehaviour = monoBehaviour;

                item.FadeIn();
                targetPositionY = item.RectTransform.anchoredPosition.y;
                coroutine = monoBehaviour.StartCoroutine(CoMove());
            }

            IEnumerator CoMove()
            {
                while (true)
                {
                    float posY = Mathf.Lerp(item.RectTransform.anchoredPosition.y, targetPositionY, LERPT * Mathf.Min(Time.deltaTime, 1));
                    posY = Mathf.MoveTowards(posY, targetPositionY, MOVE_DELTA_ADD * Time.deltaTime);
                    item.RectTransform.anchoredPosition = new Vector2(item.RectTransform.anchoredPosition.x, posY);
                    yield return 1;
                }
            }

            public void Recycle()
            {
                item.FadeOut();
                monoBehaviour.StartCoroutine(CoDestroy());
            }

            IEnumerator CoDestroy()
            {
                yield return new WaitForSeconds(DESTROY_AFTER);
                monoBehaviour.StopCoroutine(coroutine);
                Destroy(item.gameObject);
            }
        }

        protected float GetHoldTime()
        {
            return startDelay + moveNextAfter + frameHoldTime * dataFrames.Length;
        }

        public override void Initialize(NCSPlayerBase player)
        {
            base.Initialize(player);
            requireImageKeys = new HashSet<string>(player.events.Select(ev => $"{ev.assetbundleName}_logo"));
            foreach (var ev in player.events)
            {
                string imageKey = $"{ev.assetbundleName}_logo";
                if (!player.imageData.ContainsValue(imageKey))
                {
                    player.imageData.AppendAbstractValue(
                        imageKey,
                        $"{EnvPath.Assets}/event/{ev.assetbundleName}/logo_rip/logo.png");
                }
            }
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            holdTime = settings.holdTime;
            requireImageKeys = new HashSet<string>(settings.requireImageKeys);
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public float frameHoldTime;
            public string[] requireImageKeys;

            public Settings(DynamicBarChart dynamicBarChart)
            {
                holdTime = dynamicBarChart.holdTime;
                requireImageKeys = dynamicBarChart.requireImageKeys.ToArray();
                frameHoldTime = dynamicBarChart.frameHoldTime;
            }
        }

        protected void AbortAllItems()
        {
            StopAllCoroutines();
            foreach (var itemManager in items)
            {
                if (itemManager != null && itemManager.item != null)
                    Destroy(itemManager.item.gameObject);
            }
            items = new List<ItemManager>();
        }

        bool HasItem(string key)
        {
            foreach (var itemManager in items)
            {
                if (itemManager != null && itemManager.key.Equals(key) && !itemManager.item.IsFadingOut)
                    return true;
            }
            return false;
        }

        IEnumerator CoPlay()
        {
            canMoveNext = false;
            yield return new WaitForSeconds(startDelay);
            for (int i = 0; i < dataFrames.Length; i++)
            {
                DataFrame dataFrame = dataFrames[i];
                PlayFrame(dataFrame);
                for (float t = 0; t < frameHoldTime; t += Time.deltaTime)
                {
                    float percent = t / frameHoldTime;
                    progressBar.SetProgress(percent + i);
                    yield return 1;
                }
            }
            progressBar.SetProgress(dataFrames.Length);
            yield return new WaitForSeconds(moveNextAfter);
            canMoveNext = true;
        }

        bool isPlaying = false;
        Coroutine playCoroutine;
        //private void Update()
        //{
        //    if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        //    {
        //        playCoroutine = StartCoroutine(CoPlay());
        //        isPlaying = true;
        //    }
        //}

        protected void Play()
        {
            if (!isPlaying && gameObject.activeSelf)
            {
                playCoroutine = StartCoroutine(CoPlay());
                isPlaying = false;
            }
        }

        protected void Stop()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                isPlaying = false;
            }
        }

        void PlayFrame(DataFrame dataFrame)
        {
            if (dataFrame.data.Count <= 0) return;

            List<string> sortedStrings = new List<string>(
                dataFrame.data.Select(kvp => kvp.Key)
                .OrderBy(str => -dataFrame.data[str]));

            float maxNumber = dataFrame.data[sortedStrings[0]];

            //检查并添加缺少的
            for (int i = 0; i < sortedStrings.Count && i < maxItemNumber; i++)
            {
                if (!HasItem(sortedStrings[i]))
                {
                    int particlePositionY = GetUnusedParticlePosition();
                    itemPrefab.headController.instantiatePosition = new Vector2(particleStartPosition.x, particlePositionY);

                    DynamicBarChart_Item dynamicBarChart_Item = Instantiate(itemPrefab, targetRectTransform);
                    dynamicBarChart_Item.RectTransform.anchoredPosition =
                        new Vector2(dynamicBarChart_Item.RectTransform.anchoredPosition.x, -UsedItems * itemDistance);
                    DynamicBarChart_Item_Head dynamicBarChart_Item_Head
                        = dynamicBarChart_Item.headController.InstantiateObject.GetComponent<DynamicBarChart_Item_Head>();

                    usedParticlePositions[particlePositionY] = dynamicBarChart_Item_Head;

                    items.Add(new ItemManager(sortedStrings[i], dynamicBarChart_Item, this));
                }
            }

            //移去未使用的
            items.RemoveAll(im => im == null);
            foreach (var itemManager in items)
            {
                if (itemManager == null
                    || itemManager.item == null
                    || !sortedStrings.Contains(itemManager.key)
                    || sortedStrings.IndexOf(itemManager.key) >= maxItemNumber)
                {
                    if (!itemManager.item.IsFadingOut)
                    {
                        itemManager.targetPositionY += itemDistance;
                        itemManager.Recycle();
                    }
                }
            }

            //更新数据
            for (int i = 0; i < sortedStrings.Count; i++)
            {
                string key = sortedStrings[i];

                foreach (var itemManager in items)
                {
                    if (itemManager != null && itemManager.key.Equals(key))
                    {
                        itemManager.item.UpdateData(dataFrame, key, maxNumber,frameHoldTime);
                        if (!itemManager.item.IsFadingOut) itemManager.targetPositionY = -i * itemDistance;
                    }
                }
            }

        }

        int GetUnusedParticlePosition()
        {
            int[] clearedPositions = usedParticlePositions
                .Where(kvp => kvp.Value == null)
                .Select(kvp => kvp.Key)
                .ToArray();

            foreach (var posY in clearedPositions)
            {
                usedParticlePositions.Remove(posY);
            }

            for (int i = (int)particleStartPosition.y; ; i += particleDistance)
            {
                if (!usedParticlePositions.ContainsKey(i))
                {
                    return i;
                }
            }
        }
    }
}