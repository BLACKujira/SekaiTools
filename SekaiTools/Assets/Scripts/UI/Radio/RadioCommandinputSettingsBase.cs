namespace SekaiTools.UI.Radio
{
    public abstract class RadioCommandinputSettingsBase
    {
        [System.Serializable]
        public class IncorrectSettingTypeException : System.Exception
        {
            public IncorrectSettingTypeException() { }
            public IncorrectSettingTypeException(string message) : base(message) { }
            public IncorrectSettingTypeException(string message, System.Exception inner) : base(message, inner) { }
            protected IncorrectSettingTypeException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}