using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using SekaiTools.Count;
using SekaiTools.DecompiledClass;
using System.IO;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class NicknameCounterInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NicknameCounterInitialize_NicknameArea nicknameArea;
        public NicknameCounterInitialize_SampleArea sampleArea;
        public NicknameCounterInitialize_OtherArea otherArea;
        [Header("Settings")]
        public NicknameCounterInitialize_InbuiltData inbuiltData;
        [Header("Prefab")]
        public Window counterWindowPrefab;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        public string SaveFolderName => "NicknameCounterInitialize";

        public void Apply()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择保存统计文件的目录";
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string saveFolder = folderBrowserDialog.SelectedPath;
            List<NicknameCountMatrix> nicknameCountMatrices = new List<NicknameCountMatrix>();

            foreach (var file in sampleArea.unitStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(file, StoryType.UnitStory,Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.unitStoriesFolder)));
            foreach (var file in sampleArea.eventStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(file, StoryType.EventStory, Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.eventStoriesFolder)));
            foreach (var file in sampleArea.cardStoryFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(file, StoryType.CardStory, Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.cardStoriesFolder)));
            foreach (var file in sampleArea.mapTalkFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(file, StoryType.MapTalk, Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.mapTalkFolder)));
            foreach (var file in sampleArea.liveTalkFiles)
            {
                NicknameCountMatrix_Ceremony countMatrix = new NicknameCountMatrix_Ceremony();
                countMatrix.fileName = Path.GetFileNameWithoutExtension(file);
                countMatrix.storyType = StoryType.LiveTalk;
                countMatrix.masterOfCeremonyData = JsonUtility.FromJson<DecompiledClass.Core.VirtualLive.MasterOfCeremonyData>(File.ReadAllText(file));
                countMatrix.SavePath = Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.liveTalkFolder, countMatrix.fileName + ".ncm");
                nicknameCountMatrices.Add(countMatrix);
            }
            foreach (var file in sampleArea.otherStoriesFiles)
                nicknameCountMatrices.Add(GetCountMatrix_Scenario(file, StoryType.OtherStory, Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.otherStoriesFolder)));

            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.unitStoriesFolder));
            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.eventStoriesFolder));
            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.cardStoriesFolder));
            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.mapTalkFolder));
            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.liveTalkFolder));
            Directory.CreateDirectory(Path.Combine(saveFolder, NicknameCounterInitialize_SampleArea.otherStoriesFolder));

            NicknameCounter.NicknameCounter.NicknameCounterSettings settings = new NicknameCounter.NicknameCounter.NicknameCounterSettings();
            settings.rawMatrices = nicknameCountMatrices.ToArray();
            settings.nicknameSetGlobal = nicknameArea.nicknameSetGlobal;
            settings.nicknameSetCharacter = nicknameArea.nicknameSets;

            NicknameCounter.NicknameCounter nicknameCounter = window.OpenWindow<NicknameCounter.NicknameCounter>(counterWindowPrefab);
            nicknameCounter.Initialize(settings);
        }

        private static NicknameCountMatrix_Scenario GetCountMatrix_Scenario(string file, StoryType storyType,string saveFolder)
        {
            NicknameCountMatrix_Scenario countMatrix = new NicknameCountMatrix_Scenario();
            countMatrix.fileName = Path.GetFileNameWithoutExtension(file);
            countMatrix.storyType = storyType;
            countMatrix.scenarioSceneData = JsonUtility.FromJson<ScenarioSceneData>(File.ReadAllText(file));
            countMatrix.SavePath = Path.Combine(saveFolder, countMatrix.fileName + ".ncm");
            return countMatrix;
        }
    }
}