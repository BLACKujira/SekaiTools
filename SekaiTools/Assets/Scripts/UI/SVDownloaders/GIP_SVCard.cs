using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.SVDownloaders
{
    public class GIP_SVCard : MonoBehaviour, IGenericInitializationPart
    {
        [Header("Components")]
        public Toggle[] toggleFormats = new Toggle[2];
        public FolderSelectItem folderSelectItem;
        public CharacterFilterDisplayTypeA characterFilterDisplay;
        [Header("Prefab")]
        public Window charIDMaskSelectPrefab;

        string[] formats = { ".png", ".webp" };
        int[] selectedCharacters = new int[0];

        public string format
        {
            get
            {
                for (int i = 0; i < toggleFormats.Length; i++)
                {
                    if (toggleFormats[i].isOn)
                        return formats[i];
                }
                return formats[0];
            }
        }
        public int[] SelectedCharacters => selectedCharacters;

        private void Awake()
        {
            selectedCharacters = new int[26];
            for (int i = 1; i <= 26; i++)
            {
                selectedCharacters[i-1] = i;
            }
            characterFilterDisplay.SetMask(selectedCharacters);
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(folderSelectItem.SelectedPath))
                errors.Add("无效的保存目录");
            return GenericInitializationCheck.GetErrorString("保存设置错误", errors);
        }

        public void Initialize()
        {
            folderSelectItem.defaultPath = $"{EnvPath.Assets}/character/member";
            folderSelectItem.ResetPath();
        }

        public void SelectCharacter()
        {
            CharIDMaskSelect.CharIDMaskSelect charIDMaskSelect
                = WindowController.CurrentWindow.OpenWindow<CharIDMaskSelect.CharIDMaskSelect>(charIDMaskSelectPrefab);
            charIDMaskSelect.Initialize(selectedCharacters, value =>
            {
                selectedCharacters = value;
                characterFilterDisplay.SetMask(selectedCharacters);
            });
        }
    }
}