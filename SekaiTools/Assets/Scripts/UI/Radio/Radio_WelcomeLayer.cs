using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    public class Radio_WelcomeLayer : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public GraphicColorMapping tipBg;
        public Text tipText;
        public ContentSizeFitter contentSizeFitter;
        [Header("Settings")]
        public float tipFadeTime = 0.5f;

        public ConfigUIItem[] configUIItems => new ConfigUIItem[]
        {
            new ConfigUIItem_StringListLong("提示文字","主界面",()=>tips,(value)=>tips = value)
        };

        List<string> tips = new List<string>();
        float tipStayTime = 15;
        int currentTipId = 0;

        public class Settings
        {
            public List<string> tips = new List<string>();
            public float tipStayTime = 15;
        }

        private void Awake()
        {
            radio.OnThemeChange += (theme) =>
            {
                tipBg.Color = theme.color_UI;
            };
        }

        public void Initialize(Settings settings)
        {
            tips = settings.tips;
            tipStayTime = settings.tipStayTime;
            tipBg.Color = radio.CurrentTheme.color_UI;
        }

        public IEnumerator ShowTips()
        {
            while (true)
            {
                if(tips.Count!=0)
                {
                    if (currentTipId >= tips.Count)
                        currentTipId = 0;
                    tipText.text = tips[currentTipId];
                    yield return 1;
                    contentSizeFitter.enabled = false;
                    contentSizeFitter.enabled = true;
                    DOTween.To(
                        () => tipBg.Alpha,
                        (value) => tipBg.Alpha = value,
                        1,tipFadeTime);
                    tipText.DOFade(1, tipFadeTime);
                    yield return new WaitForSeconds(tipFadeTime);
                    yield return new WaitForSeconds(tipStayTime);
                    DOTween.To(
                        () => tipBg.Alpha,
                        (value) => tipBg.Alpha = value,
                        0, tipFadeTime);
                    tipText.DOFade(0, tipFadeTime);
                    yield return new WaitForSeconds(tipFadeTime);
                    currentTipId++;
                }
                else
                {
                    yield return 1;
                }
            }
        }
    }
}