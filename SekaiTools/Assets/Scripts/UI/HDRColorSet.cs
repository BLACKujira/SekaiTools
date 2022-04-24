using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    [CreateAssetMenu(menuName = "SekaiTools/UI/HDRColorSet")]
    public class HDRColorSet : ScriptableObject
    {
        [ColorUsage(true, true)]
        public Color[] colors = new Color[56];

        //private void Awake()
        //{
        //    for (int i = 1; i <= 56; i++)
        //    {
        //        colors[i] = ConstData.characters[i].imageColor;
        //    }
        //}
    }
}