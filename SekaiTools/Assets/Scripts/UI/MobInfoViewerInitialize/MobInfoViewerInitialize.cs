using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCounterInitialize;
using SekaiTools.UI.StorySelector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.MobInfoViewerInitialize
{
    public class MobInfoViewerInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_NicknameCounterSample gIP_NicknameCounterSample;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        [Header("Prefab")]
        public Window mobInfoViewerPrefab;

        private void Awake()
        {
            gIP_NicknameCounterSample.Initialize();
            HashSet<string> requireMasterTables = new HashSet<string>();
            requireMasterTables.UnionWith(MobInfoCounter.RequireMasterTables);
            requireMasterTables.UnionWith(StorySelector.StorySelector.RequireMasterTables);
            gIP_MasterRefUpdate.SetMasterRefUpdateItems(requireMasterTables);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_NicknameCounterSample, gIP_MasterRefUpdate);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            MobInfoCounter.Settings settings = new MobInfoCounter.Settings();
            settings.UnitStoryFiles = gIP_NicknameCounterSample.UnitStoryFiles;
            settings.EventStoryFiles = gIP_NicknameCounterSample.EventStoryFiles;
            settings.CardStoryFiles = gIP_NicknameCounterSample.CardStoryFiles;
            settings.OtherStoryFiles = gIP_NicknameCounterSample.OtherStoryFiles;

            MobInfoCounter mobInfoCounter = new MobInfoCounter();
            mobInfoCounter.Initialize(settings);

            MobInfoViewer mobInfoViewer = window.OpenWindow<MobInfoViewer>(mobInfoViewerPrefab);
            mobInfoViewer.Initialize(mobInfoCounter);
        }
    }
}