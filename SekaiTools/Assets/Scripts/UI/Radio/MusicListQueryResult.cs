using System.Collections.Generic;

namespace SekaiTools.UI.Radio
{
    public class MusicListQueryResult
    {
        public MusicListQueryInfo musicListQueryInfo;
        public List<MusicListItem> resultItems;

        public MusicListQueryResult(MusicListQueryInfo musicListQueryInfo, List<MusicListItem> resultItems)
        {
            this.musicListQueryInfo = musicListQueryInfo;
            this.resultItems = resultItems;
        }
    }
}