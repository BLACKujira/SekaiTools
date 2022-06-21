using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SekaiTools.Spine.SpineControllerTypeA_DefaultSettings;
using UnityEngine.Events;
using SekaiTools.UI.BackGround;

namespace SekaiTools.Spine
{
    public class SpineControllerTypeA : MonoBehaviour
    {
        public UnityEvent onModelChange;

        [System.Serializable]
        public class ModelPair
        {
            public string Name { get => model.name; }
            public SkeletonAnimation Model
            {
                get
                {
                    if (!model_r) return model;
                    else
                    {
                        if (IfFlip) return model_r;
                        else return model;
                    }
                }
            }

            public bool IfFlip { get => ifFlip; }
            public AtlasAssetPair AtlasAssetPair 
            {
                get
                {
                    AtlasAsset atlasAsset = model.SkeletonDataAsset.atlasAssets[0];
                    AtlasAsset atlasAsset_r = model_r == null ? null: model_r.SkeletonDataAsset.atlasAssets[0];
                    return new AtlasAssetPair(Name, atlasAsset, atlasAsset_r);
                }
            }
            public int OrderInLayer
            {
                get => model.GetComponent<MeshRenderer>().sortingOrder;
                set
                {
                    model.GetComponent<MeshRenderer>().sortingOrder = value;
                    if (model_r) model_r.GetComponent<MeshRenderer>().sortingOrder = value;
                }
            }
            public float animationProgress = 0;


            public SkeletonAnimation model = null;
            public SkeletonAnimation model_r = null;
            bool ifFlip = false;

            /// <summary>
            /// 生成模型对，翻转时镜像模型
            /// </summary>
            /// <param name="model"></param>
            public ModelPair(SkeletonAnimation model)
            {
                this.model = model;
            }

            /// <summary>
            /// 生成模型对，翻转时切换为另一个模型
            /// </summary>
            /// <param name="model"></param>
            /// <param name="model_r"></param>
            public ModelPair(SkeletonAnimation model, SkeletonAnimation model_r) : this(model)
            {
                this.model_r = model_r;
                if (model_r.gameObject.activeSelf)
                    model_r.gameObject.SetActive(false);
                model_r.initialFlipX = true;
                model_r.Initialize(true);
            }

            public void SetFlip(bool value)
            {
                if (ifFlip == value) return;
                ifFlip = value;

                if (!model_r)
                {
                    //TODO 改为使用scale翻转
                    model.initialFlipX = value;
                    model.Initialize(true);
                }
                else
                {
                    if (value)
                    {
                        model.gameObject.SetActive(false);
                        model_r.transform.CopyComponentValues(model.transform);
                        model_r.gameObject.SetActive(true);
                    }
                    else
                    {
                        model_r.gameObject.SetActive(false);
                        model.transform.CopyComponentValues(model_r.transform);
                        model.gameObject.SetActive(true);
                    }
                }
            }

            public void DestroyThis()
            {
                Destroy(model.gameObject);
                if (model_r) Destroy(model_r.gameObject);
            }
        }

        [Header("Runtime")]
        public List<ModelPair> models = new List<ModelPair>();
        [Header("Settings")]
        public SpineControllerTypeA_DefaultSettings defaultSettings;
        public InbuiltSpineModelSet spineModelSet;
        [Header("Prefab")]
        public SkeletonAnimation baseModelPrefab;

        public void ResetPosition()
        {
            for (int i = 0; i < models.Count; i++)
            {
                DefaultSettingItem defaultSettingItem = defaultSettings.GetDefaultSetting(i,models.Count);
                models[i].Model.transform.position = defaultSettingItem.position;
                models[i].SetFlip(defaultSettingItem.flipX);
            }
        }

        public ModelPair AddModel(AtlasAssetPair atlasAssetPair)
        {
            ModelPair modelPair = CreateModelPair(atlasAssetPair);
            models.Add(modelPair);
            onModelChange.Invoke();
            ApplySortingOrder();
            return modelPair;
        }

