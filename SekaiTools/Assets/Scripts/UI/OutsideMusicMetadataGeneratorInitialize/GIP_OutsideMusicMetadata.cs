using SekaiTools.DecompiledClass;
using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.OutsideMusicMetadataGeneratorInitialize
{
    public class GIP_OutsideMusicMetadata : MonoBehaviour
    {
        public Dropdown dropdownVocalType;
        public InputField InputFieldSizeText;

        public MusicVocalType musicVocalType => (MusicVocalType)dropdownVocalType.value;
        public string vocalSize => InputFieldSizeText.text;
    }
}