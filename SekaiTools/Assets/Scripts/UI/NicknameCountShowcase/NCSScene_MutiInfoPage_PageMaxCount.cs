using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_MutiInfoPage_PageMaxCount : NCSScene_MutiInfoPage_PageBase
    {
        [Header("Components")]
        public Image imgCharL;
        public Image imgCharR;
        [Header("Settings")]
        public IconSet imgIconSet;

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
            int maxCount = 0;
            int charRId = 1;
            for (int i = 1; i < 27; i++)
            {
                int total = nicknameCountData[charId, i].Total;
                if (total > maxCount) { maxCount = total; charRId = i; }
            }

            int serifCount = nicknameCountData.GetSerifCount(charRId);
            float percent = ((float)maxCount / serifCount);
            titleText.text = $"提及次数最多：{maxCount}次";
            infoText.text =
                $@"在 {ConstData.characters[charId].namae} 的 {serifCount} 句台词中
一共提到 {ConstData.characters[charRId].namae} {maxCount} 次，占比 {percent * 100:00.00}%
也就是 {ConstData.characters[charId].namae} 平均 {1 / percent:0.0} 句就会提到一次 {ConstData.characters[charRId].namae}";

            SetCharRGraphics(charRId);
        }
    }
}