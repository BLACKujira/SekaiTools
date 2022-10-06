using UnityEngine;

namespace SekaiTools.UI.SNSIconCapturer
{
    public class SNSIconCapturerClear : SNSIconCapturer
    {
        [Header("Settings")]
        public RenderTexture spineRenderTexture;

        RenderTexture lastActive;

        public override void Initialize(SNSIconCapturerSettings sNSIconCapturerSettings)
        {
            base.Initialize(sNSIconCapturerSettings);
            lastActive = RenderTexture.active;
            onItemChange += (item)=>
                RenderTexture.active = spineRenderTexture;
        }

        private void OnDestroy()
        {
            RenderTexture.active = lastActive;
        }
    }
}