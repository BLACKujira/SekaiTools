namespace SekaiTools.StringConverter
{
    public class StringConverter_UnitName : StringConverter_Base<Unit>
    {
        public StringConverter_UnitName(string[][] unitNameForm) : base(unitNameForm)
        {
            foreach (var row in unitNameForm)
            {
                int id = int.Parse(row[0]);
                for (int i = 0; i < 2; i++)
                {
                    string zeroFill = "00";
                    dictionary[id.ToString(zeroFill.Substring(0, i + 1))] = (Unit)id;
                }
                for (int i = 1; i < row.Length; i++)
                {
                    dictionary[row[i].ToLower()] = (Unit)id;
                    dictionary[row[i].ToLower().Replace(" ", "")] = (Unit)id;
                }

            }
        }

        /// <summary>
        /// 获取字符串对应的团队，找不到则返回Unit.none
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override Unit GetValue(string name)
        {
            return GetValueNullable(name) ?? Unit.none;
        }

        /// <summary>
        /// 获取字符串对应的团队，区分Unit.none与无效输入
        /// </summary>
        /// <param name="name"></param>
        public Unit? GetValueNullable(string name)
        {
            name = name.ToLower();
            if (dictionary.ContainsKey(name))
                return dictionary[name];
            return null;
        }
    }
}