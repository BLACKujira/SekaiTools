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
    /// Keyobard hotkey.
    /// </summary>
    [Serializable]
    public struct CubismViewerMouseScrollHotkey : ICubismViewerHotkey
    {
        /// <summary>
        /// Key condition.
        /// </summary>
        [SerializeField]
        public KeyCode Modifier;


        /// <summary>
        /// Evaluates hotkey.
        /// </summary>
        /// <returns><see langword="true"/> if hotkey pressed; <see langword="false"/> otherwise.</returns>
        public bool Evaluate()
        {
            if (Modifier != KeyCode.None && !Input.GetKey(Modifier))
            {
                return false;
            }


            return Input.mouseScrollDelta != Vector2.zero;
        }

        /// <summary>
        /// Evaluates if hotkey just pressed.
        /// </summary>
        /// <returns><see langword="true"/> if hotkey pressed; <see langword="false"/> otherwise.</returns>
        public bool EvaluateJust()
        {
            return Evaluate();
        }
    }
}
