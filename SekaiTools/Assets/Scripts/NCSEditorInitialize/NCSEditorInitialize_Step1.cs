using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NCSEditorInitialize
{
    public class NCSEditorInitialize_Step1 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_NCSSaveData gIP_NCSSaveData;
        public GIP_NicknameCountData gIP_NicknameCountData;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        [Header("Prefab")]
        public Window step2WindowPrefab;

        public static HashSet<string> RequireMasterTables => NCSEditorInitialize_Step2.RequireMasterTables;

        private void Awake()
        {
            gIP_MasterRefUpdate.SetMasterRefUpdateItems(RequireMasterTables);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_NCSSaveData, gIP_NicknameCountData, gIP_MasterRefUpdate);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            NCSEditorInitialize_Step2.Settings settings = new NCSEditorInitialize_Step2.Settings();
            settings.nicknameCountShowcase = gIP_NCSSaveData.NicknameCountShowcase;
            settings.nicknameCountData = gIP_NicknameCountData.NicknameCountData;

            NCSEditorInitialize_Step2 nCSEditorInitialize_Step2
                = window.OpenWindow<NCSEditorInitialize_Step2>(step2WindowPrefab);
            nCSEditorInitialize_Step2.Initialize(settings);   
        }
    }
}