using SekaiTools.DecompiledClass;

namespace SekaiTools.UI.Radio
{
    public class MusicVocalData
    {
        public string filePath;
        public float startAt;
        public MusicVocalType vocalType;
        /// <summary>
        /// 约定游戏版本的字符串为game，完整版的为full
        /// </summary>
        public string musicSize;
        public string[] singers;

        public MusicVocalData(string filePath, float startAt, MusicVocalType vocalType, string musicSize, string[] singers)
        {
            this.filePath = filePath;
            this.startAt = startAt;
            this.vocalType = vocalType;
            this.musicSize = musicSize;
            this.singers = singers;
        }
    }
}