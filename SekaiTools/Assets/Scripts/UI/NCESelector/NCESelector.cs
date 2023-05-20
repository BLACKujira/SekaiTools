using SekaiTools.Count;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        public Toggle toggle_CheckCount;
        public NCESelector_CheckCount checkCountArea;
        [Header("Prefab")]
        public Window nCESingleWindowPrefab;
        public Window nCEMutiWindowPrefab;
        public Window nCEFullWindowPrefab;
        public Window nCEAmbiguityWindowPrefab;

        [NonSerialized] public NicknameCountData countData;
        HashSet<NicknameCountMatrix> checkedMatrices = new HashSet<NicknameCountMatrix>();

        public enum Mode { single, muti, full, ambiguity }
        [NonSerialized] public Mode mode;
        [NonSerialized] public int talkerId;
        [NonSerialized] public int nameId;
        [NonSerialized] public string ambiguityRegex;
        [NonSerialized] public StoryType currentStoryTypeTab = StoryType.UnitStory;
        [NonSerialized] StoryDescriptionGetter storyDescriptionGetter;

        private void Awake()
        {
            storyDescriptionGetter = new StoryDescriptionGetter();
            window.OnReShow.AddListener(() => Refresh());
            if (toggle_CheckCount)
            {
                toggle_CheckCount.onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        checkCountArea.Initialize(nameId);
                        checkedMatrices = new HashSet<NicknameCountMatrix>();
                        checkCountArea.SetData(0, countData[talkerId, nameId].Total);
                    }
                });
                toggle_CheckCount.gameObject.SetActive(false);
            }
        }

        void UpdateCheckCount()
        {
            int count = checkedMatrices.Sum((mat) => mat[talkerId, nameId].Times);
            checkCountArea.SetData(count, countData[talkerId, nameId].Total);
        }

        /// <summary>
        /// 初始化函数，进入全部剧情模式
        /// </summary>
        /// <param name="countData"></param>
        /// <param name="characterId"></param>
        public void Initialize(NicknameCountData countData)
        {
            mode = Mode.full;
            this.countData = countData;
            InitializeTab();
            Refresh();
        }

        /// <summary>
        /// 初始化函数，进入模糊昵称模式
        /// </summary>
        /// <param name="countData"></param>
        /// <param name="characterId"></param>
        public void Initialize(NicknameCountData countData, string ambiguityRegex)
        {
            mode = Mode.ambiguity;
            this.countData = countData;
            this.ambiguityRegex = ambiguityRegex;
            InitializeTab();
            Refresh();
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
        public void Initialize(NicknameCountData countData, int talkerId, int nameId)
        {
            mode = Mode.single;
            this.talkerId = talkerId;
            this.nameId = nameId;
            this.countData = countData;
            toggle_CheckCount.gameObject.SetActive(true);
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

            screenedMatrices = new List<NicknameCountMatrix>(NicknameCountMatrix.Sort(screenedMatrices.ToArray()));
            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    Vector2Int[] countCharacter = nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count > 0 ?
                        new Vector2Int[] { new Vector2Int(nameId, nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count) }
                        : new Vector2Int[0];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        nicknameCountMatrix.storyType,
                        storyDescriptionGetter,
                        countCharacter,
                        checkedMatrices.Contains(nicknameCountMatrix));
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];

                    if (toggle_CheckCount.isOn)
                    {
                        checkedMatrices.Add(nicknameCountMatrix);
                        UpdateCheckCount();
                    }

                    NCEWindow.NCESingle nCESingle = window.OpenWindow<NCEWindow.NCESingle>(nCESingleWindowPrefab);
                    nCESingle.Initialize(nicknameCountMatrix, talkerId, nameId, storyDescriptionGetter);
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

            screenedMatrices = new List<NicknameCountMatrix>(NicknameCountMatrix.Sort(screenedMatrices.ToArray()));
            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        nicknameCountMatrix.storyType,
                        storyDescriptionGetter,
                        new Vector2Int[]
                        {
                            new Vector2Int(nameId, nicknameCountMatrix.nicknameCountRows[talkerId].nicknameCountGrids[nameId].matchedIndexes.Count)
                        }
                        , checkedMatrices.Contains(nicknameCountMatrix));
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    if (toggle_CheckCount.isOn)
                    {
                        checkedMatrices.Add(nicknameCountMatrix);
                    }

                    NCEWindow.NCESingle nCESingle = window.OpenWindow<NCEWindow.NCESingle>(nCESingleWindowPrefab);
                    nCESingle.window.OnClose.AddListener(() => UpdateCheckCount());
                    nCESingle.Initialize(nicknameCountMatrix, talkerId, nameId, storyDescriptionGetter);
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

            screenedMatrices = new List<NicknameCountMatrix>(NicknameCountMatrix.Sort(screenedMatrices.ToArray()));
            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        nicknameCountMatrix.storyType,
                        storyDescriptionGetter,
                        nicknameCountMatrix.nicknameCountRows[talkerId].NameCountArray
                        );
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    NCEWindow.NCEMuti nCEMuti = window.OpenWindow<NCEWindow.NCEMuti>(nCEMutiWindowPrefab);
                    nCEMuti.Initialize(nicknameCountMatrix, talkerId, storyDescriptionGetter);
                });
        }

        public void Generate_Full(NicknameCountMatrix[] nicknameCountMatrices)
        {
            buttonGenerator.ClearButtons();
            List<NicknameCountMatrix> screenedMatrices = new List<NicknameCountMatrix>();
            screenedMatrices = new List<NicknameCountMatrix>(NicknameCountMatrix.Sort(nicknameCountMatrices.ToArray()));
            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    int countAll = nicknameCountMatrix.nicknameCountRows
                                            .Where(ncr => ncr != null)
                                            .SelectMany(ncr => ncr.NameCountArray)
                                            .Sum(v2i => v2i.y);
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        countAll,
                        nicknameCountMatrix.storyType,
                        storyDescriptionGetter,
                        new Vector2Int[0]
                        );
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    NCEWindow.NCEMuti nCEMuti = window.OpenWindow<NCEWindow.NCEMuti>(nCEMutiWindowPrefab);
                    nCEMuti.Initialize(nicknameCountMatrix, storyDescriptionGetter);
                });
        }

        public void Generate_Ambiguity(NicknameCountMatrix[] nicknameCountMatrices)
        {
            buttonGenerator.ClearButtons();
            List<NicknameCountMatrix> screenedMatrices = new List<NicknameCountMatrix>();

            foreach (var nicknameCountMatrix in nicknameCountMatrices)
            {
                if (nicknameCountMatrix.GetAmbiguitySerifSet(ambiguityRegex)?.matchedIndexes.Count > 0)
                    screenedMatrices.Add(nicknameCountMatrix);
            }

            screenedMatrices = new List<NicknameCountMatrix>(NicknameCountMatrix.Sort(screenedMatrices.ToArray()));
            buttonGenerator.Generate(screenedMatrices.Count,
                (Button button, int id) =>
                {
                    NCESelector_Item nCESelector_Item = button.GetComponent<NCESelector_Item>();
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    nCESelector_Item.Initialize(
                        nicknameCountMatrix.fileName,
                        nicknameCountMatrix.nicknameCountRows[talkerId].serifCount.Count,
                        nicknameCountMatrix.storyType,
                        storyDescriptionGetter,
                        new Vector2Int[]
                        {
                            new Vector2Int(21, nicknameCountMatrix.GetAmbiguitySerifSet(ambiguityRegex).matchedIndexes.Count)
                        }
                        );
                },
                (int id) =>
                {
                    NicknameCountMatrix nicknameCountMatrix = screenedMatrices[id];
                    NCEWindow.NCEMuti nCEMuti = window.OpenWindow<NCEWindow.NCEMuti>(nCEAmbiguityWindowPrefab);
                    nCEMuti.Initialize(nicknameCountMatrix, ambiguityRegex, storyDescriptionGetter);
                });
        }

        public void Refresh()
        {
            if (mode == Mode.single)
            {
                if (toggleScreening.isOn)
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
            else if (mode == Mode.muti)
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
            else if (mode == Mode.ambiguity)
            {
                switch (currentStoryTypeTab)
                {
                    case StoryType.UnitStory:
                        Generate_Ambiguity(countData.countMatrix_Unit.ToArray());
                        break;
                    case StoryType.EventStory:
                        Generate_Ambiguity(countData.countMatrix_Event.ToArray());
                        break;
                    case StoryType.CardStory:
                        Generate_Ambiguity(countData.countMatrix_Card.ToArray());
                        break;
                    case StoryType.MapTalk:
                        Generate_Ambiguity(countData.countMatrix_Map.ToArray());
                        break;
                    case StoryType.LiveTalk:
                        Generate_Ambiguity(countData.countMatrix_Live.ToArray());
                        break;
                    case StoryType.OtherStory:
                        Generate_Ambiguity(countData.countMatrix_Other.ToArray());
                        break;
                }
            }
            else
            {
                switch (currentStoryTypeTab)
                {
                    case StoryType.UnitStory:
                        Generate_Full(countData.countMatrix_Unit.ToArray());
                        break;
                    case StoryType.EventStory:
                        Generate_Full(countData.countMatrix_Event.ToArray());
                        break;
                    case StoryType.CardStory:
                        Generate_Full(countData.countMatrix_Card.ToArray());
                        break;
                    case StoryType.MapTalk:
                        Generate_Full(countData.countMatrix_Map.ToArray());
                        break;
                    case StoryType.LiveTalk:
                        Generate_Full(countData.countMatrix_Live.ToArray());
                        break;
                    case StoryType.OtherStory:
                        Generate_Full(countData.countMatrix_Other.ToArray());
                        break;
                }
            }
        }

        private void InitializeTab()
        {
            toggle_Unit.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    currentStoryTypeTab = StoryType.UnitStory; Refresh();
                }
            });
            toggle_Event.onValueChanged.AddListener((bool value) =>
            {
                if (value) { currentStoryTypeTab = StoryType.EventStory; Refresh(); }
            });
            toggle_Card.onValueChanged.AddListener((bool value) =>
            {
                if (value) { currentStoryTypeTab = StoryType.CardStory; Refresh(); }
            });
            toggle_Map.onValueChanged.AddListener((bool value) =>
            {
                if (value) { currentStoryTypeTab = StoryType.MapTalk; Refresh(); }
            });
            toggle_Live.onValueChanged.AddListener((bool value) =>
            {
                if (value) { currentStoryTypeTab = StoryType.LiveTalk; Refresh(); }
            });
            toggle_Other.onValueChanged.AddListener((bool value) =>
            {
                if (value) { currentStoryTypeTab = StoryType.OtherStory; Refresh(); }
            });
            toggle_Unit.isOn = true;
        }
    }
}