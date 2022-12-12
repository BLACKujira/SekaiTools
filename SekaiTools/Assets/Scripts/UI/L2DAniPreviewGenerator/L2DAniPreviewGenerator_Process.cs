using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.Downloader;
using SekaiTools.Live2D;
using System.IO;

namespace SekaiTools.UI.L2DAniPreviewGenerator
{
    /// <summary>
    /// 控制生成L2D动画预览的脚本
    /// </summary>
    public class L2DAniPreviewGenerator_Process : MonoBehaviour
    {
        [Header("Components")]
        public L2DAniPreviewGenerator l2DAniPreviewGenerator;
        public L2DAniPreviewGenerator_Card card;
        public Downloader_PerecntBar perecntBar;
        [Header("Settings")]
        public float delay = 0.5f;
        public Texture2D mask;

        SekaiLive2DModel cloneModel;
        SekaiLive2DModel originModel;

        Vector2Int originScreenSize = new Vector2Int(1920,1080);
        bool ifFullScreen;


        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize()
        {
            //储存窗口状态
            originScreenSize.x = Screen.width;
            originScreenSize.y = Screen.height;
            ifFullScreen = Screen.fullScreen;

            //暂时进入固定分辨率（以后会移除并改用固定大小UI）
            Screen.SetResolution(1920, 1080, true);

            originModel = l2DAniPreviewGenerator.l2DController.model;
            originModel.gameObject.SetActive(false);

            string savePath = Path.Combine(l2DAniPreviewGenerator.savePathInputField.text, l2DAniPreviewGenerator.animationSet.name);
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        }

        /// <summary>
        /// 开始处理
        /// </summary>
        public void StartProcess()
        {
            Initialize();
            StartCoroutine(IProcess());
        }
        IEnumerator IProcess()
        {
            float count = 0;

            cloneModel = Instantiate(originModel);
            cloneModel.AnimationSet = l2DAniPreviewGenerator.animationSet;
            cloneModel.StopAutoBreathAndBlink();
            cloneModel.gameObject.SetActive(true);
            l2DAniPreviewGenerator.l2DController.ShowModel(cloneModel);
            l2DAniPreviewGenerator.ChangeModeToFacial();
            yield return new WaitForSeconds(delay);
            foreach (var animationClip in l2DAniPreviewGenerator.animationSet.facialPack)
            {
                cloneModel.StopAllAnimation();
                cloneModel.PlayAnimation(null, animationClip, Mathf.Infinity);
                perecntBar.priority = count++ / l2DAniPreviewGenerator.animationSet.animationClips.Count;
                perecntBar.info = animationClip.name;
                string[] nameArray = animationClip.name.Split('_');
                card.SetData(nameArray[2], nameArray[1],true);

                yield return new WaitForSeconds(delay);
                yield return new WaitForEndOfFrame();
                TakeShootAndSave(animationClip);
            }

            Destroy(cloneModel.gameObject);

            cloneModel = Instantiate(originModel);
            cloneModel.AnimationSet = l2DAniPreviewGenerator.animationSet;
            cloneModel.StopAutoBreathAndBlink();
            cloneModel.gameObject.SetActive(true);

            l2DAniPreviewGenerator.l2DController.ShowModel(cloneModel);
            l2DAniPreviewGenerator.ChangeModeToMotion();
            yield return new WaitForSeconds(delay);
            foreach (var animationClip in l2DAniPreviewGenerator.animationSet.motionPack)
            {
                cloneModel.StopAllAnimation();
                cloneModel.PlayAnimation(animationClip, null, Mathf.Infinity);
                perecntBar.priority = count++ / l2DAniPreviewGenerator.animationSet.animationClips.Count;
                perecntBar.info = animationClip.name;
                string[] nameArray = animationClip.name.Split('-');
                card.SetData(nameArray[1], nameArray[2]);

                yield return new WaitForSeconds(delay);
                yield return new WaitForEndOfFrame();
                TakeShootAndSave(animationClip);
            }

            perecntBar.priority = 1;
            perecntBar.info = "完成";
        }

        /// <summary>
        /// 捕捉屏幕，处理，保存
        /// </summary>
        /// <param name="animationClip"></param>
        private void TakeShootAndSave(AnimationClip animationClip)
        {
            Texture2D texture2D = Resize(SetAlpha(TakeShoot(new Vector2Int(330, 520))));
            byte[] png = texture2D.EncodeToPNG();

            string savePath = Path.Combine(l2DAniPreviewGenerator.savePathInputField.text, l2DAniPreviewGenerator.animationSet.name, animationClip.name + ".png");

            File.WriteAllBytes(savePath, png);
        }

        /// <summary>
        /// 关闭窗口时恢复分辨率
        /// </summary>
        public void OnExit()
        {
            Screen.SetResolution(originScreenSize.x, originScreenSize.y, ifFullScreen);
            l2DAniPreviewGenerator.l2DController.ShowModel(originModel);
            if (cloneModel) Destroy(cloneModel.gameObject);
            l2DAniPreviewGenerator.ChangeModeToMotion();
            l2DAniPreviewGenerator.Refresh();
            card.ResetTextAndColor();
        }

        /// <summary>
        /// 捕捉屏幕图像
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Texture2D TakeShoot(Vector2Int size)
        {
            Texture2D frame = new Texture2D(size.x, size.y);
            frame.ReadPixels(new Rect(Screen.width / 2 - size.x / 2, Screen.height / 2 - size.y / 2, size.x, size.y), 0, 0);
            frame.Apply();
            return frame;
        }

        /// <summary>
        /// 缩小图片大小为1/2
        /// </summary>
        /// <param name="texture2D"></param>
        /// <returns></returns>
        Texture2D Resize(Texture2D texture2D)
        {
            Texture2D newTex = new Texture2D(texture2D.width / 2, texture2D.height / 2);
            for (int y = 0; y < newTex.height; y++)
            {
                for (int x = 0; x < newTex.width; x++)
                {
                    List<Color> colors = new List<Color>();
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            colors.Add(texture2D.GetPixel(x * 2 + i, y * 2 + j));
                        }
                    }
                    Color averColor = colors[0];
                    for (int i = 1; i < colors.Count; i++)
                    {
                        averColor = Color.Lerp(averColor, colors[i], 1f / (i + 1));
                    }
                    newTex.SetPixel(x, y, averColor);
                }
            }
            newTex.Apply();
            return newTex;
        }

        /// <summary>
        /// 将mask的alpha值应用于贴图
        /// </summary>
        /// <param name="texture2D"></param>
        /// <returns></returns>
        Texture2D SetAlpha(Texture2D texture2D)
        {
            for (int x = 0; x < texture2D.width; x++)
            {
                for (int y = 0; y < texture2D.height; y++)
                {
                    Color color = texture2D.GetPixel(x, y);
                    color.a = mask.GetPixel(x, y).a;
                    texture2D.SetPixel(x, y, color);
                }
            }
            return texture2D;
        }

        /// <summary>
        /// 停止处理
        /// </summary>
        public void StopProcess()
        {
            StopAllCoroutines();
            OnExit();
        }
    }
}