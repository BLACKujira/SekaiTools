/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Rendering;
using System.Windows.Forms;
using UnityEngine;


using EventSystem = UnityEngine.EventSystems.EventSystem;
using Screen = UnityEngine.Screen;


namespace Live2D.Cubism.Viewer
{
    /// <summary>
    /// Unity-based viewer.
    /// </summary>
    public sealed class CubismViewer : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Handles model load events.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="model">New model.</param>
        public delegate void NewModelHandler(CubismViewer sender, CubismModel model);

        /// <summary>
        /// Handles non-model JSON file drops.
        /// </summary>
        /// <param name="sender">Events source.</param>
        /// <param name="absolutePath">Absolute path to dropped file.</param>
        public delegate void FileDropHandler(CubismViewer sender, string absolutePath);


        /// <summary>
        /// Called when new model loaded.
        /// </summary>
        public event NewModelHandler OnNewModel;

        /// <summary>
        /// Called on file drop.
        /// </summary>
        public event FileDropHandler OnFileDrop;

        #endregion

        /// <summary>
        /// <see cref="Camera"/> backing field.
        /// </summary>
        [SerializeField]
        private Camera _camera;

        /// <summary>
        /// Viewer camera.
        /// </summary>
        public Camera Camera
        {
            get { return _camera;  }
        }

        /// <summary>
        /// Model JSON of current model.
        /// </summary>
        public CubismModel3Json ModelJson { get; set; }

        /// <summary>
        /// Current model.
        /// </summary>
        public CubismModel Model { get; set; }

        /// <summary>
        /// File dialog.
        /// </summary>
        private OpenFileDialog FileDialog { get; set; }

        /// <summary>
        /// Config.
        /// </summary>
        private CubismViewerConfig Config { get; set; }


        /// <summary>
        /// Shows dialog for opening files.
        /// </summary>
        public void ShowFileDialog()
        {
            // HACK Deselect active UI element.
            EventSystem.current.SetSelectedGameObject(null);


            // Pre-filter for motions in case a model is loaded.
            if (Model != null)
            {
                FileDialog.FilterIndex = 2;
            }


            // Return unless a file was selected.
            if (FileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            var absoluteFilePath = FileDialog.FileName;


            // Store path.
            Config.LastFileDialogPath = absoluteFilePath;


            // Handle model JSON files.
            if (absoluteFilePath.EndsWith(".model3.json"))
            {
                LoadModel(absoluteFilePath);


                // Guess best zoom and position.
                // TODO Improve if necessary.
                //var modelBounds = Model
                //    .GetComponent<CubismRenderController>()
                //    .Renderers
                //    .GetMeshRendererBounds();


                //Camera.transform.position = new Vector3(0f, 0f, -10f);
                //Camera.orthographicSize = modelBounds.extents.y * 1.1f;


                return;
            }


            // Trigger event for other files.
            if (OnFileDrop != null)
            {
                OnFileDrop(this, absoluteFilePath);
            }
        }

       public CubismModel LoadModel(string absoluteFilePath)
        {
            var lastModel = Model;


            // Load model.
            ModelJson = CubismModel3Json.LoadAtPath(absoluteFilePath, CubismViewerIo.LoadAsset);
            Model = ModelJson.ToModel();


            // Trigger event.
            if (OnNewModel != null)
            {
                OnNewModel(this, Model);
            }


            // Remove previous model.
            if (lastModel != null)
            {
                Destroy(lastModel.gameObject);
            }

            return Model;
        }

        #region Unity Event Handling

        /// <summary>
        /// Called by Unity. Initializes viewer.
        /// </summary>
        private void Awake()
        {
            // Load config.
            Config = CubismViewerIo.LoadConfig<CubismViewerConfig>();


            // Initialize screen size.
            //Screen.SetResolution(Config.ScreenWidth, Config.ScreenHeight, false);


            // Initialize file dialog.
            FileDialog = new OpenFileDialog();

            //if (!string.IsNullOrEmpty(Config.LastFileDialogPath))
            //{
             //   FileDialog.InitialDirectory = CubismViewerIo.GetDirectoryName(Config.LastFileDialogPath);
            //}

            FileDialog.Filter = "Models (*.model3.json)|*.model3.json|Motions (*.motion3.json)|*.motion3.json|Others (*.*)|*.*";
            FileDialog.FilterIndex = 1;
            FileDialog.RestoreDirectory = true;
        }


        /// <summary>
        /// Called by Unity. Stores config.
        /// </summary>
        private void OnDestroy()
        {
            //Config.ScreenWidth = Screen.width;
            //Config.ScreenHeight = Screen.height;


            //CubismViewerIo.SaveConfig(Config);
        }

        #endregion
    }
}