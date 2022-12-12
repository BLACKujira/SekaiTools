using SekaiTools.DecompiledClass;
using SekaiTools.StringConverter;
using SekaiTools.UI.Radio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioMusicPlayer : MonoBehaviour
    {
        [Header("Components_CSVs")]
        public LoadFileSelectItem file_musicNames;
        public LoadFileSelectItem file_characterNames;
        public LoadFileSelectItem file_outsideCharacterNames;
        public LoadFileSelectItem file_cpNames;
        public LoadFileSelectItem file_vocalTypeNames;
        public LoadFileSelectItem file_sizeTypeNames;
        public LoadFileSelectItem file_unitNames;
        [Header("Components_MusicFiles")]
        public FolderSelectItem folder_SVMusic;
        public FolderSelectItem folder_OutsideMusic;
        public StringListEditItem stringList_Extensions;
        [Header("Components_PlayerSettings")]
        public InputField inputField_maxPlaylistLength;
        public StringListEditItem stringList_bannedMusicIds;

        List<string> extensions = new List<string>() { ".mp3", ".wav", ".ogg" };
        List<string> bannedMusicIds = new List<string> { "95" };
        public Radio_MusicLayer.Settings settings
        {
            get
            { 
                Radio_MusicLayer.Settings settings = new Radio_MusicLayer.Settings();
                settings.musicNames = new StringConverter_MusicName(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_musicNames.SelectedPath)));
                settings.singerNames = new StringConverter_SingerName(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_characterNames.SelectedPath)),
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_outsideCharacterNames.SelectedPath)));
                settings.cpNames = new StringConverter_StringAlias(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_cpNames.SelectedPath)));
                settings.vocalTypeNames = new StringConverter_StringAlias(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_vocalTypeNames.SelectedPath)));
                settings.sizeTypeNames = new StringConverter_StringAlias(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_sizeTypeNames.SelectedPath)));
                settings.unitNames = new StringConverter_UnitName(
                    CSVTools.LoadCSV(
                        File.ReadAllText(file_unitNames.SelectedPath)));

                settings.masterMusics = EnvPath.GetTable<MasterMusic>("musics");
                settings.masterMusicVocals = EnvPath.GetTable<MasterMusicVocal>("musicVocals");
                settings.masterOutsideCharacters = EnvPath.GetTable<MasterOutsideCharacter>("outsideCharacters");
                settings.masterMusicTags = EnvPath.GetTable<MasterMusicTag>("musicTags");

                settings.musicFolderSV = folder_SVMusic.SelectedPath;
                settings.musicFolderOutside = folder_OutsideMusic.SelectedPath;

                settings.musicExtensions = extensions.ToArray();
                settings.maxPlaylistLength = int.Parse(inputField_maxPlaylistLength.text);

                HashSet<int> bannedMusics = new HashSet<int>();
                foreach (var item in bannedMusicIds)
                {
                    bannedMusics.Add(int.Parse(item));
                }
                settings.bannedMusics = bannedMusics;

                return settings;
            }
        }

        public void Initialize()
        {
            file_musicNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "MusicNameCSV.txt");
            file_musicNames.ResetPath();
            file_characterNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "CharNameCSV.txt");
            file_characterNames.ResetPath();
            file_outsideCharacterNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "OutsideCharVocalCSV.txt");
            file_outsideCharacterNames.ResetPath();
            file_cpNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "CPNameCSV.txt");
            file_cpNames.ResetPath();
            file_vocalTypeNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "VocalTypeCSV.txt");
            file_vocalTypeNames.ResetPath();
            file_sizeTypeNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "SizeTypeCSV.txt");
            file_sizeTypeNames.ResetPath();
            file_unitNames.defaultPath = Path.Combine(EnvPath.Inbuilt, "UnitNameCSV.txt");
            file_unitNames.ResetPath();

            folder_SVMusic.defaultPath = $"{EnvPath.Assets}/music/long";
            folder_SVMusic.ResetPath();

            stringList_Extensions.Initialize(extensions);
            stringList_bannedMusicIds.Initialize(bannedMusicIds);
        }

    }
}