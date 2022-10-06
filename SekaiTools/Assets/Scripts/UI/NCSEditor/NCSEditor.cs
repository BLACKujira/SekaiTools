using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.NicknameCountShowcase;
using SekaiTools.Count.Showcase;
using UnityEngine.UI;
using SekaiTools.Count;
using System.IO;

namespace SekaiTools.UI.NCSEditor
{
    public class NCSEditor : NCSPlayerBase
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        [Header("Setting")]
        [Header("Prefab")]
        public Window itemSelectorWindow;
        public GameObject addItemButton;

        public NCSSceneSet nCSSceneSet => GlobalData.globalData.nCSSceneSet;

        private void Awake()
        {
            countData = NicknameCountData.Load(@"D:\BackUp\220528\0");
            audioData = new AudioData(@"D:\BackUp\220528\0\save.aud");
            NowLoadingTypeA nowLoadingTypeA = WindowController.windowController.currentWindow.OpenWindow<NowLoadingTypeA>(WindowController.windowController.nowLoadingTypeAWindow);
            nowLoadingTypeA.OnFinish += () => 
            {
                showcase = Count.Showcase.NicknameCountShowcase.LoadData(@"D:\BackUp\220528\0\save.ncs");
                Initialize(showcase); 
            };
            nowLoadingTypeA.StartProcess(audioData.LoadData(File.ReadAllText(audioData.savePath)));
        }

        public void Initialize(Count.Showcase.NicknameCountShowcase showcase)
        {
            this.showcase = showcase;
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();
            universalGenerator.Generate(showcase.scenes.Count,
                (gameObject, id) =>
                {
                    NCSEditor_Item item = gameObject.GetComponent<NCSEditor_Item>();
                    item.Initialize(showcase.scenes[id], this);
                });
            universalGenerator.AddItem(addItemButton, (gameObject) =>
             {
                 Button button = gameObject.GetComponent<Button>();
                 button.onClick.AddListener(() =>
                 {
                     UniversalSelector universalSelector = window.OpenWindow<UniversalSelector>(itemSelectorWindow);
                     universalSelector.Generate(nCSSceneSet.nCSScenes.Count,
                         (btn, id) =>
                         {
                             var nCSSceneSelector_Item = btn.GetComponent<NCSSceneSelector.NCSSceneSelector_Item>();
                             nCSSceneSelector_Item.Initialize(nCSSceneSet.nCSScenes[id]);
                         },
                         (id) =>
                         {
                             NCSScene nCSScene = showcase.AddScene(nCSSceneSet.nCSScenes[id]);
                             nCSScene.Initialize(this);
                             nCSScene.NewData();
                             universalSelector.window.Close();
                             Refresh();
                         });
                 });
             });
        }

        public void Save()
        {
            AudioData audioData = new AudioData(this.audioData.savePath);
            foreach (var scene in showcase.scenes)
            {
                scene.nCSSceneSettings = scene.nCSScene.GetSaveData();

                IAudioFileReference audioFileReference = scene.nCSScene as IAudioFileReference;
                if (audioFileReference!=null)
                {
                    scene.nCSScene.player = this;
                    foreach (var keyValuePair in audioFileReference.audioFiles)
                    {
                        audioData.AppendValue(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            this.audioData = audioData;

            showcase.SaveData();
            this.audioData.SaveData();

            Debug.Log("保存完成");
        }
    }
}