using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    [CreateAssetMenu(menuName = "SekaiTools/BackGroundPrefabSet")]
    public class BackGroundPrefabSet : ScriptableObject
    {
        public List<BackGroundRoot> backGrounds = new List<BackGroundRoot>();

        public BackGroundRoot GetPrefab(string name)
        {
            foreach (var backGroundPart in backGrounds)
            {
                if (backGroundPart.name.Equals(name))
                    return backGroundPart;
            }
            return null;
        }
    }
}
