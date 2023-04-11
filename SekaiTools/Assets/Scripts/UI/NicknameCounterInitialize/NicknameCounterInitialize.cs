using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using SekaiTools.DecompiledClass.Core.VirtualLive;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class NicknameCounterInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_NicknameList gIP_NicknameList;
        public GIP_NicknameCounterSample gIP_NicknameCounterSample;
        public GIP_NicknameCounterRule gIP_NicknameCounterRule;
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        public GIP_ExtraFunc gIP_ExtraFunc;
        public GIP_PathSelect gIP_PathSelect;
        [Header("Prefab")]
        public Window counterWindowPrefab;

        public static HashSet<string> RequireMasterTables
        {
            get
            {
                HashSet<string> requireMasterTables = new HashSet<string>()
                {
                    "character3ds",
                };
                requireMasterTables.UnionWith(StoryPublishTimeGetter.RequireMasterTables);
                return requireMasterTables;
            }
        }

        public class Settings
        {
            public NicknameSet nicknameSetGlobal;
            public NicknameSet[] nicknameSetCharacter;
            public FileStruct fileStruct;
            public string folder_Sample;
            public bool passExistingFile;
            public string folder_PassSample;
            public bool passUnusedFile;
        }

        private void Awake()
        {
            gIP_NicknameList.Initialize();
            gIP_NicknameCounterSample.Initialize();
            gIP_MasterRefUpdate.SetMasterRefUpdateItems(RequireMasterTables);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_NicknameList, gIP_NicknameCounterSample, gIP_MasterRefUpdate, gIP_PathSelect);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }

            MasterCharacter3D[] character3ds = EnvPath.GetTable<MasterCharacter3D>("character3Ds");

            List<NicknameCountMatrix> nicknameCountMatrices = GetNicknameCountMatrices(character3ds);
            NicknameCounter.NicknameCounter.Settings settings = new NicknameCounter.NicknameCounter.Settings();
            settings.rawMatrices = nicknameCountMatrices.ToArray();
            settings.ambiguityNicknameSet = gIP_NicknameList.AmbiguityNicknameSet;
            settings.nicknameSetGlobal = gIP_NicknameList.NicknameSetGlobal;
            settings.nicknameSetCharacter = gIP_NicknameList.NicknameSets;
            if(gIP_NicknameCounterRule.UseExcludeStrings) settings.excludeStrings = gIP_NicknameCounterRule.ExcludeStrings;

            NicknameCounter.NicknameCounter nicknameCounter = window.OpenWindow<NicknameCounter.NicknameCounter>(counterWindowPrefab);
            nicknameCounter.Initialize(settings);
        }

        public List<NicknameCountMatrix> GetNicknameCountMatrices(MasterCharacter3D[] character3ds)
        {
            List<NicknameCountMatrix> nicknameCountMatrices = new List<NicknameCountMatrix>();

            string sampleFolder = gIP_NicknameCounterSample.Folder_Sample;
            string saveFolder = gIP_PathSelect.pathSelectItems[0].SelectedPath;

            StoryPublishTimeGetter storyPublishTimeGetter = new StoryPublishTimeGetter();
            if(gIP_ExtraFunc.UseAssetList)
            {
                storyPublishTimeGetter.TurnOnMapTalkPTPresume(gIP_ExtraFunc.BundleRoot);
            }

            foreach (var file in gIP_NicknameCounterSample.UnitStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(storyPublishTimeGetter, file, StoryType.UnitStory, sampleFolder, saveFolder));
            foreach (var file in gIP_NicknameCounterSample.EventStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(storyPublishTimeGetter, file, StoryType.EventStory, sampleFolder, saveFolder));
            foreach (var file in gIP_NicknameCounterSample.CardStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(storyPublishTimeGetter, file, StoryType.CardStory, sampleFolder, saveFolder));
            foreach (var file in gIP_NicknameCounterSample.MapTalkFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(storyPublishTimeGetter, file, StoryType.MapTalk, sampleFolder, saveFolder));
            foreach (var file in gIP_NicknameCounterSample.LiveTalkFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Ceremony(storyPublishTimeGetter, file, StoryType.LiveTalk, sampleFolder, saveFolder, character3ds));
            foreach (var file in gIP_NicknameCounterSample.OtherStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(storyPublishTimeGetter, file, StoryType.OtherStory, sampleFolder, saveFolder));
            return nicknameCountMatrices;
        }

        private static NicknameCountMatrix_Scenario GetCountMatrix_Scenario(StoryPublishTimeGetter storyPublishTimeGetter, string file, StoryType storyType, string sampleFolder, string saveFolder)
        {
            ScenarioSceneData scenarioSceneData = JsonUtility.FromJson<ScenarioSceneData>(File.ReadAllText(file));
            long publishedAt = storyPublishTimeGetter.GetStoryPublishTime(storyType, Path.GetFileNameWithoutExtension(file));
            NicknameCountMatrix_Scenario countMatrix = new NicknameCountMatrix_Scenario(storyType, publishedAt, scenarioSceneData);
            countMatrix.fileName = Path.GetFileNameWithoutExtension(file);
            countMatrix.storyType = storyType;
            string savePath = ExtensionTools.ChangeFolder(sampleFolder, saveFolder, Path.ChangeExtension(file, ".ncmsce"));
            countMatrix.SavePath = savePath;
            return countMatrix;
        }

        private static NicknameCountMatrix_Ceremony GetCountMatrix_Ceremony(StoryPublishTimeGetter storyPublishTimeGetter, string file, StoryType storyType, string sampleFolder, string saveFolder, MasterCharacter3D[] character3ds)
        {
            MasterOfCeremonyData masterOfCeremonyData = JsonUtility.FromJson<MasterOfCeremonyData>(File.ReadAllText(file));
            long publishedAt = storyPublishTimeGetter.GetStoryPublishTime(storyType, Path.GetFileNameWithoutExtension(file));
            NicknameCountMatrix_Ceremony countMatrix = new NicknameCountMatrix_Ceremony(storyType, publishedAt, masterOfCeremonyData, character3ds);
            countMatrix.fileName = Path.GetFileNameWithoutExtension(file);
            countMatrix.storyType = storyType;
            string savePath = ExtensionTools.ChangeFolder(sampleFolder, saveFolder, Path.ChangeExtension(file, ".ncmcer"));
            countMatrix.SavePath = savePath;
            return countMatrix;
        }
    }
}