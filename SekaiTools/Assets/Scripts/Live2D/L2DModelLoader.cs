using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Rendering;
using System.Collections;
using System.Windows.Forms;
using UnityEngine;
using Live2D.Cubism.Viewer;
using Live2D.Cubism.Core;
using System.Collections.Generic;
using System.IO;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// 模型读取器，附带储存模型的功能，只允许同时存在一个
    /// </summary>
    public class L2DModelLoader : MonoBehaviour
    {
        public InbuiltModelSet inbuiltModelSet;
        public InbuiltAnimationSet inbuiltAnimationSet;

        public static L2DModelLoader l2DModelLoader;

        protected List<LocalModelMeta> localModelMetas = new List<LocalModelMeta>();

        public static string[] ModelList
        {
            get
            {
                List<string> modelList = new List<string>();
                foreach (var sekaiLive2DModel in l2DModelLoader.inbuiltModelSet.inbuiltModelPrefabs)
                    modelList.Add(sekaiLive2DModel.name);
                foreach (var localModelMeta in l2DModelLoader.localModelMetas)
                    modelList.Add(localModelMeta.ModelName);
                return modelList.ToArray();
            }
        }

        private void Awake()
        {
            l2DModelLoader = this;
        }

        public static L2DModelLoaderObjectBase LoadModel(string modelName)
        {
            foreach (var sekaiLive2DModel in l2DModelLoader.inbuiltModelSet.inbuiltModelPrefabs)
            {
                if(sekaiLive2DModel.name.Equals(modelName))
                {
                    L2DModelLoaderObjectInbuilt l2DModelLoaderObject = new L2DModelLoaderObjectInbuilt();
                    l2DModelLoader.StartCoroutine(
                        l2DModelLoaderObject.LoadModel(sekaiLive2DModel));
                    return l2DModelLoaderObject;
                }
            }

            foreach (var localModelMeta in l2DModelLoader.localModelMetas)
            {
                if(localModelMeta.ModelName.Equals(modelName))
                {
                    L2DModelLoaderObjectFile l2DModelLoaderObject = new L2DModelLoaderObjectFile();
                    l2DModelLoader.StartCoroutine(
                        l2DModelLoaderObject.LoadModel(localModelMeta.modelPath));
                    return l2DModelLoaderObject;
                }
            }

            return null;
        }

        public static ModelInfo GetModelInfo(string modelName)
        {
            ModelInfo modelInfo = new ModelInfo(modelName);
            foreach (var cubismModel in l2DModelLoader.inbuiltModelSet.inbuiltModelPrefabs)
            {
                if(cubismModel.name.Equals(modelName))
                {
                    modelInfo.ifInbuilt = true;
                    return modelInfo;
                }    
            }
            foreach (var localModelMeta in l2DModelLoader.localModelMetas)
            {
                if(localModelMeta.ModelName.Equals(modelName))
                {
                    modelInfo.ifInbuilt = true;
                    modelInfo.modelPath = localModelMeta.modelPath;
                    return modelInfo;
                }    
            }
            return null;
        }

        public static void RemoveModel(string modelName)
        {
            foreach (var localModelMeta in l2DModelLoader.localModelMetas)
            {
                if (localModelMeta.ModelName.Equals(modelName))
                {
                    l2DModelLoader.localModelMetas.Remove(localModelMeta);
                    return;
                }
            }
            //TODO 错误检查
        }

        public static void AddLocalModel(string modelPath)
        {
            l2DModelLoader.localModelMetas.Add(new LocalModelMeta(modelPath));
        }
    }

    [System.Serializable]
    public class LocalModelMeta
    {
        string modelName;
        public string ModelName
        {
            get
            {
                if (string.IsNullOrEmpty(modelName))
                    modelName = Path.GetFileNameWithoutExtension(
                        Path.GetFileNameWithoutExtension(modelPath));
                return modelName;
            }
        }

        public string modelPath;

        public LocalModelMeta(string modelPath)
        {
            this.modelPath = modelPath;
        }
    }

    public class LocalModelMetaSave
    {
        public LocalModelMeta[] localModelDatas;
    }

    public abstract class L2DModelLoaderObjectBase : CustomYieldInstruction
    {
        protected SekaiLive2DModel model = null;
        protected bool _keepWaiting = true;
        protected System.Exception exception = null;

        public SekaiLive2DModel Model => model;
        public override bool keepWaiting => _keepWaiting;
        public System.Exception Exception => exception;
    }

    public class L2DModelLoaderObjectFile : L2DModelLoaderObjectBase
    {
        public IEnumerator LoadModel(string path)
        {
            CubismModel3Json modelJson = CubismModel3Json.LoadAtPath(path, CubismViewerIo.LoadAsset);
            CubismModel model = modelJson.ToModel();

            SekaiLive2DModel sekaiLive2DModel = model.gameObject.AddComponent<SekaiLive2DModel>();
            yield return 1;

            sekaiLive2DModel.Initialize();

            Transform drawables = model.transform.GetChild(2);
            int drawableCount = drawables.childCount;
            for (int i = 0; i < drawableCount; i++)
            {
                drawables.GetChild(i).gameObject.layer = 8;
            }
            model.gameObject.SetActive(true);

            //yield return new WaitForSeconds(Mathf.Infinity);
            yield return 1;

            //model.gameObject.SetActive(false);

            this.model = sekaiLive2DModel;
            _keepWaiting = false;
        }
    }

    public class L2DModelLoaderObjectInbuilt : L2DModelLoaderObjectBase
    {
        public IEnumerator LoadModel(CubismModel prefab)
        {
            Debug.Log(1);
            model = Object.Instantiate(prefab).gameObject.AddComponent<SekaiLive2DModel>();

            yield return 1;
            model.Initialize();

            yield return 1;

            Transform drawables = model.transform.GetChild(2);
            int drawableCount = drawables.childCount;
            for (int i = 0; i < drawableCount; i++)
            {
                drawables.GetChild(i).gameObject.layer = 8;
            }
            model.gameObject.SetActive(true);

            yield return 1;

            model.gameObject.SetActive(false);

            _keepWaiting = false;
            yield break;
        }
    }

    public class ModelInfo
    {
        public readonly string modelName;

        public ModelInfo(string modelName)
        {
            this.modelName = modelName;
        }

        public bool ifInbuilt;
        public string modelPath;
    }
}