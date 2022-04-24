using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Kizuna;
using System;
using UnityEngine.UI;
using SekaiTools.Cutin;
using System.IO;
using System.Windows.Forms;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class KizunaSceneCreate : MonoBehaviour
    {

        public enum Mode {Normal, Custom }
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public KizunaSceneCreate_Audio audioArea;

        [Header("Prefab")]
        public Window filterGeneratorPrefab;
        public Window nowLoadingWindowPrefab;

        [Header("Settings")]
        public Mode mode = Mode.Normal;

        Action<KizunaSceneDataBase , AudioData> onApply;
        SaveFileDialog saveFileDialog;

        [HideInInspector] public Vector2Int[] bonds = new Vector2Int[0];

        private void Awake()
        {
            saveFileDialog = FileDialogFactory.GetSaveFileDialog_KizunaSceneData();
            Refresh();
        }

        public void Initialize(Action<KizunaSceneDataBase,AudioData> onApply)
        {
            this.onApply = onApply;
        }

        public void SetBonds()
        {
            FilterGenerator.FilterGenerator filterGenerator = window.OpenWindow<FilterGenerator.FilterGenerator>(filterGeneratorPrefab);
            filterGenerator.Initialize(bonds, (Vector2Int[] vector2Ints) => { bonds = vector2Ints; Refresh(); });
        }

        void RefreshBonds()
        {
            //统计各组合互动语音的数量
            float[] countArray = new float[bonds.Length];
            List<CutinSceneData.CutinSceneInfo> cutinSceneInfos = new List<CutinSceneData.CutinSceneInfo>();
            if (audioArea.mode == KizunaSceneCreate_Audio.Mode.folder)
            {
                foreach (var str in audioArea.files)
                {
                    CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(Path.GetFileName(str));
                    if (cutinSceneInfo != null) cutinSceneInfos.Add(cutinSceneInfo);
                }
                for (int i = 0; i < bonds.Length; i++)
                {
                    foreach (var cutinSceneInfo in cutinSceneInfos)
                    {
                        if (cutinSceneInfo.IsConversationOf(bonds[i].x, bonds[i].y,true))
                            countArray[i] += .5f;
                    }
                }
            }
            else if(audioArea.mode == KizunaSceneCreate_Audio.Mode.cutinData)
            {
                for (int i = 0; i < bonds.Length; i++)
                {
                    foreach (var cutinScene in audioArea.cutinSceneData.cutinScenes)
                    {
                        if (cutinScene.IsConversationOf(bonds[i].x, bonds[i].y,true))
                            countArray[i] += 1;
                    }
                }
            }

            universalGenerator.ClearItems();
            //生成列表
            universalGenerator.Generate(bonds.Length,
                (GameObject gameObject, int id) =>
                {
                    BondsHonorSub bondsHonorSub = gameObject.GetComponent<BondsHonorSub>();
                    bondsHonorSub.SetCharacter(bonds[id].x, bonds[id].y);
                    Text text = gameObject.GetComponentInChildren<Text>();
                    text.text = countArray[id] + " 互动语音";
                });
        }

        public void Refresh()
        {
            RefreshBonds();
        }

        public void Apply()
        {
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string fileName = saveFileDialog.FileName;

            KizunaSceneDataBase kizunaSceneData;
            if (mode == Mode.Normal)
                kizunaSceneData = new KizunaSceneData(bonds);
            else
                kizunaSceneData = new CustomKizunaData(bonds);

            kizunaSceneData.savePath = fileName;
            AudioData audioData = null;

            if (audioArea.mode != KizunaSceneCreate_Audio.Mode.none)
            {
                string audioFileName = Path.ChangeExtension(fileName, ".aud");

                if (audioArea.mode == KizunaSceneCreate_Audio.Mode.folder)
                {
                    List<string> audioFiles = new List<string>();//挑选需要的音频文件
                    foreach (var vector2Int in bonds)
                    {
                        List<CutinSceneData.CutinSceneInfo> cutinSceneInfos = new List<CutinSceneData.CutinSceneInfo>();
                        foreach (var str in audioArea.files)
                        {
                            CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(Path.GetFileName(str));
                            if (cutinSceneInfo != null)
                                if (cutinSceneInfo.IsConversationOf(vector2Int.x, vector2Int.y,true))
                                { cutinSceneInfos.Add(cutinSceneInfo);audioFiles.Add(str); }
                        }
                        kizunaSceneData[vector2Int].cutinScenes = new CutinSceneData(cutinSceneInfos.ToArray()).cutinScenes;
                    }
                    audioData = new AudioData(audioFileName);
                    NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
                    nowLoadingTypeA.TitleText = "读取音频文件中";
                    nowLoadingTypeA.OnFinish += () => { kizunaSceneData.StandardizeAudioData(audioData);audioData.SaveData(); if (onApply != null) onApply(kizunaSceneData, audioData); };
                    nowLoadingTypeA.StartProcess(audioData.LoadFile(audioFiles.ToArray()));
                }
                else if(audioArea.mode == KizunaSceneCreate_Audio.Mode.cutinData)
                {
                    foreach (var vector2Int in bonds)
                    {
                        KizunaSceneBase kizunaScene = kizunaSceneData[vector2Int];
                        foreach (var cutinScene in audioArea.cutinSceneData.cutinScenes)
                        {
                            if (cutinScene.IsConversationOf(vector2Int.x,vector2Int.y,true))
                                { kizunaScene.cutinScenes.Add(cutinScene);}
                        }
                    }
                    string cutinSceneAudioDataPath = Path.ChangeExtension(audioArea.cutinSceneData.savePath, ".aud");
                    //如果互动语音场景有音频资料则读取资料并标准化
                    if (File.Exists(cutinSceneAudioDataPath))
                    {
                        AudioData.SerializedAudioData serializedAudioData = AudioData.DeSerializeSaveData(cutinSceneAudioDataPath);
                        AudioData.SerializedAudioData standardizedAudioData = kizunaSceneData.StandardizeAudioData(serializedAudioData);
                        audioData = new AudioData(audioFileName);
                        NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
                        nowLoadingTypeA.TitleText = "读取音频文件中";
                        nowLoadingTypeA.OnFinish += () => { kizunaSceneData.StandardizeAudioData(audioData); audioData.SaveData(); if (onApply != null) onApply(kizunaSceneData, audioData); };
                        nowLoadingTypeA.StartProcess(audioData.LoadFile(standardizedAudioData));
                    }
                }
                
            }
            else
            {
                onApply(kizunaSceneData, null);
            }

            kizunaSceneData.SaveData();

            window.Close();
        }
    }
}