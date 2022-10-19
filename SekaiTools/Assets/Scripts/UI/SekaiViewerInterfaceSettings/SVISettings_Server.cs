using SekaiTools.SekaiViewerInterface;
using SekaiTools.SekaiViewerInterface.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SekaiViewerInterfaceSettings
{
    public class SVISettings_Server : MonoBehaviour
    {
        public Toggle[] assetSeverToggles = new Toggle[3];
        public Toggle[] masterSeverToggles = new Toggle[2];

        private void Awake()
        {
            assetSeverToggles[(int)SekaiViewer.assetSever].isOn = true;
            masterSeverToggles[(int)SekaiViewer.masterSever].isOn = true;

            for (int i = 0; i < assetSeverToggles.Length; i++)
            {
                int id = i;
                assetSeverToggles[id].onValueChanged.AddListener((value) =>
                {
                    if (value)
                        SekaiViewer.assetSever = (AssetSever)id;
                });
            }
            for (int i = 0; i < masterSeverToggles.Length; i++)
            {
                int id = i;
                masterSeverToggles[id].onValueChanged.AddListener((value) =>
                {
                    if (value)
                        SekaiViewer.masterSever = (MasterSever)id;
                });
            }
        }
    }
}