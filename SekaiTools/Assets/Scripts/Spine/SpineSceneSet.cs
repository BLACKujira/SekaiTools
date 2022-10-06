using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Spine
{
    [CreateAssetMenu(menuName = "SekaiTools/Spine/SpineSceneSet")]
    public class SpineSceneSet : ScriptableObject
    {
        public List<Scene> scenes;

        public Scene this[int index] => scenes[index];

        [System.Serializable]
        public class Scene
        {
            public string name;
            public Sprite preview;
            public TextAsset spineScene;

            public Scene(Sprite preview, TextAsset spineScene)
            {
                this.preview = preview;
                this.spineScene = spineScene;
            }

            public SpineSceneWithMeta GetInstance()
            {
                if (spineScene == null)
                    return new SpineSceneWithMeta(new SpineScene());
                return JsonUtility.FromJson<SpineSceneWithMeta>(spineScene.text);
            }
        }
    }
}