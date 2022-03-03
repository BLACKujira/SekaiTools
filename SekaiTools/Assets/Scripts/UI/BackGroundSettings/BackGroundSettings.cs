using System.Collections;
using System.Collections.Generic;
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
        public ButtonGenerator decorationsButtonGenerator;
        public ValueEditButton xValueEditButton;
        public ValueEditButton yValueEditButton;
        public ValueEditButton scaleValueEditButton;
        [Header("Settings")]
        public float deltaBGOffsetX;
        public float deltaBGOffsetY;
        public float deltaBGScale;
        public BackGroundPartSet decorationSet;
        public BackGroundPartSet bGPrefabSet;
        [Header("Prefabs")]
        public Button addDecorationButton;
        public Window decorationSelector;
        public Window nowLoadingWindow;

        BackGroundController backGroundController;
        OpenFileDialog fileDialog = new OpenFileDialog();


        float bGOffsetX 
        {
            get => BackGroundController.BackGround.transform.position.x;
            set
            {
                BackGroundController.BackGround.transform.position = new Vector2(value, bGOffsetY);
            }
        }
        float bGOffsetY
        {
            get => BackGroundController.BackGround.transform.position.y;
            set
            {
                BackGroundController.BackGround.transform.position = new Vector2(bGOffsetX, value);
            }
        }
        float bGScale
        {
            get => BackGroundController.BackGround.transform.localScale.x;
            set
            {
                BackGroundController.BackGround.transform.localScale = new Vector3(value, value, 1);
            }
        }

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
            fileDialog.Title = "选择图片";
            fileDialog.Filter = "Image |*.png;*.jpg|Others (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            InitializeValueEditButton();
            InitializeButtonGenerator();
        }

        private void InitializeValueEditButton()
        {
            xValueEditButton.Initialize(() =>
            {
                bGOffsetX -= deltaBGOffsetX;
            },
            () =>
            {
                bGOffsetX += deltaBGOffsetX;
            },
            () =>
            {
                bGOffsetX = 0;
            },
            () =>
            {
                return bGOffsetX.ToString("0.00");
            }
            );

            yValueEditButton.Initialize(() =>
            {
                bGOffsetY -= deltaBGOffsetY;
            },
            () =>
            {
                bGOffsetY += deltaBGOffsetY;
            },
            () =>
            {
                bGOffsetY = 0;
            },
            () =>
            {
                return bGOffsetY.ToString("0.00");
            }
            );

            scaleValueEditButton.Initialize(() =>
            {
                bGScale -= deltaBGScale;
            },
            () =>
            {
                bGScale += deltaBGScale;
            },
            () =>
            {
                bGScale = 1;
            },
            () =>
            {
                return bGScale.ToString("0.00");
            }
            );
        }

        private void InitializeButtonGenerator()
        {
            decorationsButtonGenerator.ClearButtons();
            decorationsButtonGenerator.Generate(BackGroundController.Decorations.Count,
                (Button button,int id) =>
                {
                    ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                    buttonWithIconAndText.Label = BackGroundController.Decorations[id].name;
                    buttonWithIconAndText.Icon = BackGroundController.Decorations[id].preview;
                },
                (int id)=>
                {
                    BackGroundController.RemoveDecoration(id);
                    InitializeButtonGenerator();
                });
            decorationsButtonGenerator.AddButton(addDecorationButton,null,()=>
            {
                UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(decorationSelector);
                universalSelector.Title = "添加背景装饰";
                universalSelector.Generate(decorationSet.backGroundParts.Count,(Button button,int id)=>
                {
                    ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                    buttonWithIconAndText.Label = decorationSet.backGroundParts[id].name;
                    buttonWithIconAndText.Icon = decorationSet.backGroundParts[id].preview;
                },
                (int id)=>
                {
                    backGroundController.AddDecoration(decorationSet.backGroundParts[id]);
                    InitializeButtonGenerator();
                });
            });
        }

        public void ResetBGTransform()
        {
            bGOffsetX = 0;
            bGOffsetY = 0;
            bGScale = 1;
            xValueEditButton.LoadValue();
            yValueEditButton.LoadValue();
            scaleValueEditButton.LoadValue();
        }

        public void SelectBGPrefab()
        {
            UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(decorationSelector);
            universalSelector.Title = "选择背景预制件";
            universalSelector.Generate(bGPrefabSet.backGroundParts.Count, (Button button, int id) =>
            {
                ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                buttonWithIconAndText.Label = bGPrefabSet.backGroundParts[id].name;
                buttonWithIconAndText.Icon = bGPrefabSet.backGroundParts[id].preview;
            },
            (int id) =>
            {
                backGroundController.ChangeBackGround(bGPrefabSet.backGroundParts[id]);
                InitializeButtonGenerator();
                ResetBGTransform();
            });
        }

        public void OpenImage()
        {
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            ImageData imageData = new ImageData();
            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.OnFinish += () => { BackGroundController.backGroundController.ChangeBackGround(imageData.spriteList[0], fileDialog.FileName);ResetBGTransform(); };
            nowLoadingTypeA.StartProcess(imageData.LoadImage(fileDialog.FileName));
        }
    }
}