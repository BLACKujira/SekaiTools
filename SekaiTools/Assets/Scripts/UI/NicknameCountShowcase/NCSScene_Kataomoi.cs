using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Kataomoi : NCSScene
    {
        public NCSScene_Kataomoi_Item[] items = new NCSScene_Kataomoi_Item[20];

        public override ConfigUIItem[] configUIItems => throw new System.NotImplementedException();

        public override void Refresh()
        {
            float[,] percents = new float[27, 27];
            for (int i = 1; i < 27; i++)
            {
                int total = countData.GetCountTotal(i,true);
                for (int j = 1; j < 27; j++)
                {
                    percents[i,j] = (float)countData[i,j].Total/total;
                }
            }

            List<SortItem> sortItems = new List<SortItem>();
            for (int i = 1; i < 27; i++)
            {
                for (int j = i+1; j < 27; j++)
                {
                    sortItems.Add(new SortItem(i, j, percents[i, j], percents[j, i]));
                }
            }

            sortItems.Sort((x, y) => -x.difference.CompareTo(y.difference));

            for (int i = 0; i < items.Length; i++)
            {
                SortItem sortItem = sortItems[i];
                items[i].SetData(sortItem.idA, sortItem.idB, sortItem.percentA, sortItem.percentB);
            }
        }

        class SortItem
        {
            public int idA;
            public int idB;
            public float percentA;
            public float percentB;

            public SortItem(int idA, int idB, float percentA, float percentB)
            {
                if (percentA > percentB)
                {
                    this.idA = idA;
                    this.idB = idB;
                    this.percentA = percentA;
                    this.percentB = percentB;
                }
                else
                {
                    this.idA = idB;
                    this.idB = idA;
                    this.percentA = percentB;
                    this.percentB = percentA;
                }
            }

            public float difference => percentA - percentB;
        }
    }
}