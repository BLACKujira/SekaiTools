using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(RectTransform))]
    public class HDRUIController : MonoBehaviour
    {
        [Header("Components")]
        public Transform instantiateParent;
        public Vector2 instantiatePosition = new Vector2(-1024, 0);
        [Header("Settings")]
        public float cameraSize = 1.0f;
        public float cameraPositionZ = -10f;
        [Header("Prefab")]
        public GameObject instantiateObjectPrefab;
        public Camera cameraPrefab;

        GameObject instantiateObject;
        public GameObject InstantiateObject => instantiateObject;
        Camera instantiateCamera;
        public Camera InstantiateCamera => instantiateCamera;
        RenderTexture renderTexture;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }
        RawImage rawImage;
        public RawImage RawImage
        { 
            get 
            {
                if (rawImage == null) rawImage = GetComponent<RawImage>();
                return rawImage; 
            } 
        }

        private void Awake()
        {
            instantiateObject = Instantiate(instantiateObjectPrefab, instantiatePosition, Quaternion.identity, instantiateParent);
            Vector3 instantiatePositionCam = (Vector3)instantiatePosition;
            instantiatePositionCam.z = cameraPositionZ;
            instantiateCamera = Instantiate(cameraPrefab, instantiatePositionCam, Quaternion.identity, instantiateParent);

            renderTexture = new RenderTexture((int)RectTransform.sizeDelta.x, (int)RectTransform.sizeDelta.y, 0, RenderTextureFormat.ARGBFloat);
            instantiateCamera.targetTexture = renderTexture;
            RawImage.texture = renderTexture;
            instantiateCamera.orthographicSize = cameraSize;
        }

        private void OnDestroy()
        {
            if (instantiateObject != null)
                Destroy(instantiateObject);
            if (instantiateCamera != null)
                Destroy(instantiateCamera.gameObject);
            if (renderTexture != null)
                Destroy(renderTexture);
        }
    }
}