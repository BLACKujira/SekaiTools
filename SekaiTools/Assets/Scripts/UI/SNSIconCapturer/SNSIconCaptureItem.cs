using UnityEngine;
using SekaiTools.Spine;

namespace SekaiTools.UI.SNSIconCapturer
{
    public class SNSIconCaptureItem
    {
        public AtlasAssetPair atlasAssetPair;
        public string animation;
        //public float animationProgress;
        public Color bgColor;

        public SNSIconCaptureItem(AtlasAssetPair atlasAssetPair, string animation, Color bgColor)
        {
            this.atlasAssetPair = atlasAssetPair;
            this.animation = animation;
            this.bgColor = bgColor;
        }
    }
}