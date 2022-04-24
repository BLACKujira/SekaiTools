using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Live2D;
using System.IO;

namespace SekaiTools.UI.L2DAniPreviewGenerator
{
    public class L2DAniPreviewGenerator : MonoBehaviour
    {
        public enum Mode {Motion,Facial }

        public InbuiltAnimationSet inbuiltAnimationSet;
        [Header("Components")]
        public ToggleGenerator groupModel;
        public ToggleGenerator groupAnimation;
        public L2DControllerTypeC l2DController;
        public ValueEditButton buttonPositionX;
        public ValueEditButton buttonPositionY;
        public ValueEditButton buttonScale;
        public Toggle toggleMotionMode;
        public Toggle toggleFacialMode;
        public InputField savePathInputField;
        public L2DAniPreviewGenerator_Process process;
        public GameObject LayerSetting;
        public GameObject LayerProcess;
        public Button applyButton;
        [Header("Settings")]
        public IconSet toggleIconSet;
        [Header("PositionAndScale_Motion")]
        public Vector2 offsetPosition_Motion;
        public float positionDeltaX_Motion;
        public float positionDeltaY_Motion;
        public float offsetScale_Motion;
        public float scaleDelta_Motion;
        [Header("PositionAndScale_Facial")]
        public Vector2 offsetPosition_Facial;
        public float positionDeltaX_Facial;
        public float positionDeltaY_Facial;
        public float offsetScale_Facial;
        public float scaleDelta_Facial;

        public L2DAnimationSet animationSet { get; private set; }

        public Mode mode { get;private set; } = Mode.Motion;

        public Vector2 offsetPosition
        {
            get => mode == Mode.Motion ? offsetPosition_Motion : offsetPosition_Facial;
        }
        public float positionDeltaX
        {
            get => mode == Mode.Motion? positionDeltaX_Motion : positionDeltaX_Facial;
        }
        public float positionDeltaY
        {
            get => mode == Mode.Motion ? positionDeltaY_Motion : positionDeltaY_Facial;
        }
        public float offsetScale
        {
            get => mode == Mode.Motion ? offsetScale_Motion : offsetScale_Facial;
        }
        public float scaleDelta
        {
            get => mode == Mode.Motion ? scaleDelta_Motion : scaleDelta_Facial;
        }

        [HideInInspector] public List<L2DAnimationSet> animationSets;
        private ModelLoader modelLoader;

        Vector2 modelPosition_Motion;
        float modelScale_Motion = 1;
        Vector2 modelPosition_Facial;
        float modelScale_Facial = 1;

        public Vector2 modelPosition
        {
            get => mode == Mode.Motion ? modelPosition_Motion : modelPosition_Facial;
            set { if (mode == Mode.Motion) modelPosition_Motion = value; else modelPosition_Facial = value; }
        }
        public float modelScale
        {
            get => mode == Mode.Motion? modelScale_Motion : modelScale_Facial;
            set { if (mode == Mode.Motion) modelScale_Motion = value; else modelScale_Facial = value; }
        }

        public ModelLoader ModelLoader { get { if (!modelLoader) modelLoader = ModelLoader.modelLoader; return modelLoader; }}

        private void Awake()
        {
            animationSets = new List<L2DAnimationSet>(inbuiltAnimationSet.l2DAnimationSets);
            Initialize();
            SetToggleGroupModel();
            SetToggleGroupAnimation();
        }

