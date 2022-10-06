using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.BackGround;

namespace SekaiTools.Spine
{
    [System.Serializable]
    public class SpineScene
    {
        public BackGroundController.BackGroundSaveData backGroundData = null;
        public int spineLayerID = 0;

        public SpineObject[] spineObjects = new SpineObject[0];

        [System.Serializable]
        public class SpineObject
        {
            public string atlasAssetName;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
            public bool ifFlip;
            public int sortingOrder;
            public string animation = ConstData.defaultSpineAnimation;
            public float animationSpeed = 1;
            public float animationProgress = 0;
        }

        public string GetSaveData()
        {
            return JsonUtility.ToJson(this);  
        }

        public static SpineScene LoadData(string serializedData)
        {
            return JsonUtility.FromJson<SpineScene>(serializedData);
        }

        public static SpineScene GetEmptyScene()
        {
            SpineScene spineScene = new SpineScene();
            return spineScene;
        }

        public bool HasSameAnimation()
        {
            HashSet<string> animations = new HashSet<string>();
            foreach (var spineObject in spineObjects)
            {
                if (animations.Contains(spineObject.animation))
                    return true;
                animations.Add(spineObject.animation);
            }
            return false;
        }

        public bool HasSameAnimationAndOffset()
        {
            if (spineObjects.Length <= 1) return false;
            for (int i = 0; i < spineObjects.Length -1; i++)
            {
                for (int j = i+1; j < spineObjects.Length; j++)
                {
                    if (spineObjects[i].animation.Equals(spineObjects[j].animation)
                        && spineObjects[i].animationProgress == spineObjects[j].animationProgress)
                        return true;
                }
            }
            return false;
        }
    }
}