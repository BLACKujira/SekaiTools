using System.Collections.Generic;

namespace SekaiTools.StringConverter
{
    /// <summary>
    /// 输入表格，每行第一个值为标准名称，其他值为别名
    /// </summary>
    public class StringConverter_StringAlias
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public StringConverter_StringAlias(string[][] stringAliasForm,bool toLower = false)
        {
            foreach (var row in stringAliasForm)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if(toLower)
                        dictionary[row[i].ToLower()] = row[0];
                    else
                        dictionary[row[i]] = row[0];
                }
            }
        }

        /// <summary>
        /// 查找字符串的标准名称，找不到返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetValue(string str)
        {
            str = str.ToLower();
            if (dictionary.ContainsKey(str))
                return dictionary[str];
            return null;
        }
    }
}