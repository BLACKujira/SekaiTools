using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GlobalSettings
{
    public class GlobalSettingsButton : MonoBehaviour
    {
        public Window inWindow;
        public Window globalSettingsWindowPrefab;

        public void OpenGlobalSettings()
        {
            Window globalSettingsWindow = Instantiate(globalSettingsWindowPrefab);
            globalSettingsWindow.Initialize(inWindow);
            globalSettingsWindow.Show();
        }
    }
}