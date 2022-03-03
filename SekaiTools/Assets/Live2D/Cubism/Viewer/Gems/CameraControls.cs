/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using UnityEngine;


namespace Live2D.Cubism.Viewer.Gems
{
    /// <summary>
    /// Controls <see cref="CubismViewer.Camera"/>.
    /// </summary>
    public sealed class CameraControls : MonoBehaviour
    {
        /// <summary>
        /// Hotkey for moving camera.
        /// </summary>
        [SerializeField]
        CubismViewerMouseButtonHotkey MoveHotkey = new CubismViewerMouseButtonHotkey
        {
            Modifier = KeyCode.LeftControl,
            Button = 0
        };

        /// <summary>
        /// Scale to apply to mouse movement on move.
        /// </summary>
        [SerializeField, Range(-1f, 1f)]
        float MoveScale = 1.0f;

        /// <summary>
        /// Hotkey for zooming in.
        /// </summary>
        [SerializeField]
        CubismViewerMouseScrollHotkey ZoomHotkey = new CubismViewerMouseScrollHotkey
        {
            Modifier = KeyCode.LeftAlt
        };

        /// <summary>
        /// Scale to apply to mouse movement on zoom.
        /// </summary>
        [SerializeField, Range(-10f, 10f)]
        float ZoomScale = -5f;

        /// <summary>
        /// Maximum zoom in value.
        /// </summary>
        [SerializeField, Range(0.1f, 10f)]
        float ZoomInLimit = 0.1f;


        /// <summary>
        /// Target camera.
        /// </summary>
        private Camera Camera { get; set; }

        /// <summary>
        /// Last mouse position to compute mouse delta.
        /// </summary>
        private Vector3 LastMousePosition { get; set; }

        #region Unity Event Handling

        /// <summary>
        /// Called by Unity. Initializes instance.
        /// </summary>
        private void Start()
        {
            var viewer = GetComponent<CubismViewer>();


            // Fail silently in release.
            if (viewer == null)
            {
                Debug.LogWarning("Not attached to viewer!");


                return;
            }


            Camera = viewer.Camera;
        }

        /// <summary>
        /// Called by Unity. Updates controls.
        /// </summary>
        private void Update()
        {
            // Return if nothing to control.
            if (Camera == null)
            {
                return;
            }


            // Handle move.
            if (MoveHotkey.Evaluate())
            {
                var position = Camera.transform.position;


                position += ((LastMousePosition - Input.mousePosition) * Time.deltaTime * MoveScale);


                Camera.transform.position = position;
            }


            // Handle zoom.
            else if (ZoomHotkey.Evaluate())
            {
                var size = Camera.orthographicSize;


                size += (Input.mouseScrollDelta.y * Time.deltaTime * ZoomScale);


                // Apply limit.
                if (size < ZoomInLimit)
                {
                    size = ZoomInLimit;
                }


                Camera.orthographicSize = size;
            }


            // Keep track of mouse position.
            LastMousePosition = Input.mousePosition;
        }

        #endregion
    }
}
