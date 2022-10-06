using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    [CreateAssetMenu(menuName = "SekaiTools/Radio/RadioThemeSet")]
    public class RadioThemeSet : ScriptableObject
    {
        public List<RadioTheme> radioThemes = new List<RadioTheme>();
        public RadioTheme this[int index]=>radioThemes[index];
    }
}