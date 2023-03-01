using SekaiTools.Count;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_MutiInfoPage_PageMaxIncrease : NCSScene_MutiInfoPage_PageBase
    {
        [Header("Components")]
        public Image imgCharL;
        public Image imgCharR;
        [Header("Settings")]
        public IconSet imgIconSet;

        public override ConfigUIItem[] ConfigUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_DateTime("上次统计时间","increase",()=>lastTimeCount,value=>lastTimeCount = value)
            };

        DateTime lastTimeCount = new DateTime(2022, 05, 01);

        protected override void SetCharLGraphics(int charId)
        {
            base.SetCharLGraphics(charId);
            imgCharL.sprite = imgIconSet.icons[charId];
        }

        protected override void SetCharRGraphics(int charId)
        {
            base.SetCharRGraphics(charId);
            imgCharR.sprite = imgIconSet.icons[charId];
        }

        public override void Initialize(NicknameCountData nicknameCountData, int charId, NCSPlayerBase player)
        {
            base.Initialize(nicknameCountData, charId, player);
            int totalNow = 0;
            int totalPrev = 0;
            int maxIncrease = 0;
            int charRId = 1;
            NicknameCountMatrix[] countMatricesLastTime = nicknameCountData.GetMatricesBefore(lastTimeCount);
            for (int i = 1; i < 27; i++)
            {
                int total = nicknameCountData[charId, i].Total;
                int totalLastTime = countMatricesLastTime.Sum(mat => mat[charId, i].Times);
                int increase = total - totalLastTime;
                if (increase > maxIncrease)
                {
                    maxIncrease = increase;
                    totalNow = total;
                    totalPrev = totalLastTime;
                    charRId = i;
                }
            }
            titleText.text = $"增长次数最多：{maxIncrease}次";
            infoText.text =
                $@"从上次 {lastTimeCount:Y} 统计到现在 
{ConstData.characters[charId].namae} 提到 {ConstData.characters[charRId].namae} 的次数从 {totalPrev} 增长到 {totalNow}
共增长了 {totalNow - totalPrev} 次";

            SetCharRGraphics(charRId);
        }
    }
}