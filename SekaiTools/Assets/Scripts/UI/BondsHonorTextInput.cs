using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class BondsHonorTextInput : BondsHonorBase
    {
        [SerializeField] InputField _inputField;

        public InputField inputField { get => _inputField; }
        public string text { get => _inputField.text; set => _inputField.text = value; }
    }
}