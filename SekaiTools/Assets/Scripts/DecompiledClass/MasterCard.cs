// Sekai.MasterCard
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterCard
    {
        public const string CARD_ATTR_1 = "cute";
        public const string CARD_ATTR_2 = "cool";
        public const string CARD_ATTR_3 = "pure";
        public const string CARD_ATTR_4 = "happy";
        public const string CARD_ATTR_5 = "mysterious";
        public int id;
        public int seq;
        public int characterId;
        public string cardRarityType;
        public int maxPower;
        public int specialTrainingPower1BonusFixed;
        public int specialTrainingPower2BonusFixed;
        public int specialTrainingPower3BonusFixed;
        public string attr;
        public string supportUnit;
        public int skillId;
        public string cardSkillName;
        public string prefix;
        public string assetbundleName;
        public string gachaPhrase;
        public string flavorText;
        public long releaseAt;
        public MasterCardParameter[] cardParameters;
        public MasterSpecialTrainingCost[] specialTrainingCosts;
        public MasterMasterLessonAchieveResource[] masterLessonAchieveResources;
        public MasterCardExchangeResource[] masterCardExchangeResources;

        [IgnoreMember]
        public UnitType SupportUnitType => (UnitType)Enum.Parse(typeof(UnitType), supportUnit);

        [IgnoreMember]
        public CardRarityType RarityType => (CardRarityType)Enum.Parse(typeof(CardRarityType), cardRarityType);

        [IgnoreMember]
        public CardAttributeType AttributeType => (CardAttributeType)Enum.Parse(typeof(CardAttributeType), attr);
    }
}