using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Effect
{
    public class HDRColorSprite : HDRColorComponent
    {
        public override Material InitializeMaterial()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Material material = new Material(spriteRenderer.material);
            spriteRenderer.material = material;
            return material;
        }
    }
}