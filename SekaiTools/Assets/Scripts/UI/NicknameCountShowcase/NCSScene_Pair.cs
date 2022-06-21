using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Pair : NCSScene
    {
        public NCSScene_Pair_Item[] items = new NCSScene_Pair_Item[20];

        public override ConfigUIItem[] configUIItems => throw new System.NotImplementedException();

        public override void Refresh()
        {
            List<SortItem> sortItems = new List<SortItem>();
            for (int i = 1; i < 27; i++)
            {
                for (int j = i+1; j < 27; j++)
                {
                    Count.NicknameCountItem a = countData[i, j];
                    Count.NicknameCountItem b = countData[j, i];
                    sortItems.Add(new SortItem(i, j, a.Total, b.Total));
                }
            }

            sortItems.Sort((x, y) => -x.total.CompareTo(y.total));

            for (int i = 0; i < items.Length; i++)
            {
                SortItem sortItem = sortItems[i];
                items[i].SetData(sortItem.idA, sortItem.idB, sortItem.countA, sortItem.countB);
            }
        }

        class SortItem
        {
            public int idA;
            public int idB;
            public int countA;
            public int countB;

            public SortItem(int idA, int idB, int countA, int countB)
            {
                this.idA = idA;
                this.idB = idB;
                this.countA = countA;
                this.countB = countB;
            }

            public int total => countA + countB;


        }
    }
}