        void SetToggleGroupModel()
        {
            List<SekaiLive2DModel> modelList = ModelLoader.models;
            groupModel.Generate(modelList.Count,
                (Toggle toggle, int id) => {
                    ToggleWithIconAndColor toggleWithIconAndColor = toggle.GetComponent<ToggleWithIconAndColor>();
                    toggleWithIconAndColor.SetIcon(toggleIconSet.icons[ConstData.IsLive2DModelOfCharacter(modelList[id].name)]);
                    toggleWithIconAndColor.SetLabel(modelList[id].name);
                },
                (bool value,int id)=>{
                    if(value)
                    {
                        ResetPositionAndScale();
                        l2DController.ShowModel(ModelLoader.models[id]);
                        if (!toggleMotionMode.isOn) toggleMotionMode.isOn = true;
                        Refresh();
                    }
                }
            );
        }
        void SetToggleGroupAnimation()
        {
            groupAnimation.Generate(animationSets.Count,
                (Toggle toggle, int id) => {
                    ToggleWithIconAndColor toggleWithIconAndColor = toggle.GetComponent<ToggleWithIconAndColor>();
                    if (id >= 0 && id < 27)
                        toggleWithIconAndColor.SetIcon(toggleIconSet.icons[id+1]);
                    else
                        toggleWithIconAndColor.SetIcon(toggleIconSet.icons[0]);
                    toggleWithIconAndColor.SetLabel(animationSets[id].name);
                },
                (bool value, int id) => {
                    if (value)
                        animationSet = animationSets[id];
                }
            );
        }

        void SetModelPosition()
        {
            l2DController.SetModelPosition(offsetPosition + modelPosition);
        }
        void SetModelScale()
        {
            l2DController.SetModelScale(offsetScale * modelScale);
        }
    
        void Initialize()
        {

            buttonPositionX.Initialize(
                () =>
                {
                    modelPosition -= new Vector2(positionDeltaX,0);
                    SetModelPosition();
                },
                () =>
                {
                    modelPosition += new Vector2(positionDeltaX, 0);
                    SetModelPosition();
                },
                () =>
                {
                    modelPosition = new Vector2(0, modelPosition.y);
                    SetModelPosition();
                },
                () =>
                {
                    return modelPosition.x.ToString("0.00");
                });

            buttonPositionY.Initialize(
                () =>
                {
                    modelPosition -= new Vector2(0, positionDeltaY);
                    SetModelPosition();
                },
                () =>
                {
                    modelPosition += new Vector2(0, positionDeltaY);
                    SetModelPosition();
                },
                () =>
                {
                    modelPosition = new Vector2(modelPosition.y, 0);
                    SetModelPosition();
                },
                () =>
                {
                    return modelPosition.y.ToString("0.00");
                });

            buttonScale.Initialize(
                () =>
                {
                    modelScale -= scaleDelta;
                    SetModelScale();
                },
                () =>
                {
                    modelScale += scaleDelta;
                    SetModelScale();
                },
                () =>
                {
                    modelScale = 1;
                    SetModelScale();
                },
                () =>
                {
                    return modelScale.ToString("0.00");
                });

            toggleMotionMode.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                    ChangeModeToMotion();
            });
            toggleFacialMode.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                    ChangeModeToFacial();
            });

            CheckIfReady();
        }

        void ResetPositionAndScale()
        {
            modelPosition_Motion = Vector2.zero;
            modelPosition_Facial = Vector2.zero;
            modelScale_Motion = 1;
            modelScale_Facial = 1;
        }

        public void ChangeModeToMotion()
        {
            mode = Mode.Motion;
            Refresh();
        }
        public void ChangeModeToFacial()
        {
            mode = Mode.Facial;
            Refresh();
        }

        /// <summary>
        ///刷新显示 
        /// </summary>
        public void Refresh()
        {
            SetModelPosition();
            SetModelScale();
            buttonPositionX.LoadValue();
            buttonPositionY.LoadValue();
            buttonScale.LoadValue();
        }

        public void Apply()
        {
            LayerSetting.SetActive(false);
            LayerProcess.SetActive(true);
            process.StartProcess();
        }

        public void CancelProcess()
        {
            process.StopProcess();
            LayerSetting.SetActive(true);
            LayerProcess.SetActive(false);
        }

        public void CheckIfReady()
        {
            if (Directory.Exists(savePathInputField.text)) applyButton.interactable = true;
            else applyButton.interactable = false;
        }
    }
}