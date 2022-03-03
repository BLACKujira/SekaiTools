/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using System;
using UnityEngine;


namespace Live2D.Cubism.Viewer
{
    /// <summary>
    /// Viewer config file.
    /// </summary>
    [Serializable]
    public class CubismViewerConfig
    {
        /// <summary>
        /// Screen width.
        /// </summary>
        [SerializeField]
        public int ScreenWidth = 540;

        /// <summary>
        /// Screen height.
        /// </summary>
        [SerializeField]
        public int ScreenHeight = 720;

        /// <summary>
        /// Last path gotten via file dialog.
        /// </summary>
        [SerializeField]
        public string LastFileDialogPath;
    }
}
