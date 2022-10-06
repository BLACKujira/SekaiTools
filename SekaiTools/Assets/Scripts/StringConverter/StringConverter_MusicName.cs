using System.Collections;
using System.Collections.Generic;

namespace SekaiTools.StringConverter
{
    public class StringConverter_MusicName : StringConverter_Base<int>
    {
        protected Dictionary<int, string[]> aliases = new Dictionary<int, string[]>();

        public StringConverter_MusicName(string[][] musicNameForm):base(musicNameForm)
        {
            foreach (var row in musicNameForm)
            {
                int id = int.Parse(row[0]);
                for (int i = 0; i < 4; i++)
                {
                    string zeroFill = "0000";
                    dictionary[id.ToString(zeroFill.Substring(0, i + 1))] = id;
                }
                for (int i = 1; i < row.Length; i++)
                {
                    dictionary[row[i].ToLower()] = id;
                    dictionary[row[i].ToLower().Replace(" ","")] = id;
                }
                aliases[id] = new List<string>(row).GetRange(2, row.Length - 2).ToArray();
            }
        }

        /// <summary>
        /// 查找指定歌曲的id，默认转换为小写，找不到返回-1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override int GetValue(string name)
        {
            name = name.ToLower();
            if (dictionary.ContainsKey(name))
                return dictionary[name];
            return -1;
        }

        public string[] GetAliases(int musicId)
        {
            if (!aliases.ContainsKey(musicId))
                return null;
            else return aliases[musicId];
        }
    }
}