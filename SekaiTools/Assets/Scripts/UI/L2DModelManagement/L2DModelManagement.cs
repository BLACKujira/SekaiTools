using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using System.IO;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform scorllContent;
        public Button loadModelButton;
        public Button loadModelsButton;
        [Header("Prefabs")]
        public L2DModelManagement_Item item;
        public Window nowLoadingWindowPrefab;
        [Header("Settings")]
        public InbuiltAnimationSet animationSetsAll;
        public InbuiltAnimationSet animationSetsCharID;
        public float itemDistance;

        ModelLoader modelLoader;
        OpenFileDialog fileDialog = new OpenFileDialog();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        List<L2DModelManagement_Item> items = new List<L2DModelManagement_Item>();

        public ModelLoader ModelLoader { 
            get { if (modelLoader == null) modelLoader = ModelLoader.modelLoader; return modelLoader; }
            set => modelLoader = value; }

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
            scorllContent.sizeDelta = new Vector2(scorllContent.sizeDelta.x, itemDistance * ModelLoader.models.Count);
            for (int i = 0; i < ModelLoader.models.Count ; i++)
            {
                SekaiLive2DModel sekaiLive2DModel = ModelLoader.models[i];
                L2DModelManagement_Item l2DModelManagement_Item = Instantiate(item, scorllContent);
                l2DModelManagement_Item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * itemDistance);
                l2DModelManagement_Item.Initialize(sekaiLive2DModel,animationSetsAll, window);
                items.Add(l2DModelManagement_Item);
            }
        }
        void ClearItem()
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            items = new List<L2DModelManagement_Item>();
        }

        public void LoadModel()
        {
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string path = fileDialog.FileName;

            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
            nowLoadingTypeA.TitleText = "正在读取模型";
            nowLoadingTypeA.OnFinish += Refresh;
            nowLoadingTypeA.StartProcess(ModelLoader.LoadModel(path));
        }

        private void Refresh()
        {
            ClearItem();
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

            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
            nowLoadingTypeA.TitleText = "正在读取模型";
            nowLoadingTypeA.OnFinish += Refresh;
            nowLoadingTypeA.StartProcess(ILoadModels(files.ToArray()));
        }
        IEnumerator ILoadModels(string[] files)
        {
            foreach (var file in files)
            {
                yield return ModelLoader.LoadModel(file);
            }
        }


    }
}