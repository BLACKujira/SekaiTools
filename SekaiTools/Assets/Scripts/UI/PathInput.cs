using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI
{
    public class PathInput : MonoBehaviour
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public InputField inputField;
        public string defaultPath;

        public void OpenFolder()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            inputField.text = folderBrowserDialog.SelectedPath;
        }
        public void ResetPath()
        {
            inputField.text = defaultPath;
        }
        public void ClearPath()
        {
            inputField.text = string.Empty;
        }
    }
}