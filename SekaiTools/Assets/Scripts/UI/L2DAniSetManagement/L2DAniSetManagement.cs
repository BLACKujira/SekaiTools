using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using System.Windows.Forms;
using System.IO;

namespace SekaiTools.UI.L2DAniSetManagement
{
    /// <summary>
    /// Live2D动画集合管理页面
    /// </summary>
    public class L2DAniSetManagement : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefabs")]
        public L2DAniSetManagement_Item item;
        public Window nowLoadingWindow;
        [Header("Settings")]
        public InbuiltAnimationSet animationSets;
        public float itemDistance;

        List<L2DAniSetManagement_Item> items = new List<L2DAniSetManagement_Item>();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        private void Awake()
        {
            GenerateItem();
            folderBrowserDialog.Description = "选择保存预览文件夹的文件夹";
        }

        void GenerateItem()
        {
            L2DAnimationSet[] l2DAnimationSetArray = animationSets.L2DAnimationSetArray;
            scorllContent.sizeDelta = new Vector2(scorllContent.sizeDelta.x, itemDistance * l2DAnimationSetArray.Length);
            for (int i = 0; i < l2DAnimationSetArray.Length; i++)
            {
                L2DAnimationSet animationSet = l2DAnimationSetArray[i];
                L2DAniSetManagement_Item l2DAniSetManagement_Item = Instantiate(item,scorllContent);
                l2DAniSetManagement_Item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * itemDistance);
                l2DAniSetManagement_Item.Initialize(animationSet, this);
                items.Add(l2DAniSetManagement_Item);
            } 
        }

        public void LoadPreviews()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string selectedPath = folderBrowserDialog.SelectedPath;

            NowLoadingTypeA nowLoadingTypeA = window.OpenWindow<NowLoadingTypeA>(nowLoadingWindow);
            nowLoadingTypeA.TitleText = "正在加载预览";
            nowLoadingTypeA.StartProcess(ILoadPreviews(selectedPath));
        }
        IEnumerator ILoadPreviews(string selectedPath)
        {
            string[] paths = Directory.GetDirectories(selectedPath);
            foreach (var item in items)
            {
                foreach (var path in paths)
                {
                    string folderName = Path.GetFileName(path);
                    if (item.animationSet.name.Equals(folderName))
                    {
                        List<string> selectedFiles = new List<string>();
                        string[] files = Directory.GetFiles(path);
                        foreach (var file in files)
                        {
                            if (item.animationSet.GetAnimation(Path.GetFileNameWithoutExtension(file)))
                                selectedFiles.Add(file);
                        }
                        ImageData imageData = new ImageData(path);
                        yield return imageData.LoadFile(selectedFiles.ToArray());
                        item.animationSet.previewSet = imageData;
                        item.RefreshInfo();
                        break;
                    }
                }
            }
        }
    }
}