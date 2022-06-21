using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Tab : MonoBehaviour
    {
        public Text textNormal;
        public Text textSelected;

        public string text 
        {
            set
            {
                textNormal.text = value;
                textSelected.text = value;
            }
        }
    }
}