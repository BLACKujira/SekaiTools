using SekaiTools.Count;
using System.Collections.Generic;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DataFrameCharacter : DataFrame
    {
        public DataFrameCharacter(Dictionary<string, float> data, NicknameCountMatrix nicknameCountMatrix, string eventGroup) : base(data, nicknameCountMatrix, eventGroup)
        {
        }
    }
}