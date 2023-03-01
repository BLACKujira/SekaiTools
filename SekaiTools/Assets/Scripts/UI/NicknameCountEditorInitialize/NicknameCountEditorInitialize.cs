using UnityEngine;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.Count;
using System.Collections.Generic;

namespace SekaiTools.UI.NicknameCountEditorInitialize
{
    public class NicknameCountEditorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_PathSelect gIP_PathSelect;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        [Header("Prefab")]
        public Window editorWindowPrefab;

        public static HashSet<string> RequireMasterTables => new HashSet<string>(StoryDescriptionGetter.RequireMasterTables);

        private void Awake()
        {
            gIP_PathSelect.Initialize();
            gIP_MasterRefUpdate.SetMasterRefUpdateItems(RequireMasterTables);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_PathSelect, gIP_MasterRefUpdate);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            string selectedPath = gIP_PathSelect.pathSelectItems[0].SelectedPath;
            NicknameCountEditor.NicknameCountEditor nicknameCountEditor
                = window.OpenWindow<NicknameCountEditor.NicknameCountEditor>(editorWindowPrefab);
            nicknameCountEditor.Initialize(NicknameCountData.Load(selectedPath));
        }
    }
}
