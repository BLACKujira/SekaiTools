using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI.NCESelector
{
    public class NCESelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Toggle toggle_Unit;
        public Toggle toggle_Event;
        public Toggle toggle_Card;
        public Toggle toggle_Map;
        public Toggle toggle_Live;
        public Toggle toggle_Other;
        public ButtonGenerator buttonGenerator;
        public Toggle toggleScreening;
        [Header("Prefab")]
        public Window nCESingleWindowPrefab;
        public Window nCEMutiWindowPrefab;

        [NonSerialized] public NicknameCountData countData;

        public enum Mode { single,muti }
        [NonSerialized] public Mode mode;
        [NonSerialized] public int talkerId;
        [NonSerialized] public int nameId;
        [NonSerialized] public StoryType currentStoryTypeTab = StoryType.UnitStory;

        private void Awake()
        {
            window.OnReShow.AddListener(() => Refresh());
        }

        /// <summary>
        /// 初始化函数，进入多角色模式
        /// </summary>
        /// <param name="countData"></param>
        /// <param name="characterId"></param>
        public void Initialize(NicknameCountData countData, int talkerId)
        {
            mode = Mode.muti;
            this.talkerId = talkerId;
            this.countData = countData;
            InitializeTab();
            Refresh();
        }

        /// <summary>
        /// 初始化函数，进入单角色模式
        /// </summary>
        /// <param name="countData"></param>
        /// <param name="nameId"></param>
        public void Initialize(NicknameCountData countData,int talkerId,int nameId)
        {
            mode = Mode.single;
            this.talkerId = talkerId;
            this.nameId = nameId;
            this.countData = countData;
            InitializeTab();
            Refresh();
        }

        public void Generate_Single_NoScreening(NicknameCountMatrix[] nicknameCountMatrices)
        {
            buttonGenerator.ClearButtons();
            List<NicknameCountMatrix> screenedMatrices = new List<NicknameCountMatrix>();

            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                if (nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count > 0)
                    screenedMatrices.Add(nicknameCountMatrix);
            }

            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    Vector2Int[] countCharacter = nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count > 0?
                        new Vector2Int[]{new Vector2Int(nameId, nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count)}
                        :new Vector2Int[0];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        countCharacter
                        );
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    NCEWindow.NCESingle nCESingle = window.OpenWindow<NCEWindow.NCESingle>(nCESingleWindowPrefab);
                    nCESingle.Initialize(nicknameCountMatrix, talkerId, nameId);
                });
        }

        public void Generate_Single_Screening(NicknameCountMatrix[] nicknameCountMatrices)
        {
            buttonGenerator.ClearButtons();
            List<NicknameCountMatrix> screenedMatrices = new List<NicknameCountMatrix>();

            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                if (nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count > 0)
                    screenedMatrices.Add(nicknameCountMatrix);
            }

            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        new Vector2Int(nameId, nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count)
                        );
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    NCEWindow.NCESingle nCESingle = window.OpenWindow<NCEWindow.NCESingle>(nCESingleWindowPrefab);
                    nCESingle.Initialize(nicknameCountMatrix, talkerId, nameId);
                });
        }

        public void Generate_Muti(NicknameCountMatrix[] nicknameCountMatrices)
        {
            buttonGenerator.ClearButtons();
            List<NicknameCountMatrix> screenedMatrices = new List<NicknameCountMatrix>();

            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                if (nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count > 0)
                    screenedMatrices.Add(nicknameCountMatrix);
            }

            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        nicknameCountMatrix.nicknameCountRows[talkerId].nameCountArray
                        );
                },
                (int id) =>
                {

                });
        }

        public void Refresh()
        {
            if(mode == Mode.single)
            {
                if(toggleScreening.isOn)
                {
                    switch (currentStoryTypeTab)
                    {
                        case StoryType.UnitStory:
                            Generate_Single_Screening(countData.countMatrix_Unit.ToArray());
                            break;
                        case StoryType.EventStory:
                            Generate_Single_Screening(countData.countMatrix_Event.ToArray());
                            break;
                        case StoryType.CardStory:
                            Generate_Single_Screening(countData.countMatrix_Card.ToArray());
                            break;
                        case StoryType.MapTalk:
                            Generate_Single_Screening(countData.countMatrix_Map.ToArray());
                            break;
                        case StoryType.LiveTalk:
                            Generate_Single_Screening(countData.countMatrix_Live.ToArray());
                            break;
                        case StoryType.OtherStory:
                            Generate_Single_Screening(countData.countMatrix_Other.ToArray());
                            break;
                    }
                }
                else
                {
                    switch (currentStoryTypeTab)
                    {
                        case StoryType.UnitStory:
                            Generate_Single_NoScreening(countData.countMatrix_Unit.ToArray());
                            break;
                        case StoryType.EventStory:
                            Generate_Single_NoScreening(countData.countMatrix_Event.ToArray());
                            break;
                        case StoryType.CardStory:
                            Generate_Single_NoScreening(countData.countMatrix_Card.ToArray());
                            break;
                        case StoryType.MapTalk:
                            Generate_Single_NoScreening(countData.countMatrix_Map.ToArray());
                            break;
                        case StoryType.LiveTalk:
                            Generate_Single_NoScreening(countData.countMatrix_Live.ToArray());
                            break;
                        case StoryType.OtherStory:
                            Generate_Single_NoScreening(countData.countMatrix_Other.ToArray());
                            break;
                    }
                }
            }
            else
            {
                switch (currentStoryTypeTab)
                {
                    case StoryType.UnitStory:
                        Generate_Muti(countData.countMatrix_Unit.ToArray());
                        break;
                    case StoryType.EventStory:
                        Generate_Muti(countData.countMatrix_Event.ToArray());
                        break;
                    case StoryType.CardStory:
                        Generate_Muti(countData.countMatrix_Card.ToArray());
                        break;
                    case StoryType.MapTalk:
                        Generate_Muti(countData.countMatrix_Map.ToArray());
                        break;
                    case StoryType.LiveTalk:
                        Generate_Muti(countData.countMatrix_Live.ToArray());
                        break;
                    case StoryType.OtherStory:
                        Generate_Muti(countData.countMatrix_Other.ToArray());
                        break;
                }
            }
        }

        private void InitializeTab()
        {
            toggle_Unit.onValueChanged.AddListener((bool value) =>{
                if (value) { currentStoryTypeTab = StoryType.UnitStory;Refresh(); 
            }});
            toggle_Event.onValueChanged.AddListener((bool value) => {
                if (value) { currentStoryTypeTab = StoryType.EventStory; Refresh(); }
            });
            toggle_Card.onValueChanged.AddListener((bool value) => {
                if (value) { currentStoryTypeTab = StoryType.CardStory; Refresh(); }
            });
            toggle_Map.onValueChanged.AddListener((bool value) => {
                if (value) { currentStoryTypeTab = StoryType.MapTalk; Refresh(); }
            });
            toggle_Live.onValueChanged.AddListener((bool value) => {
                if (value) { currentStoryTypeTab = StoryType.LiveTalk; Refresh(); }
            });
            toggle_Other.onValueChanged.AddListener((bool value) => {
                if (value) { currentStoryTypeTab = StoryType.OtherStory; Refresh(); }
            });
            toggle_Unit.isOn = true;
        }
    }
}