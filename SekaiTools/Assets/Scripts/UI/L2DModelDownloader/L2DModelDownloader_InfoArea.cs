using SekaiTools.Live2D;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelDownloader
{
    public class L2DModelDownloader_InfoArea : MonoBehaviour
    {
        public L2DModelDownloader l2DModelDownloader;
        [Header("Components")]
        public Text txtName;
        public Image imgPreview;
        [Header("Settings")]
        public InbuiltImageData modelPreviews;

        public void Refresh()
        {
            string modelName = l2DModelDownloader.SelectedModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                SetDataNull();
                return;
            }
            bool hasModel = L2DModelLoader.HasModel(modelName);
            txtName.text = hasModel ? $"{modelName} (已在库中)" : modelName;
            imgPreview.sprite = modelPreviews.GetValue(modelName);
            l2DModelDownloader.btnDownload.interactable = !hasModel;
        }

        public void SetDataNull()
        {
            txtName.text = "请选择模型";
            imgPreview.sprite = modelPreviews.spriteNull;
        }
    }
}