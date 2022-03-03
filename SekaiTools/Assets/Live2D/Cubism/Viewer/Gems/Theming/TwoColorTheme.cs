/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using System;
using UnityEngine;


namespace Live2D.Cubism.Viewer.Gems.Theming
{
    /// <summary>
    /// 2-color theme
    /// </summary>
    [Serializable]
    public struct TwoColorTheme
    {
        /// <summary>
        /// Primary color.
        /// </summary>
        /// <remarks>
        /// This color will be used for the background, too.
        /// </remarks>
        public Color Primary;

        /// <summary>
        /// Secondary color.
        /// </summary>
        public Color Secondary;
    }
}
