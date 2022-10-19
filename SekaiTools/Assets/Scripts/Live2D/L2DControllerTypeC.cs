using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// C型Live2D模型控制器，只能同时显示一个模型，用于预览
    /// </summary>
    public class L2DControllerTypeC : MonoBehaviour
    {
        [Header("Components")]
        public RawImage image;
        [Header("Settings")]
        public Vector3 normalScale = new Vector3(10, 10, 1);
        [Header("Prefabs")]
        public Camera l2DCameraPrefab;

        public SekaiLive2DModel model { get; private set; }

        public Vector2 ModelPosition => (Vector2)model.transform.position - modelPosition;
        public float ModelScale => model.transform.localScale.x / normalScale.x;
        readonly Vector2 unusedModelPosition = Vector2.zero;
        readonly Vector2 modelPosition = new Vector2(-32, 0);

        Camera l2DCamera;

        /// <summary>
        /// 显示模型（不需要提前设置）
        /// </summary>
        /// <param name="model"></param>
        public void ShowModel(SekaiLive2DModel model)
        {
            if (this.model)
            {
                this.model.transform.position = unusedModelPosition;
                this.model.gameObject.SetActive(false);
            }
            this.model = model;
            model.transform.position = modelPosition;
            model.transform.localScale = normalScale;
            model.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏模型
        /// </summary>
        /// <returns></returns>
        public SekaiLive2DModel HideModel()
        {
            if (!model) return null;
            SekaiLive2DModel lastModel = model;
            model.transform.position = unusedModelPosition;
            model.transform.localScale = normalScale;
            model.gameObject.SetActive(false);
            model = null;
            return lastModel;
        }

        public void SetModelPosition(Vector2 offset)
        {
            if(model)
            {
                model.transform.position = offset + modelPosition;
            }
        }
        public void SetModelScale(float scale)
        {
            if (model)
            {
                model.transform.localScale = new Vector3(normalScale.x*scale,normalScale.y*scale,1);
            }
        }

        private void Awake()
        {
            RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.Create();
            l2DCamera = Instantiate(l2DCameraPrefab, (Vector3)modelPosition - Vector3.forward * 10, Quaternion.identity);
            l2DCamera.targetTexture = renderTexture;
            image.texture = renderTexture;
        }
        private void OnDestroy()
        {
            RenderTexture renderTexture = l2DCamera.targetTexture;
            if(l2DCamera) Destroy(l2DCamera.gameObject);
            renderTexture.Release();
            HideModel();
        }
    }
}