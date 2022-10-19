using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Live2D;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement_InfoArea : MonoBehaviour
    {
        public L2DModelManagement l2DModelManagement;
        [Header("Components")]
        public Button btnDelete;
        public Text txtName;
        public Text txtInfo;
        public Image imgPreview;
        [Header("Settings")]
        public InbuiltImageData modelPreviews;

        public void Refresh()
        {
            ModelInfo modelInfo = l2DModelManagement.CurrentModelInfo;
            if(modelInfo == null)
            {
                SetDataNull();
                return;
            }
            txtName.text = modelInfo.modelName;
            txtInfo.text = modelInfo.ifInbuilt ? "内置模型" : $"本地模型，位置 {modelInfo.modelPath}";
            imgPreview.sprite = modelPreviews.GetValue(modelInfo.modelName);

            if (btnDelete != null)
            {
                if (modelInfo.ifInbuilt)
                {
                    btnDelete.interactable = false;
                }
                else
                {
                    btnDelete.interactable = true;
                }
            }
        }

        public void SetDataNull()
        {
            txtName.text = "请选择模型";
            txtInfo.text = string.Empty;
            imgPreview.sprite = modelPreviews.spriteNull;
            if (btnDelete != null)
                btnDelete.interactable = false;
        }
    }
}