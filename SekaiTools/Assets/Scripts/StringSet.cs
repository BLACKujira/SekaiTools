using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    [CreateAssetMenu(menuName = "SekaiTools/StringSet")]
    public class StringSet : ScriptableObject
    {
        public string[] values;
    }
}