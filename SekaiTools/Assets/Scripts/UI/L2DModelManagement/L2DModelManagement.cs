using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using System.IO;
using System;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public L2DModelManagement_InfoArea infoArea;
        public ButtonGenerator2D buttonGenerator2D;
        public Button downloadModelButton;
        public Button loadModelButton;
        public Button loadModelsButton;
        [Header("Prefab")]
        public Window modelDownloaderPrefab;

        OpenFileDialog fileDialog = new OpenFileDialog();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        ModelInfo currentModelInfo;
        public ModelInfo CurrentModelInfo => currentModelInfo;

        protected event Action<ModelInfo> onChangeSelection;

        private void Awake()
        {
            fileDialog.Title = "选择模型";
            fileDialog.Filter = "Models (*.model3.json)|*.model3.json|Motions (*.motion3.json)|*.motion3.json|Others (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            folderBrowserDialog.Description = "选择存放模型文件夹的文件夹";

            GenerateItem();
        }

        void GenerateItem()
        {
            string[] modelList = L2DModelLoader.ModelList;
            buttonGenerator2D.Generate(modelList.Length,
                (btn, id) =>
                {
                    L2DModelManagement_Item l2DModelManagement_Item = btn.GetComponent<L2DModelManagement_Item>();
                    l2DModelManagement_Item.Initialize(modelList[id]);
                },
                (id) =>
                {
                    currentModelInfo = L2DModelLoader.GetModelInfo(modelList[id]);
                    onChangeSelection(currentModelInfo);
                    infoArea.Refresh();
                });
        }

        public void LoadModel()
        {
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string path = fileDialog.FileName;

            L2DModelLoader.AddLocalModel(path);
            Refresh();
        }

        public void Refresh()
        {
            buttonGenerator2D.ClearButtons();
            GenerateItem();
        }

        public void LoadModels()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string selectedPath = folderBrowserDialog.SelectedPath;

            List<string> files = new List<string>();
            foreach (var dir in Directory.GetDirectories(selectedPath))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (file.EndsWith(".model3.json"))
                        files.Add(file);
                }
            }

            foreach (var file in files)
            {
                L2DModelLoader.AddLocalModel(file);
            }

            Refresh();
        }

        public void DeleteModel()
        {
            WindowController.ShowCancelOK("注意", $"确定要移除模型 {CurrentModelInfo.modelName} 吗？",
                () =>
                {
                    L2DModelLoader.RemoveModel(CurrentModelInfo.modelName);
                    Refresh();
                });
            currentModelInfo = null;
            infoArea.Refresh();
        }

        public void DownloadModel()
        {
            L2DModelDownloader.L2DModelDownloader l2DModelDownloader = window.OpenWindow<L2DModelDownloader.L2DModelDownloader>(modelDownloaderPrefab);
            l2DModelDownloader.window.OnClose.AddListener(() =>
            {
                Refresh();
            });
        }
    }
}