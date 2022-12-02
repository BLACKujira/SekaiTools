using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class SysL2DFilterSet
    {
        public SysL2DFilter_DateTime filter_DateTime = null;
        public SysL2DFilter_Character filter_Character = null;
        public SysL2DFilter_Unit filter_Unit = null;

        public bool IsEmpty
        {
            get
            {
                foreach (var filter in Filters)
                {
                    if (filter != null)
                        return false;
                }
                return true;
            }
        }

        public SysL2DFilter[] Filters => new SysL2DFilter[]
            {
                filter_DateTime,
                filter_Character,
                filter_Unit
            };

        public List<MergedSystemLive2D> ApplyFilters(List<MergedSystemLive2D> listIn)
        {
            List<MergedSystemLive2D> listOut = listIn;
            foreach (var filter in Filters)
            {
                if (filter != null)
                    listOut = filter.ApplyFilter(listOut);
            }
            return listOut;
        }

        public SysL2DFilterSet Clone()
        {
            SysL2DFilterSet sysL2DFilterSet = new SysL2DFilterSet();
            if (filter_DateTime != null) sysL2DFilterSet.filter_DateTime
                     = new SysL2DFilter_DateTime(filter_DateTime.dateTimeStart,filter_DateTime.dateTimeEnd);
            if (filter_Character != null) sysL2DFilterSet.filter_Character
                    = JsonUtility.FromJson<SysL2DFilter_Character>(JsonUtility.ToJson(filter_Character));
            if (filter_Unit != null) sysL2DFilterSet.filter_Unit
                    = JsonUtility.FromJson<SysL2DFilter_Unit>(JsonUtility.ToJson(filter_Unit));
            return sysL2DFilterSet;
        }
    }
}