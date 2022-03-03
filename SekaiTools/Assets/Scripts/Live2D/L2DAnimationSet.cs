using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework.MotionFade;
using System.IO;
using UnityEditor;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// Live2D动画集，同时绑定动画对应的CubismFadeMotionList
    /// </summary>
    public class L2DAnimationSet : ScriptableObject
    {
        public CubismFadeMotionList fadeMotionList;
        public L2DAnimationPreviewSet previewSet = null;

        public List<AnimationClip> motionPack;
        public List<AnimationClip> facialPack;
        public List<MotionSet> motionSets;

        public List<AnimationClip> animationClips = new List<AnimationClip>();
        public Dictionary<string, AnimationClip> animations;

        private void OnEnable()
        {
            InitializeDictionary();
        }

        /// <summary>
        ///由列表animationClips生成字典animations
        /// </summary>
        void InitializeDictionary()
        {
            animations = new Dictionary<string, AnimationClip>();
            foreach (var animationClip in motionPack)
            {
                animations.Add(animationClip.name, animationClip);
            }
            foreach (var animationClip in facialPack)
            {
                animations.Add(animationClip.name, animationClip);
            }
        }

        /// <summary>
        /// 尝试将动作加入MotionSet
        /// </summary>
        /// <param name="animationClip"></param>
        void AddMotionToMotionSet(AnimationClip animationClip)
        {
            string[] nameArray = animationClip.name.Split('-');
            //挑选出armescape动画，减少normal集合大小
            if (nameArray[1].Equals("normal") && nameArray[2].StartsWith("armescape"))
            {
                for (int i = 0; i < motionSets.Count; i++)
                {
                    MotionSet set = motionSets[i];
                    if (set.setName.Equals("armescape"))
                    {
                        set.motions.Add(animationClip);
                        return;
                    }
                }
                List<AnimationClip> newList = new List<AnimationClip>();
                newList.Add(animationClip);
                motionSets.Add(new MotionSet("armescape", newList));
            }
            else
            {
                for (int i = 0; i < motionSets.Count; i++)
                {
                    MotionSet set = motionSets[i];
                    if (set.setName.Equals(nameArray[1]))
                    {
                        set.motions.Add(animationClip);
                        return;
                    }
                }
                List<AnimationClip> newList = new List<AnimationClip>();
                newList.Add(animationClip);
                motionSets.Add(new MotionSet(nameArray[1], newList));
            }
        }

        /// <summary>
        /// 分类animationClips，生成motionPack和facialPack
        /// </summary>
        public void SortByAnimationType()
        {
            facialPack = new List<AnimationClip>();
            motionPack = new List<AnimationClip>();
            foreach (var animationClip in animationClips)
            {
                if (animationClip.name.StartsWith("face_"))
                {
                    facialPack.Add(animationClip);
                }
                if (animationClip.name.StartsWith("w-") || animationClip.name.StartsWith("m-") || animationClip.name.StartsWith("n-"))
                {
                    motionPack.Add(animationClip);
                }
            }
        }

        /// <summary>
        /// 分类motionPack，生成motionSets
        /// </summary>
        public void SortByMotionType()
        {
            motionSets = new List<MotionSet>();
            foreach (var pack in motionPack)
            {
                AddMotionToMotionSet(pack);
            }
            List<MotionSet> setOld = motionSets;
            motionSets = new List<MotionSet>();
            List<AnimationClip> motionsOther = new List<AnimationClip>();
            foreach (var set in setOld)
            {
                if (set.motions.Count > 3)
                    motionSets.Add(set);
                else
                    motionsOther.AddRange(set.motions);
            }
            if (motionsOther.Count != 0)
                motionSets.Add(new MotionSet("other", motionsOther));
        }

        /// <summary>
        /// 获取动画，若动画不存在则返回Null
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public AnimationClip GetAnimation(string animationName)
        {
            if (string.IsNullOrEmpty(animationName) || !animations.ContainsKey(animationName)) return null;
            return animations[animationName];
        }

        /// <summary>
        /// 获取动画预览，若没有预览集则返回Null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetPreview(string name)
        {
            if (previewSet == null) return null;
            return previewSet.GetPreview(name);
        }

        /// <summary>
        /// 统计有预览的动画数量
        /// </summary>
        /// <returns></returns>
        public int GetAvailablePreviewCount()
        {
            if (previewSet == null) return 0;
            int count = 0;
            foreach (var keyValuePair in animations)            
            {
                if (previewSet.previews.ContainsKey(keyValuePair.Key)) count++;
            }
            return count;
        }

        /// <summary>
        /// 寻找动作所在集合
        /// </summary>
        /// <param name="animationClip"></param>
        /// <returns></returns>
        public MotionSet FindMotionSet(AnimationClip animationClip)
        {
            if (animationClip == null) return null;
            foreach (var motionSet in motionSets)
            {
                foreach (var motion in motionSet.motions)
                {
                    if (motion == animationClip)
                        return motionSet;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 读取目录里的动画生成一个L2DAnimationSet,仅在编辑器内使用
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static L2DAnimationSet CreateL2DAnimationSet(string directory)
        {
            L2DAnimationSet l2DAnimationSet = CreateInstance<L2DAnimationSet>();
            string[] files = Directory.GetFiles(Path.Combine(directory, "motions"));
            foreach (var file in files)
            {
                if (Path.GetExtension(file).Equals(".anim"))
                {
                    AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(file);
                    l2DAnimationSet.animationClips.Add(animationClip);
                }
            }
            l2DAnimationSet.SortByAnimationType();
            l2DAnimationSet.SortByMotionType();
            return l2DAnimationSet;
        }
#endif

        [System.Serializable]
        public class MotionSet
        {
            public string setName;
            public List<AnimationClip> motions;

            public MotionSet(string setName, List<AnimationClip> motions)
            {
                this.setName = setName;
                this.motions = motions;
            }
        }
    }
}