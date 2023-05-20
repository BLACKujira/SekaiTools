using SekaiTools.UI.DynamicBarChart;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_BarChartCharacter : NCSScene
    {
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Text txtTitle;
        [Header("Settings")]
        public float startDelay = 2;
        public float itemDistance;
        public float maxItemNumber = 5;
        public Vector2 particleInstantiatePos;
        [Header("Prefab")]
        public NCSScene_BarChartCharacter_Item itemPrefab;

        List<NCSScene_BarChartCharacter_Item> items = new List<NCSScene_BarChartCharacter_Item>();

        int characterId = 1;
        int rank = 0;
        public override string Information => $@"持续时间 {holdTime}, 角色 {ConstData.characters[characterId].Name},
排名 {rank + 1}";

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","场景",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Character("角色","场景",()=>characterId,(value)=>characterId = value),
                new ConfigUIItem_Int("排名","场景",()=>rank+1,(value)=>rank = value<1 ? 1 : value-1)
            };

        public override void Refresh()
        {
            txtTitle.text = $"第 {rank + 1} 名持续时间";

            foreach (var barChartCharacter_Item in items)
            {
                if (barChartCharacter_Item != null)
                    Destroy(barChartCharacter_Item.gameObject);
            }
            items = new List<NCSScene_BarChartCharacter_Item>();

            List<DataFrameCharacter> dataFrames = DynamicBarChartCharacter.GetDataFrameCharacter(countData, characterId, player);
            Dictionary<string, int> count = new Dictionary<string, int>();
            List<KeyValuePair<string, float>> rankFrame = new List<KeyValuePair<string, float>>();

            foreach (DataFrameCharacter dataFrame in dataFrames)
            {
                rankFrame = new List<KeyValuePair<string, float>>(dataFrame.data.OrderBy(df => -df.Value));
                if (rankFrame.Count <= rank) continue;

                KeyValuePair<string, float> selected = rankFrame[rank];
                if (selected.Value > 0)
                {
                    count[selected.Key] = count.ContainsKey(selected.Key) ? count[selected.Key] + 1 : 1;
                }
            }

            KeyValuePair<string, int>[] countArray = count.OrderBy(kvp => -kvp.Value).ToArray();

            if (playCoroutine != null) StopCoroutine(playCoroutine);
            if (gameObject.activeSelf) playCoroutine = StartCoroutine(CoPlay(countArray));
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
            rank = settings.rank;
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int characterId;
            public int rank;

            public Settings(NCSScene_BarChartCharacter nCSScene_BarChartCharacter)
            {
                holdTime = nCSScene_BarChartCharacter.holdTime;
                characterId = nCSScene_BarChartCharacter.characterId;
                rank = nCSScene_BarChartCharacter.rank;
            }
        }

        Coroutine playCoroutine;
        IEnumerator CoPlay(KeyValuePair<string, int>[] countArray)
        {
            float startPos = Mathf.Min(countArray.Length - 1, maxItemNumber - 1) * this.itemDistance;
            startPos /= 2;
            startPos = -startPos;
            float total = countArray.Select(kvp => kvp.Value).Sum();

            float itemDistance = this.itemDistance;
            if (countArray.Length > maxItemNumber) itemDistance = (4 * itemDistance) / (countArray.Length - 1);

            for (int i = 0; i < countArray.Length; i++)
            {
                string[] idArray = countArray[i].Key.Split('_');
                int charAId = int.Parse(idArray[0]);
                int charBId = int.Parse(idArray[1]);

                itemPrefab.particleController.instantiatePosition = particleInstantiatePos + new Vector2(startPos + i * itemDistance, 0);

                NCSScene_BarChartCharacter_Item item = Instantiate(itemPrefab, targetRectTransform);
                item.RectTransform.anchoredPosition = new Vector2(startPos + i * itemDistance, item.RectTransform.anchoredPosition.y);
                item.SetData(charAId, charBId, 0, countArray[0].Value, total);

                items.Add(item);
            }

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < items.Count; i++)
            {
                string[] idArray = countArray[i].Key.Split('_');
                int charAId = int.Parse(idArray[0]);
                int charBId = int.Parse(idArray[1]);

                NCSScene_BarChartCharacter_Item item = items[i];
                item.SetData(charAId, charBId, countArray[i].Value, countArray[0].Value, total);
            }
        }
    }
}