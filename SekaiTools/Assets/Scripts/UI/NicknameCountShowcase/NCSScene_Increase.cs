using SekaiTools.Count;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Increase : NCSScene
    {
        [Header("Components")]
        public NCSScene_Increase_Item[] items = new NCSScene_Increase_Item[20];

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_DateTime("上次统计时间","increase",()=>lastTimeCount,value=>lastTimeCount = value)
            };

        public override string Information =>  $"上次统计时间 {lastTimeCount:D} , " + base.Information;

        DateTime lastTimeCount = new DateTime(2022, 05, 01);

        public override void Refresh()
        {
            NicknameCountMatrix[] countMatricesNow = countData.NicknameCountMatrices;
            NicknameCountMatrix[] countMatricesLastTime = countData.GetMatricesBefore(lastTimeCount);

            List<SortItem> sortItems = new List<SortItem>();
            for (int i = 1; i < 27; i++)
            {
                for (int j = 1; j < 27; j++)
                {
                    sortItems.Add(new SortItem(i, j,
                        countMatricesNow.CountNicknameCountMatrices(i, j),
                        countMatricesLastTime.CountNicknameCountMatrices(i, j)));
                }
            }
            sortItems.Sort((x,y)=>x.Increase.CompareTo(y.Increase));
            sortItems.Reverse();
            for (int i = 0; i < items.Length; i++)
            {
                SortItem sortItem = sortItems[i];
                NCSScene_Increase_Item nCSScene_Increase_Item = items[i];
                nCSScene_Increase_Item.SetData(sortItem.talkerId, sortItem.nameId, sortItem.countNow, sortItem.countLastTime);
            }
        }
        class SortItem
        {
            public int talkerId;
            public int nameId;
            public int countNow;
            public int countLastTime;

            public SortItem(int talkerId, int nameId, int countNow, int countLastTime)
            {
                this.talkerId = talkerId;
                this.nameId = nameId;
                this.countNow = countNow;
                this.countLastTime = countLastTime;
            }

            public float Increase => countNow - countLastTime;
        }

    }
}