using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

namespace SekaiTools.UI
{
    public class LoadFileSelectItem : PathSelectItem
    {
        public string fileFilter = "All(*.*) | *.*";

        public override event Action<string> onPathSelect;

        public override void SelectPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Ñ¡ÔñÎÄ¼þ";
            openFileDialog.Filter = fileFilter;
            openFileDialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(SelectedPath)) openFileDialog.FileName = SelectedPath;

            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            pathInputField.text = openFileDialog.FileName;

            if (onPathSelect != null)
                onPathSelect(SelectedPath);
        }
    }
}