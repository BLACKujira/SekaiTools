using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Rank_RankPage_Line : MonoBehaviour
    {
        [Header("Components")]
        public Image[] bgTextureImages = new Image[2];
        public Image charIconImage;
        public Text textTotal;
        public Text textPercent;
        public Text textUnit;
        public Text textEvent;
        public Text textCard;
        public Text textMap;
        public Text textLive;
        public Text textOther;
        [Header("Settings")]
        public IconSet iconSet;

        public void Initialize(NicknameCountItem nicknameCountItem,int nameId,int total)
        {
            foreach (var image in bgTextureImages)
            {
                image.color = ConstData.characters[nameId].imageColor;
            }
            charIconImage.sprite = iconSet.icons[nameId];
            textTotal.text = nicknameCountItem.Total.ToString("000");
            textPercent.text = (((float)nicknameCountItem.Total / total)*100).ToString("00.00") + "%";

            textUnit.text = nicknameCountItem.GetCount(StoryType.UnitStory).ToString("000");
            textEvent.text = nicknameCountItem.GetCount(StoryType.EventStory).ToString("000");
            textCard.text = nicknameCountItem.GetCount(StoryType.CardStory).ToString("000");
            textMap.text = nicknameCountItem.GetCount(StoryType.MapTalk).ToString("000");
            textLive.text = nicknameCountItem.GetCount(StoryType.LiveTalk).ToString("000");
            textOther.text = nicknameCountItem.GetCount(StoryType.OtherStory).ToString("000");
        }
    }
}