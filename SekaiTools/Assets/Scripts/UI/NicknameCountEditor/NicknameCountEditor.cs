using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Count;
using System.Windows.Forms;

namespace SekaiTools.UI.NicknameCountEditor
{
    public class NicknameCountEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NicknameCountEditor_SelectorArea selectorArea;
        public ToggleGenerator toggleGenerator;
        [Header("Settings")]
        public IconSet iconSet;
        [Header("Message")]
        public MessageLayer.MessageLayerBase messageLayer;

        [System.NonSerialized] public NicknameCountData countData;
        [System.NonSerialized] public int currentCharacterId = 1;

        private void Awake()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) window.Close();

            countData = NicknameCountData.Load(folderBrowserDialog.SelectedPath);
            window.OnReShow.AddListener(() => Refresh());

            Initialize();
        }

        private void Initialize()
        {
            InitializeToggles();
            selectorArea.Initialize();
            Refresh();
        }

        public void InitializeToggles()
        {
            toggleGenerator.Generate(26,
                (Toggle toggle, int id) =>
                {
                    toggle.transform.GetChild(0).GetComponent<Image>().sprite = iconSet.icons[id + 1];
                    toggle.GetComponent<Image>().color = ConstData.characters[id + 1].imageColor;
                },
                (bool value, int id) =>
                {
                    if (value)
                    {
                        currentCharacterId = id + 1;
                        Refresh();
                    }
                });
            toggleGenerator.toggles[0].isOn = true;
        }

        public void Refresh()
        {
            selectorArea.Refresh();
        }

        public void Save()
        {
            countData.SaveChangedFiles();
            messageLayer.ShowMessage("保存成功");
        }
    }
}