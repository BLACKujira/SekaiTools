using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    /// <summary>
    /// 内置图像资料，只读
    /// </summary>
    [CreateAssetMenu(menuName = "SekaiTools/InbuiltImageData")]
    public class InbuiltImageData : ScriptableObject
    {
        public List<Sprite> sprites = new List<Sprite>();
        public Sprite spriteNull;

        public Sprite this[int index] => sprites[index];
        Dictionary<string, Sprite> spritesDictionary;

        private void OnEnable()
        {
            spritesDictionary = new Dictionary<string, Sprite>();
            foreach (var sprite in sprites)
            {
                spritesDictionary[sprite.name] = sprite;
            }
        }

        public Sprite GetValue(string name)
        {
            if (!spritesDictionary.ContainsKey(name)) return spriteNull==null? null : spriteNull;
            else return spritesDictionary[name];
        }
    }
}