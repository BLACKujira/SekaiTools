using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniGIFGenerator
{
    public class SpineAniGIFGenerator_ModeArea : MonoBehaviour
    {
        public SpineAniGIFGenerator spineAniGIFGenerator;
        [Header("Components")]
        public Toggle toggleGIF;
        public Toggle togglePNGWITHBG;
        public Toggle togglePNGNOBG;

        public SpineAniGIFGenerator.GenerateMode generateMode
        {
            get
            {
                if (toggleGIF.isOn) return SpineAniGIFGenerator.GenerateMode.GIF;
                else if (togglePNGWITHBG.isOn) return SpineAniGIFGenerator.GenerateMode.PNGWITHBG;
                else return SpineAniGIFGenerator.GenerateMode.PNGNOBG;
            }
        }
    }
}