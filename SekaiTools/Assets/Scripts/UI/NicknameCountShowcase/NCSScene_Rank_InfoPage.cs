using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Rank_InfoPage : MonoBehaviour
    {
        public NCSScene_Rank scene_Rank;
        [Header("Components")]
        public Image[] bgImageTalker;
        public Image[] bgImageChar;
        public Image talkerIconImage;
        public NCSScene_Rank_RankPage_Line line;
        [Header("Components_BlockPercent")]
        public Image imageInfoPerecentTotal;
        public Image imageInfoPerecentMost;
        public Text textInfoPerecent;
        [Header("Components_BlockEvent")]
        public Text textInfoEvent;
        public Image imageEventIcon;
        [Header("Settings")]
        public IconSet iconSetCharacter;

        public string requireEvIconKey = string.Empty;

        public void Initialize(NicknameCountData nicknameCountData, int talkerId)
        {
            List<NicknameCountItem> nicknameCountItems = new List<NicknameCountItem>();
            int total = 0;
            for (int i = 1; i < 27; i++)
            {
                if (i == talkerId) continue;
                NicknameCountItem nicknameCountItem = nicknameCountData[talkerId, i];
                nicknameCountItems.Add(nicknameCountItem);
                total += nicknameCountItem.Total;
            }
            nicknameCountItems.Sort((x, y) => -x.Total.CompareTo(y.Total));

            talkerIconImage.sprite = iconSetCharacter.icons[talkerId];
            line.Initialize(nicknameCountItems[0], nicknameCountItems[0].nameId, total);

            int nameId = nicknameCountItems[0].nameId;

            foreach (var image in bgImageTalker)
                image.color = ConstData.characters[talkerId].imageColor;
        
            foreach (var image in bgImageChar)
                image.color = ConstData.characters[nameId].imageColor;

            int countAllSerif = nicknameCountData.GetSerifCount(talkerId);

            float percentTotal = (float)total / countAllSerif;
            float percentMost = (float)nicknameCountItems[0].Total / countAllSerif;

            imageInfoPerecentTotal.fillAmount = percentTotal;
            imageInfoPerecentMost.fillAmount = percentMost;

            textInfoPerecent.text = $@"在 {ConstData.characters[talkerId].Name} 的 {countAllSerif} 句台词中，有 {total} 句台词提到了其他25名角色，
其中有 {nicknameCountItems[0].Total} 句提到了 {ConstData.characters[nameId].Name} , 占总台词 {(percentMost*100).ToString("00.00")}%。
也就是说 {ConstData.characters[talkerId].namae} 平均每 {((float)countAllSerif/nicknameCountItems[0].Total).ToString("0.00")} 句台词就会提到一次 {ConstData.characters[nameId].namae}。";

            NicknameCountItemByEvent nicknameCountItemByEvent = nicknameCountData.GetCountItemByEvent(talkerId, nameId);
            KeyValuePair<int, int> eventMost = new KeyValuePair<int, int>(0,0);
            foreach (var keyValuePair in nicknameCountItemByEvent.countDictionary)
            {
                if (keyValuePair.Value > eventMost.Value) eventMost = keyValuePair;
            }

            MasterEvent ev = null;
            foreach (var masterEvent in scene_Rank.player.events)
            {
                if (masterEvent.id == eventMost.Key)
                {
                    ev = masterEvent;
                    break;
                }
            }

            if (ev != null)
            {
                string imageKey = $"{ev.assetbundleName}_logo";
                requireEvIconKey = imageKey;

                if (scene_Rank.player.imageData.ContainsValue(imageKey))
                {
                    imageEventIcon.sprite = scene_Rank.player.imageData.GetValue(imageKey);
                }
                else
                {
                    scene_Rank.player.imageData.AppendAbstractValue(
                        imageKey,
                        $"{EnvPath.Assets}/event/{ev.assetbundleName}/logo_rip/logo.png");
                }
                textInfoEvent.text = $@"在第 {eventMost.Key} 期活动 {ev.name} 中，
{ConstData.characters[talkerId].Name} 一共提到了 {ConstData.characters[nameId].Name} {eventMost.Value} 次。";

            }
            else
            {
                textInfoEvent.text = "活动信息获取失败\n请尝试将数据表更新到最新版本";
            }
        }
}
}