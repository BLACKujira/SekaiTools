using System.Collections.Generic;
using System.Linq;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class SysL2DFilter_Unit : SysL2DFilter
    {
        public bool[] unitIdMask = new bool[7];

        public SysL2DFilter_Unit(bool[] unitIdMask)
        {
            this.unitIdMask = unitIdMask;
        }

        public override List<MergedSystemLive2D> ApplyFilter(List<MergedSystemLive2D> listIn)
        {
            IEnumerable<MergedSystemLive2D> enumerable =
                from MergedSystemLive2D sysL2D in listIn
                where unitIdMask[(int)sysL2D.UnitType.ToUnit()] == true
                select sysL2D;
            return new List<MergedSystemLive2D>(enumerable);
        }
    }
}