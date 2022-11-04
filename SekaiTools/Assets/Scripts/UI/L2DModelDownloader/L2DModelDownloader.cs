using SekaiTools.DecompiledClass;
using SekaiTools.Live2D;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.SekaiViewerInterface.Pages.Live2D;
using SekaiTools.UI.Downloader;
using SekaiTools.UI.L2DModelManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelDownloader
{
    public class L2DModelDownloader : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Button btnDownload;
        public L2DModelDownloader_InfoArea infoArea;
        public ButtonGenerator2D buttonGenerator2D;
        public MasterRefUpdateItem masterRefUpdateItem;
        [Header("Settings")]
        public Color colorEdgeExist;
        [Header("Prefab")]
        public Window downloaderPrefab;

        string selectedModelName;
        public string SelectedModelName => selectedModelName;

        private void Awake()
        {
            masterRefUpdateItem.OnTableUpdated += Refresh;
            GenerateItem();
        }

        void GenerateItem()
        {
            MasterCostume2D[] masterCostume2Ds = EnvPath.GetTable<MasterCostume2D>("costume2ds");
            string[] live2DModels = 
                masterCostume2Ds
                .Where((mc2d) => !string.IsNullOrEmpty(mc2d.live2dAssetbundleName))
                .Select((mc2d) => mc2d.live2dAssetbundleName).ToArray();
            buttonGenerator2D.Generate(live2DModels.Length,
                (btn, id) =>
                {
                    L2DModelManagement_Item l2DModelManagement_Item = btn.GetComponent<L2DModelManagement_Item>();
                    l2DModelManagement_Item.Initialize(live2DModels[id]);
                    if (L2DModelLoader.HasModel(live2DModels[id]))
                        l2DModelManagement_Item.imageEdgeColor.color = colorEdgeExist;
                },
                (id) =>
                {
                    selectedModelName = live2DModels[id];
                    infoArea.Refresh();
                });

        }

        public void Refresh()
        {
            buttonGenerator2D.ClearButtons();
            GenerateItem();
        }

        public void Apply()
        {
            string buildmodeldataURL = $"{SekaiViewer.AssetUrl}/live2d/model/{SelectedModelName}_rip/buildmodeldata.asset";
            string tempFile = Path.GetTempFileName();
            DownloadFileInfo downloadFileInfoIter1 = new DownloadFileInfo(buildmodeldataURL, tempFile);
            Downloader.Downloader downloaderIter1 = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
            Downloader.Downloader.Settings settingsIter1 = new Downloader.Downloader.Settings();
            settingsIter1.downloadFiles = new DownloadFileInfo[] { downloadFileInfoIter1 };
            downloaderIter1.Initialize(settingsIter1);
            downloaderIter1.OnComplete += () =>
            {
                if(downloaderIter1.HasError)
                {
                    WindowController.ShowMessage(Message.Error.STR_ERROR, "获取模型信息失败");
                    downloaderIter1.window.Close();
                    return;
                }
                downloaderIter1.window.Close();
                BuildModelData buildModelData = JsonUtility.FromJson<BuildModelData>(File.ReadAllText(tempFile));

                List<DownloadFileInfo> downloadFileInfos = new List<DownloadFileInfo>();
                DownloadFileInfo moc3FileInfo = new DownloadFileInfo
                (
                    $"{SekaiViewer.AssetUrl}/live2d/model/{SelectedModelName}_rip/{buildModelData.Moc3FileName}",
                    $"{EnvPath.AssetFolder}/assets/live2d/model/{SelectedModelName}_rip/{selectedModelName}.moc3"
                );
                downloadFileInfos.Add(moc3FileInfo);

                DownloadFileInfo physicsFileInfo = new DownloadFileInfo
                (
                    $"{SekaiViewer.AssetUrl}/live2d/model/{SelectedModelName}_rip/{buildModelData.PhysicsFileName}",
                    $"{EnvPath.AssetFolder}/assets/live2d/model/{SelectedModelName}_rip/{selectedModelName}.physics3.json"
                );
                downloadFileInfos.Add(physicsFileInfo);

                List<string> texturePaths = new List<string>();
                foreach (var textureName in buildModelData.TextureNames)
                {
                    DownloadFileInfo textureFileInfo = new DownloadFileInfo
                    (
                        $"{SekaiViewer.AssetUrl}/live2d/model/{SelectedModelName}_rip/{textureName}",
                        $"{EnvPath.AssetFolder}/assets/live2d/model/{SelectedModelName}_rip/{SelectedModelName}.2048/{Path.GetFileName(textureName)}"
                    );
                    downloadFileInfos.Add(textureFileInfo);
                    texturePaths.Add($"{SelectedModelName}.2048/{Path.GetFileName(textureName)}");
                }

                Downloader.Downloader downloaderIter2 = window.OpenWindow<Downloader.Downloader>(downloaderPrefab);
                Downloader.Downloader.Settings settingsIter2 = new Downloader.Downloader.Settings();
                settingsIter2.downloadFiles = downloadFileInfos.ToArray();
                downloaderIter2.Initialize(settingsIter2);
                downloaderIter2.OnComplete +=
                ()=>
                {
                    if (downloaderIter2.HasError)
                    {
                        WindowController.ShowMessage(Message.Error.STR_ERROR, "下载模型失败");
                        downloaderIter2.window.Close();
                        return;
                    }
                    downloaderIter2.window.Close();
                    string model3Path = $"{EnvPath.AssetFolder}/assets/live2d/model/{SelectedModelName}_rip/{selectedModelName}.model3.json";
                    Model3 model3 = new Model3(
                        Path.GetFileName(moc3FileInfo.savePath),
                        texturePaths.ToArray(),
                        Path.GetFileName(physicsFileInfo.savePath));
                    File.WriteAllText(model3Path, JsonUtility.ToJson(model3, true));
                    L2DModelLoader.AddLocalModel(model3Path);

                    selectedModelName = null;
                    Refresh();
                    infoArea.Refresh();
                };
                downloaderIter2.StartDownload();
            };
            downloaderIter1.StartDownload();
        }
    }
}