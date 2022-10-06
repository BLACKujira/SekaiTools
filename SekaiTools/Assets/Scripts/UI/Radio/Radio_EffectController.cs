using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public partial class Radio_EffectController : MonoBehaviour
    {
        public Radio radio;

        public class Settings
        {
            public Settings_UIColorChange settings_UIColorChange;
        }

        public void Initialize(Settings settings)
        {
            if (settings.settings_UIColorChange.enable)
                Initialize_UIColorChange(settings.settings_UIColorChange);
        }

        #region UI±äÉ«
        public class Settings_UIColorChange
        {
            public bool enable;

            public Settings_UIColorChange(bool enable)
            {
                this.enable = enable;
            }
        }

        void Initialize_UIColorChange(Settings_UIColorChange settings)
        {
            radio.musicLayer.onMusicChange += (music) =>
            {
                RadioTheme radioTheme;
                MusicTag topPriorityTag = music.musicData.musicTag.TopPriorityTag;
                switch (topPriorityTag)
                {
                    case MusicTag.all:
                        radioTheme = radio.themeSet[1];
                        break;
                    case MusicTag.vocaloid:
                        radioTheme = radio.themeSet[1];
                        break;
                    case MusicTag.theme_park:
                        radioTheme = radio.themeSet[5];
                        break;
                    case MusicTag.street:
                        radioTheme = radio.themeSet[4];
                        break;
                    case MusicTag.idol:
                        radioTheme = radio.themeSet[3];
                        break;
                    case MusicTag.school_refusal:
                        radioTheme = radio.themeSet[6];
                        break;
                    case MusicTag.light_music_club:
                        radioTheme = radio.themeSet[2];
                        break;
                    case MusicTag.other:
                        radioTheme = radio.themeSet[1];
                        break;
                    default:
                        radioTheme = radio.themeSet[1];
                        break;
                }
                radio.ChangeTheme(radioTheme);
            };
        }
        #endregion

    }
}