using UnityEngine;

namespace SekaiTools.UI
{
    [CreateAssetMenu(menuName = "SekaiTools/ColorSet")]
    public class ColorSet : ScriptableObject
    {
        [SerializeField] Color[] colors = new Color[56];
        public Color[] Colors => colors;

        public Color this[int index] => colors[index]; 
    }
}