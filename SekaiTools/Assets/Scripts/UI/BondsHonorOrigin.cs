using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class BondsHonorOrigin : BondsHonorBase
    {
        [SerializeField] Image _textSprite;

        public Sprite textSprite { get => _textSprite.sprite; set => _textSprite.sprite = value; }
    }
}