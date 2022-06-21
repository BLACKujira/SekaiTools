// Sekai.ScenarioSnippetTalkMotion
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class ScenarioSnippetTalkMotion
    {
        public int Character2dId;
        public string MotionName;
        public string FacialName;
        public float TimingSyncValue;

        public ScenarioSnippetTalkMotion(int character2dId, string motionName, string facialName, float timingSyncValue)
        {
            Character2dId = character2dId;
            MotionName = motionName;
            FacialName = facialName;
            TimingSyncValue = timingSyncValue;
        }
    }
}