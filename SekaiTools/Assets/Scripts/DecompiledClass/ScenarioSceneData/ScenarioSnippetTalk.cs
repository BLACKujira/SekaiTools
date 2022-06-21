// Sekai.ScenarioSnippetTalk
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class ScenarioSnippetTalk
    {
        public enum Tension
        {
            Normal
        }
        public enum MotionChangeFactor
        {
            Text,
            PlayTime
        }
        public enum LipSyncMode
        {
            Text,
            Voice,
            Close
        }
        public ScenarioSnippetTalkCharacter[] TalkCharacters;
        public string WindowDisplayName;
        public string Body;
        public Tension TalkTention;
        public LipSyncMode LipSync;
        public MotionChangeFactor MotionChangeFrom;
        public ScenarioSnippetTalkMotion[] Motions;
        public ScenarioSnippetTalkVoice[] Voices;
        public float Speed;
        public int FontSize;
        public bool WhenFinishCloseWindow;
        public bool RequirePlayEffect;
        public int EffectReferenceIdx;
        public bool RequirePlaySound;
        public int SoundReferenceIdx;
    }
}