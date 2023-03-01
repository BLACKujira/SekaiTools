using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Love : NCSScene
    {
        [Header("Components")]
        public NCSScene_Love_Item[] items = new NCSScene_Love_Item[20];

        public override void Refresh()
        {
            List<SortItem> sortItems = new List<SortItem>();
            for (int i = 1; i < 27; i++)
            {
                int total = countData.GetCountTotal(i, true);
                Count.NicknameCountItem maxItem = countData[i, 1];
                for (int j = 2; j < 27; j++)
                {
                    Count.NicknameCountItem nicknameCountItem = countData[i, j];
                    if (nicknameCountItem.Total > maxItem.Total)
                        maxItem = nicknameCountItem;
                }
                sortItems.Add(new SortItem(maxItem.talkerId, maxItem.nameId, maxItem.Total,total));
            }

            sortItems.Sort((x, y) => -x.percent.CompareTo(y.percent));

            for (int i = 0; i < items.Length; i++)
            {
                SortItem sortItem = sortItems[i];
                items[i].SetData(sortItem.talkerId, sortItem.nameId, sortItem.total , sortItem.count);
            }
        }
        class SortItem
        {
            public int talkerId;
            public int nameId;
            public int count;
            public int total;

            public float percent => (float)count / total;

            public SortItem(int talkerId, int nameId, int count,int total)
            {
                this.talkerId = talkerId;
                this.nameId = nameId;
                this.count = count;
                this.total = total;
            }
        }
    }
}