using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    [CreateAssetMenu(menuName = "SekaiTools/NicknameCountShowcase/NCSSceneSet")]
    public class NCSSceneSet : ScriptableObject
    {
        public List<NCSScene> nCSScenes = new List<NCSScene>(); 

        public NCSScene GetValue(string name)
        {
            foreach (var nCSScene in nCSScenes)
            {
                if (name.Equals(nCSScene.name))
                    return nCSScene;
            }
            return null;
        }
    }
}