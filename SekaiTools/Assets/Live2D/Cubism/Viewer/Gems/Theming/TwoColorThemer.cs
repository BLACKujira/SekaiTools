/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using UnityEngine;


namespace Live2D.Cubism.Viewer.Gems.Theming
{
    /// <summary>
    /// 2-color themer.
    /// </summary>
    public sealed class TwoColorThemer : MonoBehaviour
    {
        /// <summary>
        /// Hotkey for triggering theme switches.
        /// </summary>
        [SerializeField]
        CubismViewerKeyboardHotkey NextThemeHotkey = new CubismViewerKeyboardHotkey
        {
            Modifier = KeyCode.LeftControl,
            Key = KeyCode.T
        };

        /// <summary>
        /// Themes.
        /// </summary>
        [SerializeField]
        public TwoColorTheme[] Themes =
        {
            new TwoColorTheme
            {
                Primary = Color.white,
                Secondary = Color.black
            }
        };

        /// <summary>
        /// Primary color UI elements.
        /// </summary>
        [SerializeField]
        public UnityEngine.UI.Graphic[] PrimaryColorElements;

        /// <summary>
        /// Secondary color UI elements.
        /// </summary>
        [SerializeField]
        public UnityEngine.UI.Graphic[] SecondaryColorElements;


        /// <summary>
        /// Currently active theme.
        /// </summary>
        private int ActiveTheme { get; set; }


        /// <summary>
        /// Switches to next scene.
        /// </summary>
        private void NextTheme()
        {
            ++ActiveTheme;


            if (ActiveTheme >= Themes.Length)
            {
                ActiveTheme = 0;
            }


            // Try update camera.
            var viewer = GetComponent<CubismViewer>();


            if (viewer != null)
            {
                viewer.Camera.backgroundColor = Themes[ActiveTheme].Primary;
            }


            // Update primary color elements.
            if (PrimaryColorElements != null)
            {
                for (var e = 0; e < PrimaryColorElements.Length; ++e)
                {
                    PrimaryColorElements[e].color = Themes[ActiveTheme].Primary;
                }
            }


            // Update secondary color elements.
            if (SecondaryColorElements != null)
            {
                for (var e = 0; e < SecondaryColorElements.Length; ++e)
                {
                    SecondaryColorElements[e].color = Themes[ActiveTheme].Secondary;
                }
            }
        }

        #region Unity Event Handling

        /// <summary>
        /// Called by Unity. Initializes themer.
        /// </summary>
        private void Start()
        {
            ActiveTheme = -1;


            NextTheme();
        }

        /// <summary>
        /// Called by Unity. Handles hotkeys.
        /// </summary>
        private void Update()
        {
            if (NextThemeHotkey.EvaluateJust())
            {
                NextTheme();
            }
        }

        #endregion
    }
}
