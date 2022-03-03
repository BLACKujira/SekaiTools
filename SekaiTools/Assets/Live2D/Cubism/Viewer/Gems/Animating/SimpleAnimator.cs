/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using Live2D.Cubism.Framework.Json;
using UnityEngine;


namespace Live2D.Cubism.Viewer.Gems.Animating
{
    /// <summary>
    /// Loops the last animation dropped.
    /// </summary>
    // TODO Clean up clips as necessary
    public sealed class SimpleAnimator : MonoBehaviour
    {
        /// <summary>
        /// Called by Unity. Registers handler.
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


            // Register event handler.
            viewer.OnFileDrop += HandleFileDrop;           
        }

        #region CubismViewer Event Handling

        /// <summary>
        /// Handles file drops.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="absolutePath">Absolute path of dropped file.</param>
        private void HandleFileDrop(CubismViewer sender, string absolutePath)
        {
            // Skip non-motion files.
            if (!absolutePath.EndsWith("motion3.json"))
            {
                return;
            }


            var model = sender.Model;


            // Make sure animation component is attached to model.
            var animator = model.GetComponent<Animation>();


            if (animator == null)
            {
                animator = model.gameObject.AddComponent<Animation>();
            }


            // Deserialize animation.
            var model3Json = CubismMotion3Json.LoadFrom(CubismViewerIo.LoadAsset<string>(absolutePath));
            var clipName = CubismViewerIo.GetFileName(absolutePath);
            var clip = model3Json.ToAnimationClip();
            clip.wrapMode = WrapMode.Loop;
            clip.legacy = true;


            // Play animation.
            animator.AddClip(clip, clipName);
            animator.Play(clipName);
        }

        #endregion
    }
}
