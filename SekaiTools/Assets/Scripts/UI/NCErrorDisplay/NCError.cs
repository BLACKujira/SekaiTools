namespace SekaiTools.UI.NCErrorDisplay
{
    public class NCError
    {
        public int charAId;
        public int charBId;
        public int timesCharAToCharB;
        public int timesCharBToCharA;
        public string description;

        public NCError(int charAId, int charBId, int timesCharAToCharB, int timesCharBToCharA, string description)
        {
            this.charAId = charAId;
            this.charBId = charBId;
            this.timesCharAToCharB = timesCharAToCharB;
            this.timesCharBToCharA = timesCharBToCharA;
            this.description = description;
        }
    }
}