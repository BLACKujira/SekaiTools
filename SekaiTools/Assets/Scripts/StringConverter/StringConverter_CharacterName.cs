using System.Collections.Generic;

namespace SekaiTools.StringConverter
{
    public class StringConverter_CharacterName
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();

        public StringConverter_CharacterName(string[][] charNameForm)
        {
            //游戏角色
            for (int i = 0; i < 26; i++)
            {
                int id = i + 1;
                string[] row = charNameForm[i];
                for (int j = 0; j < 2; j++)
                {
                    string format = "00";
                    dictionary[(id).ToString(format.Substring(0, j + 1))] = id;//ID
                }
                if (i <= 23)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        dictionary[row[j]] = id;//普通
                        dictionary[row[j].Replace(" ", "")] = id;//去空格
                        string[] name = row[j].Split(' ');
                        dictionary[name[0]] = id;//姓
                        dictionary[name[1]] = id;//名
                    }
                    for (int j = 3; j < row.Length; j++)
                    {
                        dictionary[row[j]] = id;
                    }
                }
                else
                {
                    dictionary[row[0]] = id;
                    dictionary[row[0].ToLower()] = id;
                    dictionary[row[0].ToUpper()] = id;

                    for (int j = 1; j < row.Length; j++)
                    {
                        dictionary[row[j]] = id;
                    }
                }
            }
        }

        /// <summary>
        /// 查找指定角色，返回其ID，找不到返回-1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetValue(string name)
        {
            name = name.ToLower();
            if (dictionary.ContainsKey(name))
                return dictionary[name];
            return -1;
        }
    }
}