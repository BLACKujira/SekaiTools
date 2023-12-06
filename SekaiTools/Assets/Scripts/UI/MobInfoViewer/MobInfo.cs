using System.Collections.Generic;

namespace SekaiTools.UI
{
    public class MobInfo
    {
        public int characterId;
        public HashSet<int> character2dIds = new HashSet<int>();
        public HashSet<string> assetNames = new HashSet<string>();
        public Dictionary<string, int> nicknames = new Dictionary<string, int>();
        public Dictionary<string, HashSet<int>> serifs = new Dictionary<string, HashSet<int>>();

        public MobInfo(int characterId)
        {
            this.characterId = characterId;
        }

        public void AddSerif(string fileName, int refIdx)
        {
            if (!serifs.ContainsKey(fileName))
            {
                serifs[fileName] = new HashSet<int>();
            }
            serifs[fileName].Add(refIdx);
        }
    }
}
