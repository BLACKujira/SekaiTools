using Live2D.Cubism.Core;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Live2D
{
    [CreateAssetMenu(menuName = "SekaiTools/Live2D/InbuiltModelSet")]
    public class InbuiltModelSet : ScriptableObject
    {
        public List<CubismModel> inbuiltModelPrefabs;
    }
}