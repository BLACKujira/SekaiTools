using System.Collections.Generic;

namespace SekaiTools.StringConverter
{
    /// <summary>
    /// 获取歌手正式名称，主要用于点歌系统选择another vocal
    /// </summary>
    public class StringConverter_SingerName
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public StringConverter_SingerName(string[][] charNameForm, string[][] outsideCharNameForm)
        {
            //游戏角色
            for (int i = 0; i < charNameForm.Length; i++)
            {
                string[] row = charNameForm[i];
                for (int j = 0; j < 2; j++)
                {
                    string format = "00";
                    dictionary[(i+1).ToString(format.Substring(0, j + 1))] = row[0];//ID
                }
                if (i <= 23)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        dictionary[row[j]] = row[0];//普通
                        dictionary[row[j].Replace(" ","")] = row[0];//去空格
                        string[] name = row[j].Split(' ');
                        dictionary[name[0]] = row[0];//姓
                        dictionary[name[1]] = row[0];//名
                    }
                    for (int j = 3; j < row.Length; j++)
                    {
                        dictionary[row[j]] = row[0];
                    }
                }
                else
                {
                    dictionary[row[0]] = row[0];
                    dictionary[row[0].ToLower()] = row[0];
                    dictionary[row[0].ToUpper()] = row[0];

                    for (int j = 1; j < row.Length; j++)
                    {
                        dictionary[row[j]] = row[0];
                    }
                }
            }

            //外部角色
            for (int i = 0; i < outsideCharNameForm.Length; i++)
            {
                string[] row = outsideCharNameForm[i];
                for (int j = 0; j < row.Length; j++)
                {
                    dictionary[row[j]] = row[0];
                }
            }
        }

        /// <summary>
        /// 查找指定歌手，返回其正式名称，找不到返回null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            name = name.ToLower();
            if (dictionary.ContainsKey(name))
                return dictionary[name];
            return null;
        }
    }
}