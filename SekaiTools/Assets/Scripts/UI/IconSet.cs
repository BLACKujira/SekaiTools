using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    /// <summary>
    /// 角色图标集
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/UI/IconSet")]
    public class IconSet : ScriptableObject
    {
        public Sprite[] icons = new Sprite[32];
        
    }
}