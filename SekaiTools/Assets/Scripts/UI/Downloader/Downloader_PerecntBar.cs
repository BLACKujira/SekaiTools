using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Downloader
{
    public class Downloader_PerecntBar : PerecntBar
    {
        [SerializeField] Text infoText;

        public string info { set => infoText.text = "※" + value; }
    }
}