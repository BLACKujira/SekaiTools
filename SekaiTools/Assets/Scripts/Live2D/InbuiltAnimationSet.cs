using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// 预制L2D动画集合的集合，不会动态生成
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/Live2D/InbuiltAnimationSet")]
    public class InbuiltAnimationSet : ScriptableObject
    {
        [SerializeField] List<L2DAnimationSet> l2DAnimationSets = new List<L2DAnimationSet>();

        Dictionary<string, L2DAnimationSet> animationSetDictionary = null;
        Dictionary<string, L2DAnimationSet> AnimationSetDictionary
        {
            get
            {
                if (animationSetDictionary == null)
                {
                    animationSetDictionary = new Dictionary<string, L2DAnimationSet>();
                    foreach (var l2DAnimationSet in l2DAnimationSets)
                    {
                        animationSetDictionary[l2DAnimationSet.name] = l2DAnimationSet;
                    }
                }
                return animationSetDictionary;
            }
        }

        public L2DAnimationSet[] L2DAnimationSetArray => l2DAnimationSets.ToArray();

        public L2DAnimationSet GetAnimationSetByModelName(string modelName)
        {
            string animationSetName = null;
            if (modelName.StartsWith("clb") || modelName.StartsWith("sub"))
            {
                animationSetName = modelName + "_motion_base";
            }
            else
            {
                int charId = ConstData.IsLive2DModelOfCharacter(modelName);
                if (charId != 0)
                {
                    animationSetName = $"{charId:00}{(Character)charId}_motion_base";
                }
            }

            if (string.IsNullOrEmpty(animationSetName))
                return null;
            if (AnimationSetDictionary.ContainsKey(animationSetName))
                return AnimationSetDictionary[animationSetName];
            return null;
        }

        public L2DAnimationSet GetAnimationSet(string animationSetName)
        {
            if (AnimationSetDictionary.ContainsKey(animationSetName))
                return animationSetDictionary[animationSetName];
            return null;
        }

#if UNITY_EDITOR
        public void UpdateSet(List<L2DAnimationSet> l2DAnimationSets)
        {
            this.l2DAnimationSets = l2DAnimationSets;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}