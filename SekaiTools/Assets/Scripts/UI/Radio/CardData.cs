using SekaiTools.DecompiledClass;

namespace SekaiTools.UI.Radio
{
    public class CardData
    {
        public readonly MasterCard masterCard;
        public readonly bool afterTraining;
        public readonly string imageFilePath;

        public CardData(MasterCard masterCard, bool afterTraining, string imageFilePath)
        {
            this.masterCard = masterCard;
            this.afterTraining = afterTraining;
            this.imageFilePath = imageFilePath;
        }
    }
}