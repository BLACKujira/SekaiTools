using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera _backGroundCamera;
        [SerializeField] Camera _spineCamera;

        Camera _mainCamera;

        public static CameraController cameraController;
        public static Camera MainCamera
        {
            get
            {
                if (!cameraController._mainCamera)
                    cameraController._mainCamera = cameraController.GetComponent<Camera>();
                return cameraController._mainCamera;
            }
        }
        public static Camera BackGroundCamera => cameraController._backGroundCamera;
        public static Camera SpineCamera => cameraController._spineCamera;


        private void Awake()
        {
            cameraController = this;
        }
    }
}