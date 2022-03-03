using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// 预制L2D动画集合的集合，不会动态生成
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/Live2D/InbuiltAnimationSet")]
    public class InbuiltAnimationSet : ScriptableObject
    {
        public L2DAnimationSet[] l2DAnimationSets = new L2DAnimationSet[27];
    }
}