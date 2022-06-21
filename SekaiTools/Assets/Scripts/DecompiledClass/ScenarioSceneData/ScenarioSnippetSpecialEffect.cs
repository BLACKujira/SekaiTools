// Sekai.ScenarioSnippetSpecialEffect
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class ScenarioSnippetSpecialEffect
    {
        public enum SpecialEffectType
        {
            None,
            BlackIn,
            BlackOut,
            WhiteIn,
            WhiteOut,
            ShakeScreen,
            ShakeWindow,
            ChangeBackground,
            Telop,
            FlashbackIn,
            FlashbackOut,
            ChangeCardStill,
            AmbientColorNormal,
            AmbientColorEvening,
            AmbientColorNight,
            PlayScenarioEffect,
            StopScenarioEffect,
            ChangeBackgroundStill,
            PlaceInfo,
            Movie,
            SekaiIn,
            SekaiOut,
            AttachCharacterShader,
            SimpleSelectable,
            FullScreenText,
            StopShakeScreen,
            StopShakeWindow,
            SepiaIn,
            SepiaOut
        }
        public SpecialEffectType EffectType;
        public string StringVal;
        public string StringValSub;
        public float Duration;
        public int IntVal;
        public ScenarioSnippetSpecialEffect()
        {
        }
    }
}