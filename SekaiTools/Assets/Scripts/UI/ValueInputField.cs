using SekaiTools.UI.Modifier;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    [Serializable]
    public class ValueSetButton
    {
        public enum Mode { set, add, minus }
        public Mode mode;
        public Button button;
        public float value;
    }

    public abstract class ValueInputBase<UIType> : MonoBehaviour, IValueEditElement<float>
    {
        public UIType uIInput;
        public List<ValueSetButton> valueSetButtons = new List<ValueSetButton>();

        public float value { get => getValue(); set=>setValue(value); }
        protected Func<float> getValue;
        protected Action<float> setValue;
        protected Action onValueChanged;

        protected virtual void Awake()
        {
            InitializeValueSetButton();
        }

        protected virtual void InitializeValueSetButton()
        {
            foreach (var valueSetButton in valueSetButtons)
            {
                valueSetButton.button.onClick.AddListener(
                    () =>
                    {
                        switch (valueSetButton.mode)
                        {
                            case ValueSetButton.Mode.set:
                                value = valueSetButton.value;
                                break;
                            case ValueSetButton.Mode.add:
                                value = value + valueSetButton.value;
                                break;
                            case ValueSetButton.Mode.minus:
                                value = value - valueSetButton.value;
                                break;
                            default:
                                break;
                        }
                        Refresh();
                    });
            }
        }

        public virtual void Initialize(Func<float> getValue, Action<float> setValue, Action onValueChanged = null)
        {
            this.getValue = getValue;
            this.setValue = setValue;
            this.onValueChanged = onValueChanged;
            Refresh();
        }

        public abstract void SetValue();

        public virtual float GetValue()
        {
            return getValue();
        }

        public virtual void Refresh()
        {

        }
    }

    [Serializable]
    public class ValueInputField : ValueInputBase<InputField>
    {
        public float defaultValue = 0;

        public override void Initialize(Func<float> getValue, Action<float> setValue, Action onValueChanged = null)
        {
            base.Initialize(getValue, setValue, onValueChanged);
            uIInput.onEndEdit.AddListener((_) => { onValueChanged(); });
        }

        public override void Refresh()
        {
            uIInput.text = value.ToString();
        }

        public override void SetValue()
        {
            float value;
            try
            {
                value = float.Parse(uIInput.text);
            }
            catch
            {
                value = defaultValue;
            }
            setValue(value);
        }
    }
}