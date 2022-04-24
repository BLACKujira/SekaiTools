using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class PerecntBar : MonoBehaviour
    {
        public string numberFormat = "0.00";
        [SerializeField] Image imageFill;
        [SerializeField] Text percentText;

        public float priority
        {
            get => imageFill.fillAmount;
            set
            { 
                imageFill.fillAmount = value;
                if(percentText)
                    percentText.text = (value * 100f).ToString(numberFormat) + '%';
            }
        }
    }
}