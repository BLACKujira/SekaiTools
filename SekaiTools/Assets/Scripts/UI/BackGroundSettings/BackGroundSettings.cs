using SekaiTools.UI.BackGround;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.BackGroundSettings
{
    public class BackGroundSettings : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator decorationsButtonGenerator;
        public BackGroundSettings_PartSetting partSetting;
        [Header("Settings")]
        public float deltaBGOffsetX;
        public float deltaBGOffsetY;
        public float deltaBGScale;
        [Header("Prefabs")]
        public Button addDecorationButton;
        public Window decorationSelector;
        public Window nowLoadingWindow;

        BackGroundController backGroundController;
        OpenFileDialog openFileDialog_BGImage;
        OpenFileDialog openFileDialog_SaveData;
        SaveFileDialog saveFileDialog_SaveData;

        public BackGroundPartSet decorationSet => BackGroundController.decorationSet;
        public BackGroundPrefabSet bGPrefabSet => backGroundController.bGPrefabSet;

        public BackGroundController BackGroundController
        {
            get
            {
                if (!backGroundController) backGroundController = BackGroundController.backGroundController;
                return backGroundController;
            }
        }

        private void Awake()
        {
            openFileDialog_BGImage = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_IMAGE);
            openFileDialog_SaveData = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_BGS);
            saveFileDialog_SaveData = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_BGS);

            InitializeButtonGenerator();
        }

        private void InitializeButtonGenerator()
        {
            decorationsButtonGenerator.ClearItems();
            decorationsButtonGenerator.Generate(BackGroundController.Decorations.Count,
                (GameObject gameObject, int id) =>
                {
                    BackGroundSettings_PartItem item = gameObject.GetComponent<BackGroundSettings_PartItem>();
                    item.Label = BackGroundController.Decorations[id].itemName;
                    item.Icon = BackGroundController.Decorations[id].preview;

                    if (BackGroundController.Decorations[id].disableRemove)
                    {
                        item.buttonRemove.interactable = false;
                    }
                    else
                    {
                        item.buttonRemove.onClick.AddListener(() =>
                        {
                            BackGroundController.RemoveDecoration(id);
                            InitializeButtonGenerator();
                            BackGroundController.SetSortingLayers();
                        });
                    }

                    item.buttonConfig.onClick.AddListener(() =>
                    {
                        partSetting.SetPart(BackGroundController.Decorations[id]);
                    });

                    if(id!=0)
                    {
                        item.buttonMoveLeft.onClick.AddListener(() =>
                        {
                            BackGroundPart backGroundPart = BackGroundController.Decorations[id];
                            BackGroundController.Decorations[id] = BackGroundController.Decorations[id - 1];
                            BackGroundController.Decorations[id - 1] = backGroundPart;
                            InitializeButtonGenerator();
                            BackGroundController.SetSortingLayers();
                        });
                    }
                    else
                    {
                        item.buttonMoveLeft.interactable = false;
                    }

                    if (id != BackGroundController.Decorations.Count-1)
                    {
                        item.buttonMoveRight.onClick.AddListener(() =>
                        {
                            BackGroundPart backGroundPart = BackGroundController.Decorations[id];
                            BackGroundController.Decorations[id] = BackGroundController.Decorations[id + 1];
                            BackGroundController.Decorations[id + 1] = backGroundPart;
                            InitializeButtonGenerator();
                            BackGroundController.SetSortingLayers();
                        });
                    }
                    else
                    {
                        item.buttonMoveRight.interactable = false;
                    }

                    if (BackGroundController.Decorations[id].isPartOfBGPrefab)
                        item.ChangeStyle(BackGroundSettings_PartItem.Style.BGPart);
                });
            decorationsButtonGenerator.AddItem(addDecorationButton.gameObject,(GameObject gameObject)=>
            {
                gameObject.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(decorationSelector);
                    universalSelector.Title = "添加背景装饰";
                    universalSelector.Generate(decorationSet.backGroundParts.Count, (Button button, int id) =>
                     {
                         ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                         buttonWithIconAndText.Label = decorationSet.backGroundParts[id].itemName;
                         buttonWithIconAndText.Icon = decorationSet.backGroundParts[id].preview;
                     },
                    (int id) =>
                    {
                        backGroundController.AddDecoration(decorationSet.backGroundParts[id]);
                        InitializeButtonGenerator();
                    });
                });
            });
        }

        public void SelectBGPrefab()
        {
            UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(decorationSelector);
            universalSelector.Title = "选择背景预制件";
            universalSelector.Generate(bGPrefabSet.backGrounds.Count, (Button button, int id) =>
            {
                ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                buttonWithIconAndText.Label = bGPrefabSet.backGrounds[id].mainPart.itemName;
                buttonWithIconAndText.Icon = bGPrefabSet.backGrounds[id].mainPart.preview;
            },
            (int id) =>
            {
                backGroundController.ChangeBackGround(bGPrefabSet.backGrounds[id]);
                InitializeButtonGenerator();
            });
        }

        public void OpenImage()
        {
            DialogResult dialogResult = openFileDialog_BGImage.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            ImageData imageData = new ImageData(openFileDialog_BGImage.FileName);
            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.OnFinish += () => { BackGroundController.backGroundController.ChangeBackGround(imageData.ValueArray[0], openFileDialog_BGImage.FileName); };
            nowLoadingTypeA.StartProcess(imageData.LoadFile(openFileDialog_BGImage.FileName));
        }

        public void ModifyBackGround()
        {
            partSetting.SetPart(BackGroundController.backGroundController.BackGround.mainPart);
        }

        public void Save()
        {
            DialogResult dialogResult = saveFileDialog_SaveData.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = saveFileDialog_SaveData.FileName;
            string json = JsonUtility.ToJson(new BackGroundController.BackGroundSaveData(BackGroundController),true);
            File.WriteAllText(fileName, json);
        }

        public void Load()
        {
            DialogResult dialogResult = openFileDialog_SaveData.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = openFileDialog_SaveData.FileName;
            BackGroundController.BackGroundSaveData saveData = JsonUtility.FromJson<BackGroundController.BackGroundSaveData>(File.ReadAllText(fileName));

            BackGroundController.ClearAndReset();

            string[] log = BackGroundController.Load(saveData);

            //显示Log
            if(log.Length!=0)
            {
                window.ShowLogWindow("出现异常", string.Join("\n", log));
            }

            InitializeButtonGenerator();
        }
    }
}