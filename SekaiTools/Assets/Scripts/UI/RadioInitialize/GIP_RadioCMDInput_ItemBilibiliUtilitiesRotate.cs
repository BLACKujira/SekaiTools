using SekaiTools.UI.Radio;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioCMDInput_ItemBilibiliUtilitiesRotate : GIP_RadioCMDInput_ItemBase
    {
        public InputField inputField_roomId;
        public InputField inputField_AutoReconnectTime;
        public InputField inputField_ConnectRetryWaittime;
        public InputField inputField_SurvivalTimeThreshold;
        public InputField inputField_NumberOfInstance;

        public override RadioCommandinputSettingsBase Settings
        {
            get
            {
                RadioCommandinput_BilibiliUtilitiesRotate.Settings settings = new RadioCommandinput_BilibiliUtilitiesRotate.Settings();
                settings.roomId = int.Parse(inputField_roomId.text);
                settings.autoReconnectTime = float.Parse(inputField_AutoReconnectTime.text) * 60;
                settings.survivalTimeThreshold = float.Parse(inputField_SurvivalTimeThreshold.text);
                settings.numberOfInstance = int.Parse(inputField_NumberOfInstance.text);

                settings.instanceSettings = new RadioCommandinput_BilibiliUtilities_Instance.Settings();
                settings.instanceSettings.connectRetryWaittime = int.Parse(inputField_ConnectRetryWaittime.text);

                return settings;
            }
        }
    }
}