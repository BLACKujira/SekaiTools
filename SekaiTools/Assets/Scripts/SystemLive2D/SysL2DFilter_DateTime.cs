using System;
using System.Collections.Generic;
using System.Linq;

namespace SekaiTools.SystemLive2D
{
    [Serializable]
    public class SysL2DFilter_DateTime : SysL2DFilter
    {
        public DateTime dateTimeStart;
        public DateTime dateTimeEnd;

        public SysL2DFilter_DateTime(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            this.dateTimeStart = dateTimeStart;
            this.dateTimeEnd = dateTimeEnd;
        }

        public override List<MergedSystemLive2D> ApplyFilter(List<MergedSystemLive2D> listIn)
        {
            IEnumerable<MergedSystemLive2D> enumerable =
                listIn.Where((sysL2d) =>
                {
                    foreach (var msL2d in sysL2d.masterSystemLive2Ds)
                    {
                        DateTime publishedAt = ExtensionTools.UnixTimeMSToDateTimeTST(msL2d.publishedAt);
                        if (publishedAt > dateTimeStart && publishedAt < dateTimeEnd)
                            return true;
                    }
                    return false;
                });
            return new List<MergedSystemLive2D>(enumerable);
        }
    }
}