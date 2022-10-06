namespace SekaiTools.UI.Radio
{
    public class SerifQueryInfo
    {
        public string charA;
        public string charB;
        public string[] filters;

        public SerifQueryInfo(string charA, string charB, string[] filters)
        {
            this.charA = charA;
            this.charB = charB;
            this.filters = filters;
        }
    }
}