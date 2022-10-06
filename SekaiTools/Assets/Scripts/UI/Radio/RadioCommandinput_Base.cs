using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public abstract class RadioCommandinput_Base : MonoBehaviour 
    {
        protected Radio radio;
        public string itemName;
        [TextArea] public string description;

        public abstract void Initialize(Radio radio, RadioCommandinputSettingsBase settings);
    }
}