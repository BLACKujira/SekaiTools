using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_MutiInfoPage_PageMaxEvent : NCSScene_MutiInfoPage_PageBase, IImageFileReference
    {
        [Header("Components")]
        public Image imgEvLogo;

        public HashSet<string> RequireImageKeys => new HashSet<string>() { requireEvIconKey };

        string requireEvIconKey = string.Empty;
        public override void Initialize(NicknameCountData nicknameCountData, int charId, NCSPlayerBase player)
        {
            base.Initialize(nicknameCountData, charId, player);

            NicknameCountMatrixByEvent nicknameCountMatrixByEvent = nicknameCountData.GetCountMatrixByEvent();
            int maxEventId = 1;
            int maxCharId = 18;
            int maxCount = 0;
            foreach (var keyValuePair in nicknameCountMatrixByEvent.events)
            {
                for (int i = 1; i < 27; i++)
                {
                    int count = keyValuePair.Value.GetTotal(charId, i);
                    if (count > maxCount)
                    {
                        maxCount = count;
                        maxEventId = keyValuePair.Key;
                        maxCharId = i;
                    }
                }
            }

            MasterEvent masterEvent = Player.events.Length < maxEventId ? null : Player.events[maxEventId - 1];

            SetCharRGraphics(maxCharId);
            titleText.text = $"单次活动提及次数最多：{maxCount}次";
            if (masterEvent != null)
            {
                infoText.text = $@"在第 {maxEventId} 期活动 
{masterEvent.name} 中，
{ConstData.characters[charId].namae} 一共提到了 {ConstData.characters[maxCharId].namae} {maxCount} 次。";

                string imageKey = $"{masterEvent.assetbundleName}_logo";
                requireEvIconKey = imageKey;
                if (player.imageData.ContainsValue(imageKey))
                {
                    imgEvLogo.sprite = player.imageData.GetValue(imageKey);
                }
                else
                {
                    player.imageData.AppendAbstractValue(
                        imageKey,
                        $"{EnvPath.Assets}/event/{masterEvent.assetbundleName}/logo_rip/logo.png");
                }
            }
            else
            {
                infoText.text = $"活动信息获取失败\n请尝试将数据表更新到最新版本";
            }
        }
    }
}