namespace SekaiTools.UI.Radio
{
    public class MusicOrderInfo
    {
        public string musicName;
        public string[] filters;

        public MusicOrderInfo(string musicName, string[] filters)
        {
            this.musicName = musicName;
            this.filters = filters;
        }
    }
}