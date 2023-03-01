using SekaiTools.Count;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountIntAll : NCSScene_CountIntAllBase
    {
        [Header("Text")]
        public Text[] countTextCharacters = new Text[27];
        public Text countTextTotal;

        public override void Refresh()
        {
            StopAllCoroutines();
            l2DController.HideModelAll();
            for (int i = 1; i < 27; i++)
            {
                if (i == talkerId)
                {
                    countTextCharacters[i].text = "-";
                }
                else
                {
                    NicknameCountItem nicknameCountItem = countData[talkerId, i];
                    countTextCharacters[i].text = nicknameCountItem.Total.ToString();
                }
            }
            countTextTotal.text = $"共计 {countData.GetCountTotal(talkerId, true)} 次 ，在 {countData.GetSerifCount(talkerId)} 句台词中";
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