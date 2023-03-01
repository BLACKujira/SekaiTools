using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public abstract class PathSelectItem : MonoBehaviour
    {
        public InputField pathInputField;

        [System.NonSerialized] public string defaultPath = string.Empty;
        public string SelectedPath
        {
            get
            {
                return string.IsNullOrEmpty(pathInputField.text) ? defaultPath : pathInputField.text;
            }
            set
            {
                pathInputField.text = value;
                onPathChange.Invoke(value);
            }
        }

        //[Obsolete] public abstract event Action<string> onPathSelect;
        //[Obsolete] public event Action<string> onPathReset;

        public PathChangeEvent onPathChange;

        [Serializable]
        public class PathChangeEvent : UnityEvent<string> { }

        public abstract void SelectPath();
        public void ResetPath()
        {
            pathInputField.text = defaultPath;
            onPathChange.Invoke(defaultPath);
        }
    }
}