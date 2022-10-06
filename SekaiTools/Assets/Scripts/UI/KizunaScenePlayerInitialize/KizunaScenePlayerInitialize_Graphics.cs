using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.IO;
using SekaiTools.Kizuna;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.KizunaScenePlayerInitialize
{
    public class KizunaScenePlayerInitialize_Graphics : MonoBehaviour
    {
        public KizunaScenePlayerInitialize kizunaScenePlayerInitialize;
        [Header("Components")]
        public Text textMatching;
        public Text textMissing;
        public List<Button> buttons;
        public InputField pathInputField;
        [Header("Prefab")]
        public Window nowLoadingWindowPrefab;

        [System.NonSerialized] public ImageData imageData;
        [System.NonSerialized] public KizunaSceneData.ImageMatchingCount imageMatchingCount;

        FolderBrowserDialog folderBrowserDialog;

        public bool IfGetReady
        {
            get
            {
                if (imageMatchingCount == null||imageMatchingCount.missing!=0) return false;

                return true;
            }
        }

        private void Awake()
        {
            folderBrowserDialog = new FolderBrowserDialog();
        }

        public void ShowLog()
        {
            kizunaScenePlayerInitialize.window.ShowLogWindow("详细信息", imageData==null? string.Empty : imageMatchingCount.Log);
        }

        public void NewData()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string selectedPath = folderBrowserDialog.SelectedPath;
            string savePath = Path.ChangeExtension(kizunaScenePlayerInitialize.kizunaSceneData.SavePath, ".imd");

            List<string> selectedFiles = new List<string>();
            string[] files = Directory.GetFiles(selectedPath);
            foreach (var file in files)
            {
                BondsHonorWordInfo bondsHonorWordInfo = BondsHonorWordInfo.IsBondsHonorWord(Path.GetFileName(file));
                if (bondsHonorWordInfo != null)
                {
                    foreach (var kizunaScene in kizunaScenePlayerInitialize.kizunaSceneData.kizunaSceneBaseArray)
                    {
                        if (bondsHonorWordInfo.IsKizunaOf(kizunaScene.charAID, kizunaScene.charBID))
                        {
                            selectedFiles.Add(file);
                            break;
                        }
                    }
                }
            }

            imageData = new ImageData(savePath);

            NowLoadingTypeA nowLoadingTypeA = kizunaScenePlayerInitialize.window.OpenWindow<NowLoadingTypeA>(nowLoadingWindowPrefab);
            nowLoadingTypeA.TitleText = "正在读取图像";
            nowLoadingTypeA.OnFinish += () =>
            {
                imageMatchingCount = ((KizunaSceneData)kizunaScenePlayerInitialize.kizunaSceneData).CountImageMatching(imageData);
                imageData.SaveData();
                Refresh();
            };
            nowLoadingTypeA.StartProcess(imageData.LoadFile(selectedFiles.ToArray()));

        }

        void LockButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
        }

        void UnlockButtons()
        {
            foreach (var button in buttons)
            {
                button.interactable = true;
            }
        }

        public void Refresh()
        {
            pathInputField.text = imageData == null? "请新建或读取资料" : imageData.savePath;
            textMatching.text = (imageMatchingCount == null ? "-/-" : $"{imageMatchingCount.matching}/{imageMatchingCount?.CountAll}") + " 匹配";
            textMissing.text = (imageMatchingCount == null ? "-/-" : $"{imageMatchingCount.missing}/{imageMatchingCount?.CountAll}") + " 缺失";
            if (kizunaScenePlayerInitialize.kizunaSceneData != null) UnlockButtons();
            else LockButtons();
        }

    }
}