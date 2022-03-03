using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SystemSettings
{
    public class SystemSettings_WindowSettings : MonoBehaviour
    {
        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public Toggle windowedToggle;
        public Toggle fullScreenToggle;
        [Header("Settings")]
        public List<Vector2Int> resolutions;

        private void Awake()
        {
            GenerateToggles();
            if (Screen.fullScreen) fullScreenToggle.isOn = true;
            else windowedToggle.isOn = true;
            fullScreenToggle.onValueChanged.AddListener(ChangeToFullScreen);
            windowedToggle.onValueChanged.AddListener(ChangeToWindowed);
        }

        public void ChangeToWindowed(bool value)
        {
            if(value)
                Screen.fullScreen = false;
        }
        public void ChangeToFullScreen(bool value)
        {
            if(value)
                Screen.fullScreen = true;
        }

        public void GenerateToggles()
        {
            toggleGenerator.Generate(resolutions.Count, (Toggle toggle, int id) =>
            { 
                toggle.GetComponentInChildren<Text>().text = $"{resolutions[id].x}X{resolutions[id].y}";
                if (resolutions[id].x == Screen.width && resolutions[id].y == Screen.height) toggle.isOn = true;
            },
            (bool value,int id) => { if (value) Screen.SetResolution(resolutions[id].x, resolutions[id].y,Screen.fullScreen); }
            );
        }
    }
}