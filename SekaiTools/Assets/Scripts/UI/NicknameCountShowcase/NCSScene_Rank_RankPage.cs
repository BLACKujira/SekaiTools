using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Rank_RankPage : MonoBehaviour
    {
        [Header("Components")]
        public Image bgImage;
        public Image charIconImage;
        public NCSScene_Rank_RankPage_Line[] lines;
        [Header("Settings")]
        public IconSet iconSet;

        public void Initialize(NicknameCountData nicknameCountData,int talkerId)
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
            nicknameCountItems.Sort((x,y)=>-x.Total.CompareTo(y.Total));

            bgImage.color = ConstData.characters[talkerId].imageColor;
            charIconImage.sprite = iconSet.icons[talkerId];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Initialize(nicknameCountItems[i], nicknameCountItems[i].nameId, total);
            }
        }
    }
}