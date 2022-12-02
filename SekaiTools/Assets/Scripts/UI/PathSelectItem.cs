using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
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
            }
        }

        public abstract event Action<string> onPathSelect;
        public event Action<string> onPathReset;

        public abstract void SelectPath();
        public void ResetPath()
        {
            pathInputField.text = defaultPath;
            if (onPathReset != null)
                onPathReset(defaultPath);
        }
    }
}