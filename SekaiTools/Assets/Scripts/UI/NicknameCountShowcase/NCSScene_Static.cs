using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Static : NCSScene
    {
        public override string information => $"{description} (静态) , 持续时间 {holdTime.ToString("0.00")}";

        public override void Refresh()
        {
            
        }
    }
}