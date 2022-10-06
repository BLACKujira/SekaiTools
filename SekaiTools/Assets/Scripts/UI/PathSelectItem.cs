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
        public string SelectedPath => string.IsNullOrEmpty(pathInputField.text) ? defaultPath : pathInputField.text;

        public abstract void SelectPath();
        public void ResetPath()
        {
            pathInputField.text = defaultPath;
        }
    }
}