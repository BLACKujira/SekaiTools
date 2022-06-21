// Sekai.ScenarioSnippetCharacterLayout
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class ScenarioSnippetCharacterLayout
    {
        public enum ActionType
        {
            None,
            Move,
            Apper,
            Hide,
            ShakeX,
            ShakeY
        }
        public enum LayoutMoveSpeedType
        {
            Normal,
            Fast,
            Slow
        }
        public enum LayoutDepthType
        {
            NotSet,
            Front,
            Back
        }
        public ActionType Type;
        public ScenarioCharacterLayout.Side SideFrom;
        public float SideFromOffsetX;
        public ScenarioCharacterLayout.Side SideTo;
        public float SideToOffsetX;
        public LayoutDepthType DepthType;
        public int Character2dId;
        public string CostumeType;
        public string MotionName;
        public string FacialName;
        public LayoutMoveSpeedType MoveSpeedType;

        public float MoveSpeed
        {
            get
            {
                return default;
            }
        }
        public ScenarioSnippetCharacterLayout()
        {
        }
        public ScenarioSnippetCharacterLayout(int Character2dId, ActionType type, ScenarioCharacterLayout.Side sideFrom, ScenarioCharacterLayout.Side sideTo, LayoutMoveSpeedType moveSpeedType)
        {
        }
    }
}