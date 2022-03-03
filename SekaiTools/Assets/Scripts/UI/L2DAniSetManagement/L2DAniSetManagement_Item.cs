using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using UnityEngine.UI;
using System.Windows.Forms;

namespace SekaiTools.UI.L2DAniSetManagement
{
    /// <summary>
    /// Live2D动画集合管理页面列表中的每一个项目
    /// </summary>
    public class L2DAniSetManagement_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text textName;
        public UnityEngine.UI.Button buttonShowFileDialog;
        public InputField inputFieldPath;
        public Text textMatching;
        public Text textMissing;
        [Header("Prefabs")]
        public Window nowLoadingWindow;

        [System.NonSerialized] public L2DAnimationSet animationSet;

        public void Initialize(L2DAnimationSet l2DAnimationSet,L2DAniSetManagement inWindow)
        {
            animationSet = l2DAnimationSet;
            textName.text = l2DAnimationSet.name;
            buttonShowFileDialog.onClick.AddListener(
                () =>
                {
                    //打开一个对话框选择文件夹
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                    if (dialogResult != DialogResult.OK) return;

                    //显示等待窗口，等待读取完毕
                    Window window = Instantiate(nowLoadingWindow);
                    window.Initialize(inWindow.window);
                    NowLoadingTypeA nowLoadingTypeA = (NowLoadingTypeA)window.controlScript;
                    nowLoadingTypeA.TitleText = "正在加载预览图片";
                    window.Show();
                    ImageData imageData = new ImageData();
                    nowLoadingTypeA.OnFinish += () => 
                    {
                        animationSet.previewSet = new L2DAnimationPreviewSet(folderBrowserDialog.SelectedPath, imageData.spriteList); 
                        RefreshInfo();
                    };
                    nowLoadingTypeA.StartProcess(imageData.LoadImages(folderBrowserDialog.SelectedPath));
                });
            RefreshInfo();
        }
        public void RefreshInfo()
        {
            inputFieldPath.text = animationSet.previewSet == null?"无预览":animationSet.previewSet.path;
            int availablePreviewCount = animationSet.GetAvailablePreviewCount();
            int animationCount = animationSet.animations.Count;
            textMissing.text = $"{animationCount-availablePreviewCount}/{animationCount} 缺失";
            textMatching.text = $"{availablePreviewCount}/{animationCount} 匹配";
        }
    }
}