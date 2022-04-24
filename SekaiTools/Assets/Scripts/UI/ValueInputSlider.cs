using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class ValueInputSlider : ValueInputBase<Slider>
    {
        public override void Initialize(Func<float> getValue, Action<float> setValue, Action onValueChanged = null)
        {
            base.Initialize(getValue, setValue, onValueChanged);

            uIInput.onValueChanged.AddListener((_) => { onValueChanged(); });
        }

        public override void Refresh()
        {
            base.Refresh();

            uIInput.value = value;
        }

        public override void SetValue()
        {
            setValue(uIInput.value);
        }
    }
}