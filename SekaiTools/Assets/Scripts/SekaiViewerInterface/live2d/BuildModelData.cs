using System;

namespace SekaiTools.SekaiViewerInterface.Pages.Live2D
{
    [Serializable]
    public class BuildModelData
    {
        public string Moc3FileName;
        public string[] TextureNames;
        public string PhysicsFileName;
        public string UserDataFileName;
        public Additionalmotiondata[] AdditionalMotionData;
        public object[] CategoryRules;
    }
}