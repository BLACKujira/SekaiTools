using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Kizuna;
using UnityEngine.UI;
using SekaiTools.UI.KizunaScenePlayer;
using SekaiTools.Live2D;
using System.Windows.Forms;
using SekaiTools.UI.BondsHonorCapturer;
using SekaiTools.UI.BackGround;
using SekaiTools.Exception;

namespace SekaiTools.UI.KizunaSceneEditor
{
    public class KizunaSceneEditor : MonoBehaviour
    {
        public Window window;

        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public KizunaSceneEditor_ModelArea modelArea;
        public KizunaSceneEditor_CutinArea cutinArea;
        public KizunaScenePlayer_BGController bGController;
        public KizunaScenePlayerBase_Player_MainBase player;
        [Header("Prefabs")]
        public Window capturerWindowPrefab;
        [Header("Message")]
        public MonoBehaviour _exceptionPrinter;
        public MessageLayer.MessageLayerBase messageLayer;

        public IExceptionPrinter exceptionPrinter => _exceptionPrinter as IExceptionPrinter;

        [System.NonSerialized] public AudioData audioData;
        [System.NonSerialized] public KizunaSceneDataBase kizunaSceneData;
        [System.NonSerialized] public KizunaSceneBase currentKizunaScene;

        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public void Initialize(KizunaSceneEditorSettings settings)
        {
            audioData = settings.audioData;
            kizunaSceneData = settings.kizunaSceneData;
            modelArea.l2DController.live2DModels = settings.sekaiLive2DModels;
            bGController.Initialize(settings.backGroundParts);

            if (player is KizunaScenePlayerBase_Player_MainEdit)
            {
                KizunaSceneData kizunaSceneData = (KizunaSceneData)this.kizunaSceneData;
                toggleGenerator.Generate(kizunaSceneData.kizunaScenes.Count,
                     (Toggle toggle, int id) =>
                     {
                        BondsHonorSub bondsHonorSub = toggle.GetComponent<BondsHonorSub>();
                        KizunaSceneBase kizunaScene = kizunaSceneData.kizunaScenes[id];
                        bondsHonorSub.SetCharacter(kizunaScene.charAID, kizunaScene.charBID);
                    },
                    (bool value, int id) =>
                    {
                        if (value)
                        {
                            currentKizunaScene = kizunaSceneData.kizunaScenes[id];
                            KizunaScene kizunaScene = kizunaSceneData.kizunaScenes[id];
                            SetData(kizunaScene);
                        }
                    });
                ((KizunaScenePlayerBase_Player_MainEdit)player).Initialize(settings.imageData); 
            }
            else if (player is KizunaScenePlayerBase_Player_MainEditCustom)
            {
                CustomKizunaData kizunaSceneData = (CustomKizunaData)this.kizunaSceneData;
                toggleGenerator.Generate(kizunaSceneData.kizunaScenes.Count,
                     (Toggle toggle, int id) =>
                     {
                         BondsHonorSub bondsHonorSub = toggle.GetComponent<BondsHonorSub>();
                         KizunaSceneBase kizunaScene = kizunaSceneData.kizunaScenes[id];
                         bondsHonorSub.SetCharacter(kizunaScene.charAID, kizunaScene.charBID);
                     },
                    (bool value, int id) =>
                    {
                        if (value)
                        {
                            currentKizunaScene = kizunaSceneData.kizunaScenes[id];
                            KizunaSceneBase kizunaScene = kizunaSceneData.kizunaScenes[id];
                            SetData(kizunaScene);
                        }
                    });
                ((KizunaScenePlayerBase_Player_MainEditCustom)player).Initialize();
            }
        }

        void SetData(KizunaSceneBase kizunaScene)
        {
            modelArea.SetData(kizunaScene);
            cutinArea.SetData(kizunaScene);
            if (player is KizunaScenePlayerBase_Player_MainEdit)
                ((KizunaScenePlayerBase_Player_MainEdit)player).SetScene((KizunaScene)kizunaScene);
            else if (player is KizunaScenePlayerBase_Player_MainEditCustom)
                ((KizunaScenePlayerBase_Player_MainEditCustom)player).SetScene((KizunaSceneCustom)kizunaScene);
            bGController.SetScene(kizunaScene);
            player.Hide(()=>{ player.Show(); });
        }

        public void Refresh()
        {
            SetData(currentKizunaScene);
        }

        public class KizunaSceneEditorSettings
        {
            public ImageData imageData;
            public AudioData audioData;
            public KizunaSceneDataBase kizunaSceneData;
            public SekaiLive2DModel[] sekaiLive2DModels;
            public BackGroundPart[] backGroundParts;
        }

        public void SaveData()
        {
            kizunaSceneData.SaveData();
            messageLayer.ShowMessage("保存成功");
        }

        public void Capture()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string selectedPath = folderBrowserDialog.SelectedPath;
        
            if(capturerWindowPrefab.controlScript is BondsHonorCapturer.BondsHonorCapturer)
            {
                BondsHonorCapturer.BondsHonorCapturer bondsHonorCapturer = window.OpenWindow<BondsHonorCapturer.BondsHonorCapturer>(capturerWindowPrefab);
                bondsHonorCapturer.StartCapture(currentKizunaScene, ((KizunaScenePlayerBase_Player_MainEdit)player).imageData, selectedPath);
            }
            else
            {
                CustomBondsHonorCapturer customBondsHonorCapturer = window.OpenWindow<BondsHonorCapturer.CustomBondsHonorCapturer>(capturerWindowPrefab);
                customBondsHonorCapturer.StartCapture(currentKizunaScene, selectedPath);
            }
        }
    }
}