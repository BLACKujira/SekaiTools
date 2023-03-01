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
using SekaiTools.SekaiViewerInterface;
using SekaiTools.DecompiledClass;

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
        public SaveTipCloseWindowButton saveTipCloseWindowButton;
        [Header("Prefabs")]
        public Window capturerWindowPrefab;
        public Window tableGetterPrefab;
        public Window toolBoxPrefab;
        public Window gswPrefab;
        [Header("Message")]
        public MonoBehaviour _exceptionPrinter;
        public MessageLayer.MessageLayerBase messageLayer;

        public IExceptionPrinter exceptionPrinter => _exceptionPrinter as IExceptionPrinter;

        [System.NonSerialized] public AudioData audioData;
        [System.NonSerialized] public KizunaSceneDataBase kizunaSceneData;
        [System.NonSerialized] public KizunaSceneBase currentKizunaScene;

        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public void Initialize(Settings settings)
        {
            audioData = settings.audioData;
            kizunaSceneData = settings.kizunaSceneData;
            modelArea.l2DController.live2DModels = settings.sekaiLive2DModels;

            saveTipCloseWindowButton.Initialize(() => kizunaSceneData.SavePath);

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

        public class Settings
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

        public void OpenToolBox()
        {
            FunctionSelector.FunctionSelector functionSelector
                = window.OpenWindow<FunctionSelector.FunctionSelector>(toolBoxPrefab);
            functionSelector.Initialize(SetServerTranslate);
        }

        void SetServerTranslate()
        {
            MutiServerTableGetter.MutiServerTableGetter mutiServerTableGetter
                = window.OpenWindow<MutiServerTableGetter.MutiServerTableGetter>(tableGetterPrefab);
            string t_bhw = "bondsHonorWords";
            mutiServerTableGetter.Initialize(ServerRegion.tw, t_bhw, (sr) =>
            {
                WindowController.ShowCancelOK(Message.Error.STR_WARNING_DO, "此操作会覆盖现有的翻译，确定要继续吗？", () =>
                {
                    MasterBondsHonorWord[] bhw_tra;
                    try
                    {
                        bhw_tra = EnvPath.GetTable<MasterBondsHonorWord>(t_bhw, sr);
                    }
                    catch
                    {
                        WindowController.ShowMessage(Message.Error.STR_ERROR, Message.Error.STR_TABLE_CORRUPTION);
                        return;
                    }
                    Dictionary<int, MasterBondsHonorWord> dic_tra = new Dictionary<int, MasterBondsHonorWord>();
                    foreach (var masterBondsHonorWord in bhw_tra)
                    {
                        dic_tra[masterBondsHonorWord.seq] = masterBondsHonorWord;
                    }
                    foreach (var kizunaSceneBase in kizunaSceneData.kizunaSceneBaseArray)
                    {
                        string idStrLv1 = $"1{kizunaSceneBase.charAID:00}{kizunaSceneBase.charBID:00}010";
                        string idStrLv2 = $"1{kizunaSceneBase.charAID:00}{kizunaSceneBase.charBID:00}020";
                        string idStrLv3 = $"1{kizunaSceneBase.charAID:00}{kizunaSceneBase.charBID:00}030";

                        int idLv1 = int.Parse(idStrLv1);
                        int idLv2 = int.Parse(idStrLv2);
                        int idLv3 = int.Parse(idStrLv3);

                        if (kizunaSceneBase is KizunaScene kizunaScene)
                        {
                            if (dic_tra.ContainsKey(idLv1)) kizunaScene.textLv1T = dic_tra[idLv1].name;
                            if (dic_tra.ContainsKey(idLv2)) kizunaScene.textLv2T = dic_tra[idLv2].name;
                            if (dic_tra.ContainsKey(idLv3)) kizunaScene.textLv3T = dic_tra[idLv3].name;
                        }
                        else if(kizunaSceneBase is KizunaSceneCustom kizunaSceneCustom)
                        {
                            if (dic_tra.ContainsKey(idLv1)) kizunaSceneCustom.textLv1T = dic_tra[idLv1].name;
                            if (dic_tra.ContainsKey(idLv2)) kizunaSceneCustom.textLv2T = dic_tra[idLv2].name;
                            if (dic_tra.ContainsKey(idLv3)) kizunaSceneCustom.textLv3T = dic_tra[idLv3].name;
                        }
                    }

                    SetData(currentKizunaScene);
                });
            });
        }

        public void OpenConfWindow()
        {
            GeneralSettingsWindow.GeneralSettingsWindow generalSettingsWindow = window.OpenWindow<GeneralSettingsWindow.GeneralSettingsWindow>(gswPrefab);
            generalSettingsWindow.Initialize(
                new ConfigUIItem[]
                {
                    new ConfigUIItem_CutinScenePlayer
                        ("播放器样式","播放器",
                            ()=>kizunaSceneData.cutinPlayerType,
                            (value) =>
                            {
                                kizunaSceneData.cutinPlayerType = value;
                            })
                });
        }
    }
}