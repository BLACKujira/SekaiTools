using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera _backGroundCamera;
        [SerializeField] Camera _spineCamera;

        public static CameraController cameraController;
        public static Camera BackGroundCamera => cameraController._backGroundCamera;
        public static Camera SpineCamera => cameraController._spineCamera;


        private void Awake()
        {
            cameraController = this;
        }
    }
}