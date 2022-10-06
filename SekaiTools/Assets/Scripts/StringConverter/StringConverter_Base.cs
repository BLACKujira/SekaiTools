using System.Collections.Generic;

namespace SekaiTools.StringConverter
{
    public abstract class StringConverter_Base<T>
    {
        protected Dictionary<string, T> dictionary = new Dictionary<string, T>();
        public readonly string[][] rawForm;

        protected StringConverter_Base(string[][] rawForm)
        {
            this.rawForm = rawForm;
        }

        public abstract T GetValue(string name);
    }
}