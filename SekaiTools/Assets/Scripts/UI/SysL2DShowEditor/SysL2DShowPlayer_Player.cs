using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using SekaiTools.SystemLive2D;

namespace SekaiTools.UI.SysL2DShowPlayer
{
    public class SysL2DShowPlayer_Player : MonoBehaviour
    {
        public L2DControllerTypeB l2DControllerTypeB;
        public SysL2DShowPlayer_Player_Balloon balloon;

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
        }

        public void SetData(SysL2DShow sysL2DShow)
        {
            this.sysL2DShow = sysL2DShow;
        }

        public void Play(bool showOriginText = false)
        {
            balloon.SetSerif(showOriginText ? sysL2DShow.systemLive2D.Serif : sysL2DShow.translationText);
            balloon.Play();
            l2DControllerTypeB.ShowModelLeft(sysL2DShow.systemLive2D.CharacterId);
            l2DControllerTypeB.SetModelPositionLeft(modelPos);
            l2DControllerTypeB.modelL.transform.localScale = new Vector3(modelScale, modelScale, 1);
            l2DControllerTypeB.FadeInLeft();
            l2DControllerTypeB.modelL.PlayAnimation(sysL2DShow.systemLive2D.Motion, sysL2DShow.systemLive2D.Expression);
            string voiceName = $"{sysL2DShow.systemLive2D.AssetbundleName}-{sysL2DShow.systemLive2D.Voice}";
            l2DControllerTypeB.modelL.PlayVoice(audioData.GetValue(voiceName));
        }
    }
}