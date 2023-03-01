using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_MutiInfoPage_PageMaxCardStory : NCSScene_MutiInfoPage_PageBase, IImageFileReference
    {
        [Header("Components")]
        public Image imgCard;
        public Image imgCardFrame;
        [Header("Settings")]
        public IconSet cardFrameSet;

        public HashSet<string> RequireImageKeys => new HashSet<string>() { requireCardImageKey };

        string requireCardImageKey = string.Empty;
        public override void Initialize(NicknameCountData nicknameCountData, int charId, NCSPlayerBase player)
        {
            base.Initialize(nicknameCountData, charId, player);

            CardStoryInfo maxCardStoryInfo = null;
            int maxCount = 0;
            int maxCharId = 18;
            foreach (var nicknameCountMatrix in nicknameCountData.countMatrix_Card)
            {
                CardStoryInfo cardStoryInfo = ConstData.IsCardStory(nicknameCountMatrix.fileName);
                if (cardStoryInfo != null)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        int times = nicknameCountMatrix[charId, i].Times;
                        if (times > maxCount)
                        {
                            maxCount = times;
                            maxCharId = i;
                            maxCardStoryInfo = cardStoryInfo;
                        }
                    }
                }
            }

            MasterCard card = null;
            if (maxCardStoryInfo != null)
            {
                foreach (var masterCard in player.cards)
                {
                    if (masterCard.assetbundleName.Equals(maxCardStoryInfo.AssetbundleName))
                    {
                        card = masterCard;
                        break;
                    }
                }
            }

            SetCharRGraphics(maxCharId);
            titleText.text = $"单次卡面剧情提及最多：{maxCount}次";

            if (card != null)
            {
                infoText.text = $@"在 {ConstData.characters[card.characterId].namae} 的卡片
{card.prefix} 中，
{ConstData.characters[charId].namae} 一共提到了 {ConstData.characters[maxCharId].namae} {maxCount} 次。";

                string imageKey = $"{card.assetbundleName}_normal";
                requireCardImageKey = imageKey;
                if (player.imageData.ContainsValue(imageKey))
                {
                    imgCard.sprite = player.imageData.GetValue(imageKey);
                }
                else
                {
                    player.imageData.AppendAbstractValue(
                        imageKey,
                        $"{EnvPath.Assets}/thumbnail/chara_rip/{card.assetbundleName}_normal.png");
                }
                imgCardFrame.sprite = cardFrameSet.icons[(int)card.RarityType];
            }
            else
            {
                infoText.text = "卡片信息获取失败\n请尝试将数据表更新到最新版本";
            }
        }
    }
}