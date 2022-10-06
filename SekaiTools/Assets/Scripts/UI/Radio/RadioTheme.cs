using UnityEngine;

namespace SekaiTools.UI.Radio
{
    [System.Serializable]
    public class RadioTheme
    {
        public Color color_UI;
        public Color color_Message_Error;
        public Color color_Message_Success;
        public Color color_Message_System;
        public Color color_ProgressBar;
        [ColorUsage(true,true)] public Color color_HDRParticle;

        public static RadioTheme Lerp(RadioTheme a, RadioTheme b,float t)
        {
            RadioTheme radioTheme = new RadioTheme();
            radioTheme.color_UI = Color.Lerp(a.color_UI, b.color_UI, t);
            radioTheme.color_Message_Error = Color.Lerp(a.color_Message_Error, b.color_Message_Error, t);
            radioTheme.color_Message_Success = Color.Lerp(a.color_Message_Success, b.color_Message_Success, t);
            radioTheme.color_Message_System = Color.Lerp(a.color_Message_System, b.color_Message_System, t);
            radioTheme.color_ProgressBar = Color.Lerp(a.color_ProgressBar, b.color_ProgressBar, t);
            radioTheme.color_HDRParticle = Color.Lerp(a.color_HDRParticle, b.color_HDRParticle, t);
            return radioTheme;
        }
    }
}