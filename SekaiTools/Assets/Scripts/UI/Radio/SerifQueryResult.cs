using System.Collections.Generic;
using SekaiTools.Count;

namespace SekaiTools.UI.Radio
{
    public class SerifQueryResult
    {
        public int charAId;
        public int charBId;
        public SerifQueryInfo serifQueryInfo;
        public List<NicknameCountMatrix> resultCountMatrices;

        public SerifQueryResult(int charAId, int charBId, SerifQueryInfo serifQueryInfo, List<NicknameCountMatrix> resultCountMatrices)
        {
            this.charAId = charAId;
            this.charBId = charBId;
            this.serifQueryInfo = serifQueryInfo;
            this.resultCountMatrices = resultCountMatrices;
        }
    }
}