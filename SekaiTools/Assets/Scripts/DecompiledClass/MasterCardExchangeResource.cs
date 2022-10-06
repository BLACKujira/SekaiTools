// Sekai.MasterCardExchangeResource
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    public class MasterCardExchangeResource
    {
        public string cardRarityType;
        public int seq;
        public int resourceBoxId;
        [IgnoreMember]
        public CardRarityType CardRarityType => (CardRarityType)Enum.Parse(typeof(CardRarityType), cardRarityType);

        public MasterCardExchangeResource()
        {
        }
    }
}