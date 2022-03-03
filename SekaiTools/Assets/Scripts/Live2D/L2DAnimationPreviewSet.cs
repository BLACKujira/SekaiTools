using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// Live2D动画预览集
    /// </summary>
    public class L2DAnimationPreviewSet
    {
        public string path;
        public List<Sprite> sprites = new List<Sprite>();
        public Dictionary<string, Sprite> previews;

        /// <summary>
        /// 读取所有图片，建立列表，再由列表建立字典
        /// </summary>
        public L2DAnimationPreviewSet(string path, List<Sprite> sprites)
        {
            this.path = path;
            this.sprites = sprites;
            InitializeDictionary();
        }

        /// <summary>
        /// 初始化，由列表建立字典
        /// </summary>
        void InitializeDictionary()
        {
            previews = new Dictionary<string, Sprite>();
            foreach (var sprite in sprites)
            {
                previews[sprite.name] = sprite;
            }
        }

        /// <summary>
        /// 获取预览
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public Sprite GetPreview(string animationName)
        {
            if (!previews.ContainsKey(animationName)) return null;
            return previews[animationName];
        }
    }
}