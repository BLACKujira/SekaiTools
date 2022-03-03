using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    /// <summary>
    /// 用于储存背景预制件和背景装饰
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/BackGroundPartSet")]
    public class BackGroundPartSet : ScriptableObject
    {
        public List<BackGroundPart> backGroundParts = new List<BackGroundPart>();
    }
}