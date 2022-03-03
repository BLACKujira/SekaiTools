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
    public struct CubismViewerKeyboardHotkey: ICubismViewerHotkey
    {
        /// <summary>
        /// Auxuliary condition.
        /// </summary>
        [SerializeField]
        public KeyCode Modifier;

        /// <summary>
        /// Primary condition.
        /// </summary>
        [SerializeField]
        public KeyCode Key;


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

            if (Key != KeyCode.None && !Input.GetKey(Key))
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// Evaluates if hotkey just pressed.
        /// </summary>
        /// <returns><see langword="true"/> if hotkey pressed; <see langword="false"/> otherwise.</returns>
        public bool EvaluateJust()
        {
            if (Modifier != KeyCode.None && !Input.GetKey(Modifier))
            {
                return false;
            }

            if (!Input.GetKeyDown(Key))
            {
                return false;
            }


            return true;
        }
    }
}
