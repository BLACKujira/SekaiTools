using SekaiTools.UI.BackGround;
using SekaiTools.UI.NicknameCountShowcase;
using SekaiTools.UI.Transition;
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

        public string SavePath { get; set; }

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
            File.WriteAllText(SavePath, json);
        }

        public NCSScene AddScene(NCSScene nCSScenePrefab)
        {
            NCSScene nCSScene = GameObject.Instantiate(nCSScenePrefab);
            nCSScene.gameObject.SetActive(false);
            nCSScene.name = nCSScenePrefab.name;
            scenes.Add(new Scene(nCSScene));
            return nCSScene;
        }

        public static NicknameCountShowcase LoadData(string savePath,bool instantiateScene = true)
        {
            string json = File.ReadAllText(savePath);
            NicknameCountShowcase nicknameCountShowcase = JsonUtility.FromJson<NicknameCountShowcase>(json);
            nicknameCountShowcase.SavePath = savePath;
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

        public void DestroyScenes()
        {
            foreach (var scene in scenes)
            {
                if (scene.nCSScene != null)
                {
                    scene.DestroyScene();
                }
            }
        }

        [System.Serializable]
        public class Scene
        {
            public bool useTransition = false;
            public SerializedTransition transition = null;
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
                try
                {
                    nCSScene.LoadData(nCSSceneSettings);
                }
                catch(System.Exception ex)
                {
                    Debug.LogError($"设置读取失败 {ex}");
                }
            }

            public void DestroyScene()
            {
                if(nCSScene != null)
                    GameObject.Destroy(nCSScene.gameObject);
            }
        }
    }
}