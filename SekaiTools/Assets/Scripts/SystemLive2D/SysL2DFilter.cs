using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public abstract class SysL2DFilter
    {
        public abstract List<MergedSystemLive2D> ApplyFilter(List<MergedSystemLive2D> listIn);
    }
}