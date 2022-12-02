using System.Collections.Generic;
using System.Linq;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class SysL2DFilter_Character : SysL2DFilter
    {
        public bool[] characterIdMask = new bool[27];

        public SysL2DFilter_Character(bool[] characterIdMask)
        {
            this.characterIdMask = characterIdMask;
        }

        public override List<MergedSystemLive2D> ApplyFilter(List<MergedSystemLive2D> listIn)
        {
            IEnumerable<MergedSystemLive2D> enumerable = 
                from MergedSystemLive2D sysL2D in listIn
                where characterIdMask[sysL2D.CharacterId] == true
                select sysL2D;
            return new List<MergedSystemLive2D>(enumerable);
        }
    }
}