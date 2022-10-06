// Sekai.MasterCardParameter
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterCardParameter
    {
        public int id;
        public int cardId;
        public int cardLevel;
        public string cardParameterType;
        public int power;
        [IgnoreMember]
        public CardParameterType CardParameterType
        {
            get
            {
                return default;
            }
        }
    }
}