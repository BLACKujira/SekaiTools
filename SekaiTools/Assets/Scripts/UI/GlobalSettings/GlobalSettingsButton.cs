using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GlobalSettings
{
    public class GlobalSettingsButton : MonoBehaviour
    {
        public Window globalSettingsWindowPrefab;

        public void OpenGlobalSettings()
        {
            WindowController.windowController.currentWindow.OpenWindow(globalSettingsWindowPrefab);
        }
    }
}