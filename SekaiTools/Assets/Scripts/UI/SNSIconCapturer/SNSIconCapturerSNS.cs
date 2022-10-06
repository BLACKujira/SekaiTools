using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SNSIconCapturer
{

    public class SNSIconCapturerSNS : SNSIconCapturer
    {
        [Header("Components")]
        public Image bgImage;
        [Header("Settings")]
        public Texture2D mask;

        public override void Initialize(SNSIconCapturerSettings sNSIconCapturerSettings)
        {
            base.Initialize(sNSIconCapturerSettings);
            onItemChange += (sNSIconCaptureItem) =>
                bgImage.color = sNSIconCaptureItem.bgColor;
            postProcessing = (t2d) => ExtensionTools.ApplyMask(t2d, mask);
        }
    }
}