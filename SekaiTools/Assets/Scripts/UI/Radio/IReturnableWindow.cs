namespace SekaiTools.UI.Radio
{
    public interface IReturnableWindow
    {
        ReturnPermission ReturnPermission { get; }
        string SenderUserName { get; set; }
    }
}