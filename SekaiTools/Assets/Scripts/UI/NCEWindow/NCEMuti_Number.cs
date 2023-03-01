using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NCEWindow
{
    public class NCEMuti_Number : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform targetRt;
        [Header("Settings")]
        public float startX = 30;
        public float distance = 60;
        public int maxNumber = 10;
        [Header("Prefab")]
        public NCEMuti_Number_Item itemPrefab;

        List<NCEMuti_Number_Item> items = new List<NCEMuti_Number_Item>();

        public void SetData(Vector2Int[] idTimesPairs)
        {
            foreach (var item in items)
            {
                if (item != null) Destroy(item.gameObject);
            }

            items = new List<NCEMuti_Number_Item>();
            List<Vector2Int> idTimesPairList = new List<Vector2Int>(idTimesPairs);
            idTimesPairList.Sort((x, y) => x.x.CompareTo(y.x));
            for (int i = 0; i < idTimesPairList.Count; i++)
            {
                Vector2Int kvp = idTimesPairList[i];
                NCEMuti_Number_Item nCEMuti_Number_Item = Instantiate(itemPrefab, targetRt);
                nCEMuti_Number_Item.RectTransform.anchoredPosition
                    = new Vector2(startX - i * distance, nCEMuti_Number_Item.RectTransform.anchoredPosition.y);
                nCEMuti_Number_Item.SetData(kvp.x, kvp.y);
                items.Add(nCEMuti_Number_Item);
            }
        }
    }
}