        public void RemoveModel(int index)
        {
            ModelPair modelPair = models[index];
            models.RemoveAt(index);
            modelPair.DestroyThis();
            ApplySortingOrder();
            onModelChange.Invoke();
        }

        public void RemoveModel(ModelPair modelPair)
        {
            RemoveModel(models.IndexOf(modelPair));
        }

        private ModelPair CreateModelPair(AtlasAssetPair atlasAssetPair)
        {
            ModelPair modelPair;
            SkeletonAnimation skeletonAnimation = CreateSekaiSpineModel(atlasAssetPair.atlasAsset);
            skeletonAnimation.name = atlasAssetPair.name;
            if (atlasAssetPair.atlasAsset_r != null)
            {
                SkeletonAnimation skeletonAnimation_r = CreateSekaiSpineModel(atlasAssetPair.atlasAsset_r);
                skeletonAnimation_r.name = atlasAssetPair.name + "_r";
                modelPair = new ModelPair(skeletonAnimation, skeletonAnimation_r);
            }
            else
            {
                modelPair = new ModelPair(skeletonAnimation);
            }

            return modelPair;
        }

        private SkeletonAnimation CreateSekaiSpineModel(AtlasAsset atlasAsset)
        {
            SkeletonAnimation skeletonAnimation = Instantiate(baseModelPrefab, Vector3.zero, Quaternion.identity, transform);
            SkeletonDataAsset newSkeletonDataAsset = Instantiate(baseModelPrefab.skeletonDataAsset);
            newSkeletonDataAsset.atlasAssets[0] = atlasAsset;
            skeletonAnimation.skeletonDataAsset = newSkeletonDataAsset;


            if (skeletonAnimation.skeletonDataAsset != null)
            {
                foreach (AtlasAsset aa in skeletonAnimation.skeletonDataAsset.atlasAssets)
                {
                    if (aa != null)
                        aa.Clear();
                }
                skeletonAnimation.skeletonDataAsset.Clear();
            }
            skeletonAnimation.Initialize(true);
            return skeletonAnimation;
        }

        public void ReplaceModel(AtlasAssetPair atlasAssetPair,int index)
        {
            ModelPair newModelPair = CreateModelPair(atlasAssetPair);
            newModelPair.Model.transform.CopyComponentValues(models[index].Model.transform);
            models[index].DestroyThis();
            models[index] = newModelPair;
            onModelChange.Invoke();
            ApplySortingOrder();
        }

        public void ClearModel()
        {
            foreach (var modelPair in models)
            {
                modelPair.DestroyThis();
            }
            models = new List<ModelPair>();
            onModelChange.Invoke();
        }

        private void OnDestroy()
        {
            foreach (var modelPair in models)
            {
                modelPair.DestroyThis();
            }
        }

        public void LoadData(SpineScene spineScene)
        {
            ClearModel();
            ModelPair[] sortingArray = new ModelPair[spineScene.spineObjects.Length];
            foreach (var spineObject in spineScene.spineObjects)
            {
                ModelPair modelPair = AddModel(spineModelSet.GetValue(spineObject.atlasAssetName));
                modelPair.Model.transform.position = spineObject.position;
                modelPair.Model.transform.rotation = Quaternion.Euler(spineObject.rotation);
                modelPair.Model.transform.localScale = spineObject.scale;
                modelPair.SetFlip(spineObject.ifFlip);

                sortingArray[spineObject.sortingOrder] = modelPair;
                global::Spine.TrackEntry trackEntry = modelPair.Model.AnimationState.SetAnimation(0, spineObject.animation, true);
                modelPair.Model.AnimationState.TimeScale = spineObject.animationSpeed;
                trackEntry.TrackTime = trackEntry.animationEnd * spineObject.animationProgress;
            }
            models = new List<ModelPair>(sortingArray);
            ApplySortingOrder();
        }

