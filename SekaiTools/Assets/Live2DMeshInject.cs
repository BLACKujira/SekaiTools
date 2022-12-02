using Live2D.Cubism.Rendering;
using SekaiTools.Live2D;
using SekaiTools.UI.L2DModelPreview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DMeshInject : MonoBehaviour
{
    public string syncModelName;
    public L2DModelPreview l2DModelPreview;
    public List<MeshPair> meshPairs;
    public Vector2 positionOffset;
    [Header("Runtime")]
    public SekaiLive2DModel syncModel;

    [System.Serializable]
    public class MeshPair
    {
        public CubismRenderer getMeshFrom;
        public CubismRenderer injectMeshTo;
        public bool changeTexture = false;
    }

    private void Start()
    {
        l2DModelPreview.OnModelSet += 
            (model) => 
            {
                if (syncModel)
                    Destroy(syncModel);
                StartCoroutine(CoLoadModel(model));
            };

        l2DModelPreview.animationArea.OnPlayFacial += (facial) =>
        {
            if (syncModel != null)
                syncModel.PlayAnimation(null, facial);
        };

        l2DModelPreview.animationArea.OnPlayMotion += (motion) =>
        {
            if (syncModel != null)
                syncModel.PlayAnimation(motion, null);
        };
    }

    IEnumerator CoLoadModel(SekaiLive2DModel injectModel)
    {
        L2DModelLoaderObjectBase l2DModelLoaderObjectBase = L2DModelLoader.LoadModel(syncModelName);
        yield return l2DModelLoaderObjectBase;
        SekaiLive2DModel model = l2DModelLoaderObjectBase.Model;
        model.AnimationSet = injectModel.AnimationSet;
        syncModel = model;
    }

    private void LateUpdate()
    {
        if(syncModel)
        {
            syncModel.GetParameter("ParamPositionX").Value = positionOffset.x;
            syncModel.GetParameter("ParamPositionY").Value = positionOffset.y;
            foreach (var meshPair in meshPairs)
            {
                Mesh mesh = Instantiate(meshPair.getMeshFrom.Mesh);
                meshPair.injectMeshTo.GetComponent<MeshFilter>().mesh = mesh;
                if(meshPair.changeTexture)
                {
                    Texture2D mainTexture = meshPair.getMeshFrom.MainTexture;
                    meshPair.injectMeshTo.MainTexture = mainTexture;
                }
            }
        }

    }
}
