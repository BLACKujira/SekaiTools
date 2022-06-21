using SekaiTools.UI.BackGround;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.Count.Showcase
{
    [System.Serializable]
    public class NicknameCountShowcase : ISaveData
    {
        public List<Scene> scenes = new List<Scene>();
        public int[] charactersRequireL2d;

        public string savePath { get; set; }

        public void SaveData()
        {
            List<int> charactersRequireL2d = new List<int>();
            foreach (var scene in scenes)
            {
                scene.nCSSceneType = scene.nCSScene.name;
                scene.nCSSceneSettings = scene.nCSScene.GetSaveData();

                ILive2DReferenceCharacter live2DReferenceCharacter = scene.nCSScene as ILive2DReferenceCharacter;
                if (live2DReferenceCharacter !=  null)
                {
                    charactersRequireL2d.Add(live2DReferenceCharacter.l2dReferenceCharacterID);
                }
                this.charactersRequireL2d = charactersRequireL2d.ToArray();
            }

            string json = JsonUtility.ToJson(this,true);
            File.WriteAllText(savePath, json);
        }

        public NCSScene AddScene(NCSScene nCSScenePrefab)
        {
            NCSScene nCSScene = GameObject.Instantiate(nCSScenePrefab);
            nCSScene.name = nCSScenePrefab.name;
            nCSScene.gameObject.SetActive(false);
            scenes.Add(new Scene(nCSScene));
            return nCSScene;
        }

        public static NicknameCountShowcase LoadData(string savePath,bool instantiateScene = true)
        {
            string json = File.ReadAllText(savePath);
            NicknameCountShowcase nicknameCountShowcase = JsonUtility.FromJson<NicknameCountShowcase>(json);
            nicknameCountShowcase.savePath = savePath;
            if(instantiateScene) nicknameCountShowcase.InstantiateScenes();
            return nicknameCountShowcase;
        }
        
        public void InstantiateScenes()
        {
            foreach (var scene in scenes)
            {
                if (scene.nCSScene == null)
                {
                    scene.InstantiateScene();
                    scene.nCSScene.gameObject.SetActive(false);
                }
            }
        }

        [System.Serializable]
        public class Scene
        {
            public bool useTransition = false;
            public Transition transition = null;
            public bool changeBackGround = false;
            public BackGroundController.BackGroundSaveData backGround = null;

            [System.NonSerialized] public NCSScene nCSScene = null;
            public string nCSSceneType;
            public string nCSSceneSettings;

            public Scene(NCSScene nCSScene)
            {
                backGround = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
                this.nCSScene = nCSScene;
                nCSSceneType = nCSScene.name;
                nCSSceneSettings = nCSScene.GetSaveData();
            }

            public void InstantiateScene()
            {
                nCSScene = GameObject.Instantiate(GlobalData.globalData.nCSSceneSet.GetValue(nCSSceneType));
                nCSScene.name = nCSSceneType;
                nCSScene.LoadData(nCSSceneSettings);
            }
        }
        [System.Serializable]
        public class Transition
        {
            public string type;
            public string serialisedSettings;

            public Transition(string type, string serialisedSettings)
            {
                this.type = type;
                this.serialisedSettings = serialisedSettings;
            }
        }
    }
}