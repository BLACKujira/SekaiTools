using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SekaiTools.UI.Modifier
{
    public interface IValueEditElement<T>
    {
        void Initialize(Func<T> getValue, Action<T> setValue,Action onValueChanged = null);
        void SetValue();
        T GetValue();
        void Refresh();
    }

    public class ModifierUI_UniversalValueEdit : MonoBehaviour
    {
        public List<MonoBehaviour> valueEditElements = new List<MonoBehaviour>();

        public class ValueEditSetting
        {
            public Func<float> getValue;
            public Action<float> setValue;

            public ValueEditSetting(Func<float> getValue, Action<float> setValue)
            {
                this.getValue = getValue;
                this.setValue = setValue;
            }
        }

        public void Initialize(params ValueEditSetting[] valueEditSettings)
        {
            for (int i = 0; i < valueEditElements.Count; i++)
            {
                int id = i;
                MonoBehaviour monoBehaviour = valueEditElements[i];
                if (monoBehaviour is IValueEditElement<float>) 
                {
                    IValueEditElement<float> valueEditElement = (IValueEditElement<float>)monoBehaviour;
                    valueEditElement.Initialize(
                        valueEditSettings[i].getValue, 
                        valueEditSettings[i].setValue, 
                        ()=> { valueEditElement.SetValue(); });
                }
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < valueEditElements.Count; i++)
            {
                int id = i;
                MonoBehaviour monoBehaviour = valueEditElements[i];
                if (monoBehaviour is IValueEditElement<float>) 
                {
                    IValueEditElement<float> valueEditElement = (IValueEditElement<float>)monoBehaviour;
                    valueEditElement.Refresh();
                }
            }
        }
    }
}