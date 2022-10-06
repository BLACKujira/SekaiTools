using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.OutsideMusicMetadataGeneratorInitialize
{
    public class GIP_OMMGFileInput : MonoBehaviour
    {
        public FolderSelectItem folderSelectItem;
        public StringListEditItem stringListEditItem;

        public string selectedFolder => folderSelectItem.SelectedPath;
        [System.NonSerialized] public List<string> extensionList = new List<string>() { ".mp3",".ogg",".wav" };

        private void Awake()
        {
            stringListEditItem.Initialize(extensionList);
        }
    }
}