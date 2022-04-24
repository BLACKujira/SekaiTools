using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    /// <summary>
    /// 用于储存背景装饰
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/BackGroundPartSet")]
    public class BackGroundPartSet : ScriptableObject
    {
        public List<BackGroundPart> backGroundParts = new List<BackGroundPart>();

        public BackGroundPart GetPart(string name)
        {
            foreach (var backGroundPart in backGroundParts)
            {
                if (backGroundPart.name.Equals(name))
                    return backGroundPart;
            }
            return null;
        }
    }
}