using SekaiTools.DecompiledClass;
using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioCardAppreciation : MonoBehaviour
    {
        public Toggle toggle_enable;
        public FolderSelectItem folder_Cards;
        public StringListEditItem stringList_Extensions;
        public InputField inputField_SwitchingTime;
        public InputField inputField_MinimumSwitchingTime;
        public CardRarityFilterItem cardRarityFilterItem;

        List<string> extensions = new List<string>() { ".png" };

        public Radio_CardAppreciationLayer.Settings Settings
        {
            get
            {
                Radio_CardAppreciationLayer.Settings settings = new Radio_CardAppreciationLayer.Settings();
                settings.enable = toggle_enable.isOn;
                settings.masterCards = EnvPath.GetTable<MasterCard>("cards");
                settings.switchingTime = float.Parse(inputField_SwitchingTime.text);
                settings.minimumSwitchingTime = float.Parse(inputField_MinimumSwitchingTime.text);
                settings.cardImageFolder = folder_Cards.SelectedPath;
                settings.extensions = extensions.ToArray();
                settings.displayCardRarities = cardRarityFilterItem.cardRarityTypes;
                return settings;
            }
        }

        public void Initialize()
        {
            folder_Cards.defaultPath = $"{EnvPath.Assets}/character/member";
            folder_Cards.ResetPath();
            stringList_Extensions.Initialize(extensions);
        }
    }
}