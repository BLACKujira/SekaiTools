namespace SekaiTools.UI.Transition
{
    [System.Serializable]
    public class SerializedTransition
    {
        public string type;
        public string serializedSettings;

        public SerializedTransition(string type, string serializedSettings)
        {
            this.type = type;
            this.serializedSettings = serializedSettings;
        }
    }
}