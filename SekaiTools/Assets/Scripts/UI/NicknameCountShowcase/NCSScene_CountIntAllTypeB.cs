using SekaiTools.Count;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountIntAllTypeB : NCSScene_CountIntAllBase
    {
        [Header("Components")]
        public NCSScene_CountIntAll_ItemBase[] countCharacters = new NCSScene_CountIntAll_ItemBase[25];
        public NCSScene_CountIntAllTypeB_ItemTotal countTotal;

        public override void Refresh()
        {
            StopAllCoroutines();
            l2DController.HideModelAll();

            List<NicknameCountItem> nicknameCountItems = new List<NicknameCountItem>();
            for (int i = 1; i < 27; i++)
            {
                if (i == talkerId) continue;
                NicknameCountItem nicknameCountItem = countData[talkerId, i];
                nicknameCountItems.Add(nicknameCountItem);
            }
            nicknameCountItems.Sort((x, y) => x.Total.CompareTo(y.Total));
            nicknameCountItems.Reverse();
            int total = countData.GetCountTotal(talkerId, true);
            for (int i = 0; i < 25; i++)
            {
                NicknameCountItem nicknameCountItem = nicknameCountItems[i];
                countCharacters[i].Initialize(nicknameCountItem.talkerId, nicknameCountItem.nameId, nicknameCountItem.Total, total);
            }

            string totalText = $"共计 {total} 次 ，在 {countData.GetSerifCount(talkerId)} 句台词中";
            countTotal.Initialize(talkerId, totalText, total, countData.GetSerifCount(talkerId));
            if (l2DController.live2DModels[talkerId])
            {
                l2DController.ShowModelLeft((Character)talkerId);
                l2DController.SetModelPositionLeft(modelPosition);
                l2DController.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
                l2DController.FadeInLeft();
                AudioClip audioClip;
                if (audioData != null) audioClip = this.audioData.ValueArray[0];
                else audioClip = player.audioData.GetValue(audioClipName);
                if (audioClip)
                    StartCoroutine(IPlayVoice(audioClip));
                l2DController.modelL.PlayAnimation(motionName, facialName);
            }
        }
    }
}