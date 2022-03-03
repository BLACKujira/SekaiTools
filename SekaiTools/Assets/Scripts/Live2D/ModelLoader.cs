using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using Live2D.Cubism.Viewer;
using Live2D.Cubism.Core;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// 模型读取器，附带储存模型的功能，只允许同时存在一个
    /// </summary>
    public class ModelLoader : MonoBehaviour
    {
        public CubismViewer cubismViewer;

        public InbuiltAnimationSet inbuiltAnimationSet;

        public List<SekaiLive2DModel> models = new List<SekaiLive2DModel>();

        public static ModelLoader modelLoader;

        private void Awake()
        {
            modelLoader = this;
            cubismViewer.OnNewModel += (CubismViewer sender, CubismModel model) =>{
                SekaiLive2DModel sekaiLive2DModel = model.gameObject.AddComponent<SekaiLive2DModel>();
                sekaiLive2DModel.Initialize();
                int id = ConstData.IsModelOfCharacter(model.name);
                if (id == 0) sekaiLive2DModel.AnimationSet = null;
                else sekaiLive2DModel.AnimationSet = inbuiltAnimationSet.l2DAnimationSets[id];
            };
        }

        public IEnumerator LoadModel(string path)
        {
            SekaiLive2DModel model = cubismViewer.LoadModel(path).GetComponent<SekaiLive2DModel>();
            model.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            model = Instantiate(model);
            model.name = cubismViewer.Model.name;
            models.Add(model);
            Transform drawables = model.transform.GetChild(2);
            int drawableCount = drawables.childCount;
            for (int i = 0; i < drawableCount; i++)
            {
                drawables.GetChild(i).gameObject.layer = 8;
            }
            model.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            model.gameObject.SetActive(false);
        }
    }
}