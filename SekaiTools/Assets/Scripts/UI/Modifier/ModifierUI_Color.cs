using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI.Modifier
{
    public class ModifierUI_ColorBase : MonoBehaviour,IValueEditElement<Color>
    {
        public virtual Color color
        {
            get => getValue();
            set
            {
                setValue(value);
                Refresh();
                onColorChange.Invoke();
            }
        }

        [Header("events")]
        public UnityEvent onColorChange;

        protected Action<Color> setValue;
        protected Func<Color> getValue;

        public virtual void Refresh()
        {

        }

        public virtual void Initialize(Func<Color> getValue, Action<Color> setValue, Action onValueChanged = null)
        {
            this.setValue = setValue;
            this.getValue = getValue;
            Refresh();
        }

        public void SetValue()
        {
            setValue(color);
        }

        public Color GetValue()
        {
            return getValue();
        }
    }


    /// <summary>
    /// 基础色彩改造器，不适用于HDR色彩
    /// </summary>
    public class ModifierUI_Color : ModifierUI_ColorBase
    {
        public enum Mode { RGB,HSV}
        public Mode mode = Mode.RGB;

        [Header("Components")]
        public MonoBehaviour valueR;
        public MonoBehaviour valueG;
        public MonoBehaviour valueB;

        public MonoBehaviour valueH;
        public MonoBehaviour valueS;
        public MonoBehaviour valueV;

        public MonoBehaviour valueA;

        public List<ValueSetButton> valueSetButtons = new List<ValueSetButton>();

        public override void Initialize(Func<Color> getValue, Action<Color> setValue, Action onValueChanged = null)
        {
            this.setValue = setValue;
            this.getValue = getValue;

            ((IValueEditElement<float>)valueR).Initialize(
                () => color.r,
                (value) => color = new Color(value, color.g, color.b, color.a),
                () =>
                {
                    ((IValueEditElement<float>)valueR).SetValue();
                    onColorChange.Invoke();
                });

            ((IValueEditElement<float>)valueG).Initialize(
                    () => color.g,
                    (value) => color = new Color(color.r, value, color.b, color.a),
                    () =>
                    {
                        ((IValueEditElement<float>)valueG).SetValue();
                        onColorChange.Invoke();
                    });

            ((IValueEditElement<float>)valueB).Initialize(
                () => color.b,
                (value) => color = new Color(color.r, color.g, value, color.a),
                () =>
                {
                    ((IValueEditElement<float>)valueB).SetValue();
                    onColorChange.Invoke();
                });


            ((IValueEditElement<float>)valueH).Initialize(
                () => { float h; Color.RGBToHSV(color, out h, out _, out _); return h; },
                (value) =>
                {
                    float h, s, v;
                    Color.RGBToHSV(color, out h, out s, out v);
                    color = Color.HSVToRGB(value, s, v);
                },
                () =>
                {
                    ((IValueEditElement<float>)valueH).SetValue();
                    onColorChange.Invoke();
                });

            ((IValueEditElement<float>)valueS).Initialize(
                () => { float s; Color.RGBToHSV(color, out _, out s, out _); return s; },
                (value) =>
                {
                    float h, s, v;
                    Color.RGBToHSV(color, out h, out s, out v);
                    color = Color.HSVToRGB(h, value, v);
                },
                () =>
                {
                    ((IValueEditElement<float>)valueS).SetValue();
                    onColorChange.Invoke();
                });


            ((IValueEditElement<float>)valueV).Initialize(
                () => { float v; Color.RGBToHSV(color, out _, out _, out v); return v; },
                (value) =>
                {
                    float h, s, v;
                    Color.RGBToHSV(color, out h, out s, out v);
                    color = Color.HSVToRGB(h, s, value);
                },
                () => 
                {
                    ((IValueEditElement<float>)valueV).SetValue(); 
                    onColorChange.Invoke();
                });

            if(valueA)
            ((IValueEditElement<float>)valueA).Initialize(
                () => color.a,
                (value) => color = new Color(color.r, color.g, color.b, value),
                () =>
                {
                    ((IValueEditElement<float>)valueA).SetValue();
                    onColorChange.Invoke();
                });

            Refresh();
        }

        [System.Serializable]
        public class ValueSetButton
        {
            public enum Mode { set, add, minus }
            public Button button;
            [ColorUsage(true,true)]
            public Color value;
            public Mode mode;
        }

        public void ChangeMode()
        {
            if(mode==Mode.RGB)
            {
                mode = Mode.HSV;
            }
            else
            {
                mode = Mode.RGB;
            }
            Refresh();
        }

        public override void Refresh()
        {
            MonoBehaviour[] monoBehaviours = { valueR, valueG, valueB, valueH, valueS, valueV, valueA };
            foreach (var monoBehaviour in monoBehaviours)
            {
                if (!monoBehaviour) continue;
                IValueEditElement<float> valueEditElement = (IValueEditElement<float>)monoBehaviour;
                valueEditElement.Refresh();
            }
        }
    }
}