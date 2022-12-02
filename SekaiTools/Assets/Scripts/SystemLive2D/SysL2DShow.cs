using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class SysL2DShow
    {
        public string translationText = null;
        public string dateTimeOverrideText = null;
        public MergedSystemLive2D systemLive2D;

        public SysL2DShow(MergedSystemLive2D systemLive2D)
        {
            this.systemLive2D = systemLive2D;
        }
    }
}