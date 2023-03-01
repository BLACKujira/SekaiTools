using SekaiTools.Count;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class GIP_NicknameList : MonoBehaviour, IGenericInitializationPart
    {
        [Header("Components")]
        public Button globalSetButton;
        public Button ambiguitySetButton;
        public Button[] characterSetButtons = new Button[27];
        [Header("Prefab")]
        public Window nicknameSettingWindowPrefab;
        public Window ambiguityNicknameSettingWindowPrefab;
        [Header("Inbuilt")]
        public NicknameCounterInitialize_InbuiltData inbuiltData;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        AmbiguityNicknameSet ambiguityNicknameSet;
        NicknameSet nicknameSetGlobal;
        NicknameSet[] nicknameSets = new NicknameSet[27];

        public AmbiguityNicknameSet AmbiguityNicknameSet => ambiguityNicknameSet;
        public NicknameSet NicknameSetGlobal => nicknameSetGlobal;
        public NicknameSet[] NicknameSets => nicknameSets;

        public const string FOLDER_CFG = "NicknameCounter";
        public string ConfigFolder => Path.Combine(EnvPath.Config, FOLDER_CFG);
        public string NicknameSetGlobalPath => Path.Combine(ConfigFolder, "NicknameSetGlobal.json");
        public string AmbiguityNicknameSetPath => Path.Combine(ConfigFolder, "AmbiguityNicknameSet.json");
        public string GetNicknameSetPath(int charId) => Path.Combine(ConfigFolder, $"NicknameSet_{charId:00}.json");

        public void Initialize()
        {
            LoadOrCreateData();
            InitializeButton();
        }

        public void InitializeButton()
        {
            ambiguitySetButton.onClick.AddListener(() =>
            {
                NicknameSetting.AmbiguityNicknameSetting ambiguityNicknameSetting
                    = WindowController.CurrentWindow.OpenWindow<NicknameSetting.AmbiguityNicknameSetting>(ambiguityNicknameSettingWindowPrefab);
                ambiguityNicknameSetting.Initialize(ambiguityNicknameSet, ShowMessage_Save);
            });

            globalSetButton.onClick.AddListener(() =>
            {
                NicknameSetting.NicknameSetting nicknameSetting = WindowController.CurrentWindow.OpenWindow<NicknameSetting.NicknameSetting>(nicknameSettingWindowPrefab);
                nicknameSetting.Initialize(nicknameSetGlobal, ShowMessage_Save);
            });

            for (int i = 1; i < 27; i++)
            {
                int id = i;
                characterSetButtons[i].onClick.AddListener(() =>
                {
                    NicknameSetting.NicknameSetting nicknameSetting = WindowController.CurrentWindow.OpenWindow<NicknameSetting.NicknameSetting>(nicknameSettingWindowPrefab);
                    nicknameSetting.Initialize(nicknameSetGlobal, nicknameSets[id], ShowMessage_Save);
                });
            }
        }

        public void LoadOrCreateData()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(NicknameSetGlobalPath))
                File.WriteAllText(NicknameSetGlobalPath, inbuiltData.nickNameSetGlobalData.text);
            nicknameSetGlobal = NicknameSet.LoadData(NicknameSetGlobalPath);

            if (!File.Exists(AmbiguityNicknameSetPath))
            {
                File.WriteAllText(AmbiguityNicknameSetPath, inbuiltData.ambiguityNickNameSetData.text);
            }
            ambiguityNicknameSet = AmbiguityNicknameSet.LoadData(AmbiguityNicknameSetPath);

            for (int i = 1; i < 27; i++)
            {
                string saveDataFile = GetNicknameSetPath(i);
                if (!File.Exists(saveDataFile))
                    File.WriteAllText(saveDataFile, inbuiltData.nickNameSetData[i].text);
                nicknameSets[i] = NicknameSet.LoadData(saveDataFile);
            }
        }

        private void ShowMessage_Save(bool success)
        {
            if (success)
                messageLayer.ShowMessage("昵称设置已更新");
            else
                WindowController.ShowLog(Message.Error.STR_ERROR, $"保存昵称列表失败");
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            foreach (var regex in ambiguityNicknameSet.ambiguityRegices)
            {
                if (!ExtensionTools.RegexCheck(regex))
                    errors.Add($"模糊昵称 {regex} 不是有效的正则表达式");
            }
            for (int i = 1; i < nicknameSetGlobal.nicknameItems.Length; i++)
            {
                NicknameSet.NicknameItem nicknameItem = nicknameSetGlobal.nicknameItems[i];
                foreach (var regex in nicknameItem.nickNames)
                {
                    if(!ExtensionTools.RegexCheck(regex))
                        errors.Add($"{ConstData.characters[i].Name} 的全局昵称 {regex} 不是有效的正则表达式");
                }
            }
            for (int i = 1; i < 27; i++)
            {
                NicknameSet nicknameSet = nicknameSets[i];
                for (int j = 1; j < 27; j++)
                {
                    NicknameSet.NicknameItem nicknameItem = nicknameSet.nicknameItems[j];
                    foreach (var regex in nicknameItem.nickNames)
                    {
                        if (!ExtensionTools.RegexCheck(regex))
                            errors.Add($"{ConstData.characters[i].Name} 对 {ConstData.characters[j].Name} 的昵称 {regex} 不是有效的正则表达式");
                    }
                }
            }
            return GenericInitializationCheck.GetErrorString("昵称列表错误", errors);
        }
    }
}