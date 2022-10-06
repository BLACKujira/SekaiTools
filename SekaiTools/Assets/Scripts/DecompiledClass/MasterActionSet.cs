// Sekai.MasterActionSet

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterActionSet
    {
        public const string TYPE_NORMAL = "normal";
        public const string TYPE_LIMITED = "limited";
        public int id;
        public int areaId;
        public string scriptId;
        public string actionSetType;
        public string scenarioId;
        public int[] characterIds;
    }
}