        public SpineScene GetSaveData()
        {
            SpineScene spineScene = new SpineScene();
            spineScene.spineLayerID = 1;//TODO 获取图层ID
            List<SpineScene.SpineObject> spineObjects = new List<SpineScene.SpineObject>();
            for (int i = 0; i < models.Count; i++)
            {
                ModelPair modelPair = models[i];
                SpineScene.SpineObject spineObject = new SpineScene.SpineObject();
                spineObject.atlasAssetName = modelPair.Name;
                spineObject.position = modelPair.Model.transform.position;
                spineObject.rotation = modelPair.Model.transform.rotation.eulerAngles;
                spineObject.scale = modelPair.Model.transform.localScale;
                spineObject.ifFlip = modelPair.IfFlip;
                spineObject.sortingOrder = modelPair.OrderInLayer;
                spineObject.animation = modelPair.Model.AnimationName;
                spineObject.animationSpeed = modelPair.Model.AnimationState.TimeScale;
                spineObject.animationProgress = modelPair.animationProgress;
                spineObjects.Add(spineObject);
            }
            spineScene.spineObjects = spineObjects.ToArray();
            return spineScene;
        }

        public void SetModel(SpineScene.SpineObject spineObject, int index)
        {
            ModelPair modelPair = models[index];
            SetModel(spineObject, modelPair);
        }

        public static void SetModel(SpineScene.SpineObject spineObject, ModelPair modelPair)
        {
            modelPair.Model.transform.position = spineObject.position;
            modelPair.Model.transform.rotation = Quaternion.Euler(spineObject.rotation);
            modelPair.Model.transform.localScale = spineObject.scale;
            modelPair.SetFlip(spineObject.ifFlip);

            if (!string.IsNullOrEmpty(spineObject.animation))
            {
                global::Spine.TrackEntry trackEntry = modelPair.Model.AnimationState.SetAnimation(0, spineObject.animation, true);
                trackEntry.TrackTime = trackEntry.AnimationEnd * spineObject.animationProgress;
            }
            modelPair.Model.AnimationState.TimeScale = spineObject.animationSpeed;
        }

        public void SetSortingOrder(int modelIndex,int sortingOrder)
        {
            if (modelIndex >= models.Count) throw new System.IndexOutOfRangeException();
            SetSortingOrder(models[modelIndex],sortingOrder);
        }
        public void SetSortingOrder(ModelPair modelPair, int sortingOrder)
        {
            if (!models.Remove(modelPair)) throw new ModelNotFoundException(modelPair.Name);
            models.Insert(sortingOrder, modelPair);
            ApplySortingOrder();
        }

        public void SortingOrderUp(ModelPair modelPair)
        {
            int index = models.IndexOf(modelPair);
            if (index == -1) throw new ModelNotFoundException(modelPair.Name);
            if (index == models.Count - 1) return;
            SwapSortingOrder(index, index + 1);
        }

        public void SortingOrderDown(ModelPair modelPair)
        {
            int index = models.IndexOf(modelPair);
            if (index == -1) throw new ModelNotFoundException(modelPair.Name);
            if (index == 0) return;
            SwapSortingOrder(index, index -1);
        }

        public void SwapSortingOrder(int indexAInSortingArray,int indexBInSortingArray)
        {
            ModelPair modelPair = models[indexAInSortingArray];
            models[indexAInSortingArray] = models[indexBInSortingArray];
            models[indexBInSortingArray] = modelPair;
            ApplySortingOrder();
        }

        void ApplySortingOrder()
        {
            foreach (var model in models)
            {
                model.OrderInLayer = models.IndexOf(model);
            }
        }

        [System.Serializable]
        public class ModelNotFoundException : System.Exception
        {
            public static string info = "在此控制器的列表中未找到模型";
            
            public ModelNotFoundException() : base(info) { }
            public ModelNotFoundException(string message) : base(message+info) { }
            public ModelNotFoundException(string message, System.Exception inner) : base(message, inner) { }
            protected ModelNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}