using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace SekaiTools.UI.Radio
{
    public enum MessageType
    {
        error,
        success,
        system
    }
    [RequireComponent(typeof(RectTransform))]
    public class Radio_MessageLayer_Item : MonoBehaviour
    {
        public GraphicColorMapping bgImage;
        public Text text;

        public float fadeDuration = 0.3f;
        public float disappearAfter = 16;

        [System.NonSerialized] public MessageType messageType;
        [System.NonSerialized] public bool isFading = true;

        [System.NonSerialized] public float lifeTime = 0;

        [System.NonSerialized] RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        Radio radio;

        public void Initialize(string message,MessageType messageType,Radio radio)
        {
            text.text = message;
            this.messageType = messageType;
            this.radio = radio;

            bgImage.Alpha = 0;
            text.color = new Color(1, 1, 1, 0);
            ChangeTheme(radio.CurrentTheme);
            radio.OnThemeChange += ChangeTheme;
        }

        public void Fade(float endValue,Action onComplete)
        {
            isFading = true;
            DOTween.To(
                () => bgImage.Alpha,
                (value) => bgImage.Alpha = value,
                endValue, fadeDuration);
            text.DOFade(endValue, fadeDuration).OnComplete(() =>
            {
                isFading = false;
                onComplete();
            });
        }

        private void Update()
        {
            lifeTime += Time.deltaTime;
            if (lifeTime >= disappearAfter && !isFading)
                Fade(0, () => Destroy(gameObject));
        }

        public void ChangeTheme(RadioTheme radioTheme)
        {
            switch (messageType)
            {
                case MessageType.error:
                    bgImage.Color = radioTheme.color_Message_Error;
                    break;
                case MessageType.success:
                    bgImage.Color = radioTheme.color_Message_Success;
                    break;
                case MessageType.system:
                    bgImage.Color = radioTheme.color_Message_System;
                    break;
                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            radio.OnThemeChange -= ChangeTheme;
        }
    }
}