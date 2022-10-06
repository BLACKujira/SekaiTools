using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

namespace SekaiTools.UI
{
    public class FolderSelectItem : PathSelectItem
    {
        public override void SelectPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            pathInputField.text = folderBrowserDialog.SelectedPath;
        }
    }
}