using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using SekaiTools.SystemLive2D;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace SekaiTools.UI.SysL2DShowPlayer
{
    public class SysL2DShowPlayer_Player : MonoBehaviour
    {
        [Header("Components")]
        public L2DControllerTypeB l2DControllerTypeB;
        public SysL2DShowPlayer_Player_Balloon balloon;
        public Text txtTimeRange;
        [Header("Settings")]
        public float safeFrame = 2;
        public float fadeInTime = .15f;
        public float textFadeTime = .25f;

        public Vector2 modelPos;
        public float modelScale;

        SysL2DShow sysL2DShow;
        public SysL2DShow SysL2DShow => sysL2DShow;
        AudioData audioData;
        public AudioData AudioData => audioData;

        public class Settings
        {
            public L2DControllerTypeB.Settings l2dSettings = new L2DControllerTypeB.Settings();
            public AudioData audioData;
        }

        public void Initialize(Settings settings)
        {
            audioData = settings.audioData;
            l2DControllerTypeB.Initialize(settings.l2dSettings);
            l2DControllerTypeB.FadeOutAll(0);
            if(txtTimeRange) txtTimeRange.DOFade(0, 0);
        }

        public void SetData(SysL2DShow sysL2DShow)
        {
            this.sysL2DShow = sysL2DShow;
        }

        public void Play(bool showOriginText = false)
        {
            StopAllCoroutines();
            StartCoroutine(CoPlay(showOriginText));
        }

        IEnumerator CoPlay(bool showOriginText = false)
        {
            balloon.SetSerif(showOriginText ? sysL2DShow.systemLive2D.Serif : sysL2DShow.translationText);
            balloon.Play();
            if (txtTimeRange)
            {
                txtTimeRange.text = string.IsNullOrEmpty(sysL2DShow.dateTimeOverrideText) ?
                    string.Join("\n",sysL2DShow.systemLive2D.masterSystemLive2Ds.Select((msl2d) => $"{ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.publishedAt):D} µ½ {ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.closedAt):D}")) 
                    : sysL2DShow.dateTimeOverrideText;
                txtTimeRange.DOFade(1, textFadeTime);
            }

            if (l2DControllerTypeB.modelL != l2DControllerTypeB.live2DModels[sysL2DShow.systemLive2D.CharacterId])
            {
                l2DControllerTypeB.ShowModelLeft(sysL2DShow.systemLive2D.CharacterId);
                l2DControllerTypeB.SetModelPositionLeft(modelPos);
                l2DControllerTypeB.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            }
            l2DControllerTypeB.modelL.PlayAnimation(sysL2DShow.systemLive2D.Motion, sysL2DShow.systemLive2D.Expression);
            for (int i = 0; i < safeFrame; i++)
            {
                yield return 1;
            }
            l2DControllerTypeB.FadeInLeft(fadeInTime);
            yield return new WaitForSeconds(fadeInTime);
            string voiceName = $"{sysL2DShow.systemLive2D.AssetbundleName}-{sysL2DShow.systemLive2D.Voice}";
            l2DControllerTypeB.modelL.PlayVoice(audioData.GetValue(voiceName));
        }

        public void FadeOutModel()
        {
            l2DControllerTypeB.FadeOutLeft();
        }

        public void FadeOutText()
        {
            StopAllCoroutines();
            balloon.FadeOut();
            if (txtTimeRange) txtTimeRange.DOFade(0, textFadeTime);
        }
    }
}