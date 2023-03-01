using SekaiTools.UI.CutinScenePlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Cutin
{
    [CreateAssetMenu(menuName = "SekaiTools/Cutin/CutinScenePlayerSet")]
    public class CutinScenePlayerSet : ScriptableObject
    {
        public CutinScenePlayerSet_Item[] items;

        public CutinScenePlayerSet_Item DefaultItem => items[0]; 

        public CutinScenePlayerSet_Item GetItem(string key)
        {
            foreach (var item in items)
            {
                if (item.key.Equals(key))
                    return item;
            }
            return null;
        }
    }

    [System.Serializable]
    public class CutinScenePlayerSet_Item
    {
        public string key;
        public CutinScenePlayer_Player player;
        public string displayName;
        public Sprite preview;
    }
}