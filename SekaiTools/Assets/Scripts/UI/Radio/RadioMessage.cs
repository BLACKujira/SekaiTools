namespace SekaiTools.UI.Radio
{
    public class RadioMessage
    {
        public string userName;
        public MessageType messageType;
        public string messageText;

        public RadioMessage(string userName, MessageType messageType, string messageText)
        {
            this.userName = userName;
            this.messageType = messageType;
            this.messageText = messageText;
        }
    }
}