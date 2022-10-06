using SekaiTools.DecompiledClass;
using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioEffect : MonoBehaviour
    {
        [Header("UIColorChange")]
        public Toggle toggle_UIColorChange_enable;
        
        public Radio_EffectController.Settings Settings
        {
            get
            {
                Radio_EffectController.Settings settings = new Radio_EffectController.Settings();
                settings.settings_UIColorChange = new Radio_EffectController.Settings_UIColorChange(toggle_UIColorChange_enable.isOn);
                if(toggle_UIColorChange_enable.isOn)
                {

                }
                return settings;
            }
        }
    }
}