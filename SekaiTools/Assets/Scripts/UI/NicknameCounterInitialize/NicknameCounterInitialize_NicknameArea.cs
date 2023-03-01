using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using System.IO;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    [System.Obsolete]
    public class NicknameCounterInitialize_NicknameArea : MonoBehaviour
    {
        public NicknameCounterInitialize_Old nicknameCounterInitialize;
        [Header("Components")]
        public Button globalSetButton;
        public Button[] characterSetButtons = new Button[27];
        [Header("Prefab")]
        public Window nicknameSettingWindowPrefab;

        [System.NonSerialized] public NicknameSet nicknameSetGlobal;
        [System.NonSerialized] public NicknameSet[] nicknameSets = new NicknameSet[27];

        public string saveFolderName => nicknameCounterInitialize.SaveFolderName;
        public string nicknameSetGlobalName { get => "NicknameSetGlobal.json"; }
        public NicknameSetName nicknameSetName { get; } = new NicknameSetName();
        public class NicknameSetName
        {
            public string this[int i] { get => $"NicknameSet_{i.ToString("00")}.json"; }
        }

        private void Awake()
        {
            LoadDataOrCreateData();
            InitializeButton();
        }

        public void LoadDataOrCreateData()
        {
            if (!Directory.Exists(Path.Combine(ConstData.SaveDataPath, saveFolderName)))
                Directory.CreateDirectory(Path.Combine(ConstData.SaveDataPath, saveFolderName));

            string globalSaveDataFile = Path.Combine(ConstData.SaveDataPath, saveFolderName, nicknameSetGlobalName);
            if (!File.Exists(globalSaveDataFile))
                File.WriteAllText(globalSaveDataFile, nicknameCounterInitialize.inbuiltData.nickNameSetGlobalData.text);
            nicknameSetGlobal = NicknameSet.LoadData(globalSaveDataFile);

            for (int i = 1; i < 27; i++)
            {
                string saveDataFile = Path.Combine(ConstData.SaveDataPath, saveFolderName, nicknameSetName[i]);
                if (!File.Exists(saveDataFile))
                    File.WriteAllText(saveDataFile, nicknameCounterInitialize.inbuiltData.nickNameSetData[i].text);
                nicknameSets[i] = NicknameSet.LoadData(saveDataFile);
            }
        }

        public void InitializeButton()
        {
            globalSetButton.onClick.AddListener(() =>
            {
                NicknameSetting.NicknameSetting nicknameSetting = nicknameCounterInitialize.window.OpenWindow<NicknameSetting.NicknameSetting>(nicknameSettingWindowPrefab);
                nicknameSetting.Initialize(nicknameSetGlobal, NicknameSetting_onApply);
                //nicknameSetting.onApply += NicknameSetting_onApply;
            });

            for (int i = 1; i < 27; i++)
            {
                int id = i;
                characterSetButtons[i].onClick.AddListener(()=>
                {
                    NicknameSetting.NicknameSetting nicknameSetting = nicknameCounterInitialize.window.OpenWindow<NicknameSetting.NicknameSetting>(nicknameSettingWindowPrefab);
                    nicknameSetting.Initialize(nicknameSetGlobal,nicknameSets[id], NicknameSetting_onApply);
                    //nicknameSetting.onApply += NicknameSetting_onApply;
                });
            }
        }

        private void NicknameSetting_onApply(bool success)
        {
            nicknameCounterInitialize.messageLayer.ShowMessage("昵称设置已更新");
        }
    }
}