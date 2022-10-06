using SekaiTools.UI.Radio;

namespace SekaiTools.UI.RadioInitialize
{
    public class RadioCMDInputInitializePart
    {
        public RadioCommandinput_Base managerObjectPrefab;
        public RadioCommandinputSettingsBase settings;

        public RadioCMDInputInitializePart(RadioCommandinput_Base managerObjectPrefab, RadioCommandinputSettingsBase settings)
        {
            this.managerObjectPrefab = managerObjectPrefab;
            this.settings = settings;
        }
    }
}