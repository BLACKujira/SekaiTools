using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Downloader
{
    public class Downloader_PerecntBar : MonoBehaviour
    {
        [SerializeField] Image imageFill;
        [SerializeField] Text percentText;
        [SerializeField] Text infoText;

        public float priority 
        { 
            get => imageFill.fillAmount;
            set { imageFill.fillAmount = value;percentText.text = (value*100f).ToString("0.00") + '%'; } 
        }
        public string info { set => infoText.text = "※" + value; }
    }
}