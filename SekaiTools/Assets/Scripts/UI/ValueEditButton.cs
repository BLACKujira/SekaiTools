using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ValueEditButton : MonoBehaviour
    {
        [Header("Components")]
        public Button buttonMinus;
        public Button buttonPlus;
        public Button buttonReset;
        public Text value;

        Func<string> loadValue;

        public void Initialize(Action minusValue, Action plusValue, Action resetValue,Func<string> loadValue)
        {
            buttonMinus.onClick.AddListener(() => { minusValue(); value.text = loadValue(); });
            buttonPlus.onClick.AddListener(() => { plusValue(); value.text = loadValue(); });
            buttonReset.onClick.AddListener(() => { resetValue(); value.text = loadValue(); });
            value.text = loadValue();
            this.loadValue = loadValue;
        }

        public void LoadValue() { value.text = loadValue(); }
    }
}