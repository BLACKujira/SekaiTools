using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Rendering;
using System.Collections;
using System.Windows.Forms;
using UnityEngine;
using Live2D.Cubism.Viewer;
using Live2D.Cubism.Core;
using System.Collections.Generic;
using System.IO;
using SekaiTools.UI.L2DModelSelect;
using System;
using Object = UnityEngine.Object;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// 模型读取器，附带储存模型的功能，只允许同时存在一个
    /// </summary>
    public class L2DModelLoader : MonoBehaviour
    {
        public InbuiltModelSet inbuiltModelSet;
        public InbuiltAnimationSet inbuiltAnimationSet;
        public InbuiltImageData modelPreviews;

        public static L2DModelLoader l2DModelLoader;
        public static InbuiltModelSet InbuiltModelSet => l2DModelLoader.inbuiltModelSet;
        public static InbuiltAnimationSet InbuiltAnimationSet => l2DModelLoader.inbuiltAnimationSet;
        public static InbuiltImageData ModelPreviews => l2DModelLoader.modelPreviews;

        protected List<LocalModelMeta> localModelMetas = new List<LocalModelMeta>();

        IEnumerator ILoadModel(L2DModelLoaderObjectBase l2DModelLoaderObjectBase)
        {
            yield return l2DModelLoaderObjectBase;
        }

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
        public static string ModelFolder => $"{EnvPath.Assets}/live2d/model";

        private void Awake()
        {
            l2DModelLoader = this;
            if(Directory.Exists(ModelFolder))
            {
                AddLocalModels(ModelFolder);
            }
        }

        public static L2DModelLoaderObjectBase LoadModel(string modelName, Action<SekaiLive2DModel> onComplete = null)
        {
            foreach (var sekaiLive2DModel in l2DModelLoader.inbuiltModelSet.inbuiltModelPrefabs)
            {
                if(sekaiLive2DModel.name.Equals(modelName))
                {
                    L2DModelLoaderObjectInbuilt l2DModelLoaderObject = new L2DModelLoaderObjectInbuilt();
                    l2DModelLoader.StartCoroutine(
                        l2DModelLoaderObject.LoadModel(sekaiLive2DModel,onComplete));
                    return l2DModelLoaderObject;
                }
            }

            foreach (var localModelMeta in l2DModelLoader.localModelMetas)
            {
                if(localModelMeta.ModelName.Equals(modelName))
                {
                    L2DModelLoaderObjectFile l2DModelLoaderObject = new L2DModelLoaderObjectFile();
                    l2DModelLoader.StartCoroutine(
                        l2DModelLoaderObject.LoadModel(localModelMeta.modelPath,onComplete));
                    return l2DModelLoaderObject;
                }
            }

            return null;
        }

        public static L2DModelLoaderObjectBase LoadModel(SelectedModelInfo selectedModelInfo, Action<SekaiLive2DModel> onComplete = null)
        {
            return LoadModel(selectedModelInfo.modelName, (model) =>
            {
                if(model!=null)
                    model.AnimationSet = InbuiltAnimationSet.GetAnimationSet(selectedModelInfo.animationSet);
                onComplete?.Invoke(model);
            });
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
                    modelInfo.ifInbuilt = false;
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
            l2DModelLoader.localModelMetas.Sort((x,y)=>x.ModelName.CompareTo(y.ModelName));
        }

        public static void AddLocalModels(string modelFolder)
        {
            List<string> files = new List<string>();
            foreach (var dir in Directory.GetDirectories(modelFolder))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (file.EndsWith(".model3.json"))
                        files.Add(file);
                }
            }
            foreach (var file in files)
            {
                if(!HasModel(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(file))))
                    AddLocalModel(file);
            }
        }

        public static bool HasModel(string modelName)
        {
            foreach (var cubismModel in l2DModelLoader.inbuiltModelSet.inbuiltModelPrefabs)
            {
                if (cubismModel.name.Equals(modelName))
                    return true;
            }

            foreach (var localModelMeta in l2DModelLoader.localModelMetas)
            {
                if (localModelMeta.ModelName.Equals(modelName))
                    return true;
            }

            return false;
        }

        public static Sprite GetPreview(string modelName)
        {
            Sprite sprite = ModelPreviews.GetValue(modelName);
            return sprite;
        }

        public static SelectedModelInfo GetDefaultModel(int characterId)
        {
            string selectedModel = null;
            foreach (var modelName in ModelList)
            {
                if (ConstData.IsLive2DModelOfCharacter(modelName) == characterId)
                {
                    selectedModel = modelName;
                    break;
                }
            }

            if(characterId>=27&&characterId<=56)
            {
                foreach (var modelName in ModelList)
                {
                    if (ConstData.IsLive2DModelOfCharacter(modelName,false) == characterId)
                    {
                        selectedModel = modelName;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(selectedModel))
                return SelectedModelInfo.Empty;

            L2DAnimationSet l2DAnimationSet = InbuiltAnimationSet.GetAnimationSetByModelName(selectedModel);
            if (!l2DAnimationSet)
                return SelectedModelInfo.Empty;

            return new SelectedModelInfo(selectedModel, l2DAnimationSet.name);
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
        public IEnumerator LoadModel(string path,Action<SekaiLive2DModel> onComplete = null)
        {
            CubismModel3Json modelJson = CubismModel3Json.LoadAtPath(path, CubismViewerIo.LoadAsset);
            CubismModel model = modelJson.ToModel(true);
            yield return 1;
            CubismModel oldModel = model;
            model = Object.Instantiate(model);
            model.name = oldModel.name;
            Object.Destroy(oldModel.gameObject);
            yield return 1;

            SekaiLive2DModel sekaiLive2DModel = model.gameObject.AddComponent<SekaiLive2DModel>();
            yield return 1;

            sekaiLive2DModel.Initialize();

            yield return 1;
            Transform drawables = model.transform.GetChild(2);
            int drawableCount = drawables.childCount;
            for (int i = 0; i < drawableCount; i++)
            {
                drawables.GetChild(i).gameObject.layer = 8;
            }
            model.gameObject.SetActive(true);

            //yield return new WaitForSeconds(Mathf.Infinity);
            yield return 1;

            model.gameObject.SetActive(false);

            this.model = sekaiLive2DModel;
            _keepWaiting = false;

            onComplete?.Invoke(sekaiLive2DModel);
        }
    }

    public class L2DModelLoaderObjectInbuilt : L2DModelLoaderObjectBase
    {
        public IEnumerator LoadModel(CubismModel prefab,Action<SekaiLive2DModel> onComplete = null)
        {
            model = Object.Instantiate(prefab).gameObject.AddComponent<SekaiLive2DModel>();
            model.name = prefab.name;

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

            onComplete?.Invoke(model);
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