using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.NicknameCountShowcase;
using SekaiTools.Count.Showcase;
using UnityEngine.UI;
using SekaiTools.Count;
using System.IO;
using SekaiTools.UI.MessageLayer;

namespace SekaiTools.UI.NCSEditor
{
    public class NCSEditor : NCSPlayerBase
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public MessageLayerBase messageLayer;
        public SaveTipCloseWindowButton saveTipCloseWindowButton;
        [Header("Prefab")]
        public Window itemSelectorWindow;
        public GameObject addItemButton;

        public NCSSceneSet NCSSceneSet => GlobalData.globalData.nCSSceneSet;

        public override void Initialize(Settings settings)
        {
            base.Initialize(settings);
            saveTipCloseWindowButton.Initialize(() => showcase.SavePath);
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
                     universalSelector.Generate(NCSSceneSet.nCSScenes.Count,
                         (btn, id) =>
                         {
                             var nCSSceneSelector_Item = btn.GetComponent<NCSSceneSelector.NCSSceneSelector_Item>();
                             nCSSceneSelector_Item.Initialize(NCSSceneSet.nCSScenes[id]);
                         },
                         (id) =>
                         {
                             NCSScene nCSScene = showcase.AddScene(NCSSceneSet.nCSScenes[id]);
                             nCSScene.Initialize(this);
                             nCSScene.NewData();
                             nCSScene.Refresh();
                             nCSScene.gameObject.SetActive(false);
                             universalSelector.window.Close();
                             Refresh();
                         });
                 });
             });
        }

        public void Save()
        {
            HashSet<string> audioKeys = new HashSet<string>();
            HashSet<string> imageKeys = new HashSet<string>();

            foreach (var scene in showcase.scenes)
            {
                scene.nCSSceneSettings = scene.nCSScene.GetSaveData();

                if (scene.nCSScene is IAudioFileReference audioFileReference)
                {
                    audioKeys.UnionWith(audioFileReference.RequireAudioKeys);
                }
                if(scene.nCSScene is IImageFileReference imageFileReference) 
                {
                    imageKeys.UnionWith(imageFileReference.RequireImageKeys);
                }
            }
            audioData.RemoveUnusedValue(audioKeys);
            imageData.RemoveUnusedValue(imageKeys);

            showcase.SaveData();
            audioData.SaveData();
            imageData.SaveData();

            messageLayer.ShowMessage(Message.IO.STR_SAVECOMPLETE);
        }
    }
}