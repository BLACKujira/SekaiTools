// Sekai.ScenarioCharacterLayout
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class ScenarioCharacterLayout
    {
        public enum Side
        {
            None,
            Left,
            LeftOver,
            LeftInside,
            Center,
            Right,
            RightOver,
            RightInside,
            LeftUnder,
            LeftInsideUnder,
            CenterUnder,
            RightUnder,
            RightInsideUnder
        }
        public Side PositionSide;
        public int Character2dId;
        public string CostumeType;
        public string MotionName;
        public string FacialName;
        public float OffsetX;

        public ScenarioCharacterLayout(Side side, int character2dId)
        {
        }
        public ScenarioCharacterLayout(Side side, int character2dId, string costumeType, string motionName, string expressionName, float offsetX)
        {
        }
    }
}