using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class BondsHonorText : BondsHonorBase
    {
        [SerializeField] Text _text;

        public string text { get => _text.text; set => _text.text = value; }
    }
}