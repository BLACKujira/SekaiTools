using System.Collections.Generic;

namespace SekaiTools.UI.Radio
{
    public class MusicListItem
    {
        public MusicData musicData;
        public List<MusicVocalData> musicVocalDatas;

        public MusicListItem(MusicData musicData, List<MusicVocalData> musicVocalDatas)
        {
            this.musicData = musicData;
            this.musicVocalDatas = musicVocalDatas;
        }
    }
}