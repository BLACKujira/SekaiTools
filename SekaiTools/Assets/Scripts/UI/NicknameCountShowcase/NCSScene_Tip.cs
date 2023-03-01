using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Tip : NCSScene, IImageFileReference
    {
        [Header("Components")]
        public Image imgEvLogo;
        public Text txtTip;

        public HashSet<string> RequireImageKeys => new HashSet<string>() { requireEvIconKey };

        public override string Information => base.Information + $" , 图像 {requireEvIconKey}";

        public string requireEvIconKey = string.Empty;
        public override void Refresh()
        {
            int maxEventId = 1;
            maxEventId = player.countData.countMatrix_Event
                .Select(mat => ConstData.IsEventStory(mat.fileName))
                .Where(evInfo => evInfo != null)
                .Select(evInfo => evInfo.eventId)
                .Max();

            MasterEvent ev = null;
            foreach (var masterEvent in player.events)
            {
                if (masterEvent.id == maxEventId)
                {
                    ev = masterEvent;
                    break;
                }
            }

            if (ev != null)
            {
                string imageKey = $"{ev.assetbundleName}_logo";
                requireEvIconKey = imageKey;

                if (player.imageData.ContainsValue(imageKey))
                {
                    imgEvLogo.sprite = player.imageData.GetValue(imageKey);
                }
                else
                {
                    player.imageData.AppendAbstractValue(
                        imageKey,
                        $"{EnvPath.Assets}/event/{ev.assetbundleName}/logo_rip/logo.png");
                }
                txtTip.text = @$"统计范围：从开服到第{ev.id}次活动「{ev.name}」的活动剧情、
地图对话、卡面剧情、Live对话；以及主线剧情、自我介绍、
一周年特别剧情及2022愚人节特别剧情。";

            }
            else
            {
                txtTip.text = "活动信息获取失败\n请尝试将数据表更新到最新版本";
            }
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            holdTime = settings.holdTime;
            requireEvIconKey = settings.evImageKey;
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public string evImageKey;

            public Settings(NCSScene_Tip nCSScene_Tip)
            {
                holdTime = nCSScene_Tip.holdTime;
                evImageKey = nCSScene_Tip.requireEvIconKey;
            }
        }
    }
}