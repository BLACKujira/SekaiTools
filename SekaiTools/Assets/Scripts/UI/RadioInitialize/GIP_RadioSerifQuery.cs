using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using SekaiTools.StringConverter;
using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioSerifQuery : MonoBehaviour
    {
        public Toggle toggle_enableLayer;
        [Header("Components_CountData")]
        public FolderSelectItem folder_nicknameCountData;
        [Header("Components_CSVs")]
        public LoadFileSelectItem file_filterNames;
        public LoadFileSelectItem file_areaNames;
        public LoadFileSelectItem file_charNames;

        public Radio_SerifQueryLayer.Settings settings
        {
            get
            {
                Radio_SerifQueryLayer.Settings settings = new Radio_SerifQueryLayer.Settings();

                settings.enable = toggle_enableLayer.isOn;
                
                if (settings.enable)
                {
                    settings.filterNames = new StringConverter_StringAlias(
                        CSVTools.LoadCSV(File.ReadAllText(file_filterNames.SelectedPath)));
                    settings.areaNames = new StringConverter_StringAlias(
                        CSVTools.LoadCSV(File.ReadAllText(file_areaNames.SelectedPath)));
                    settings.charNames = new StringConverter_CharacterName(
                        CSVTools.LoadCSV(File.ReadAllText(file_charNames.SelectedPath)));

                    settings.masterAreas = EnvPath.GetTable<MasterArea>("areas");
                    settings.masterActionSets = EnvPath.GetTable<MasterActionSet>("actionSets");
                    settings.masterVirtualLives = EnvPath.GetTable<MasterVirtualLive>("virtualLives");
                    settings.masterEvents = EnvPath.GetTable<MasterEvent>("events");
                    settings.masterCards = EnvPath.GetTable<MasterCard>("cards");

                    settings.nicknameCountData = NicknameCountData.Load(folder_nicknameCountData.SelectedPath);
                }

                return settings;
            }
        }

        public void Initialize()
        {
            file_filterNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "SerifQuerFilterCSV.txt");
            file_filterNames.ResetPath();
            file_areaNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "AreaNameCSV.txt");
            file_areaNames.ResetPath();
            file_charNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "CharNameCSV.txt");
            file_charNames.ResetPath();
        }
    }
}