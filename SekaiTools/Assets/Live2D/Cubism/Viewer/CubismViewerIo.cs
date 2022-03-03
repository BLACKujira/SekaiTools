/*
 * Copyright(c) Live2D Inc. All rights reserved.
 * 
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at http://live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */
 
 
using System;
using System.IO;
using UnityEngine;


namespace Live2D.Cubism.Viewer
{
    /// <summary>
    /// IO helpers.
    /// </summary>
    public static class CubismViewerIo
    {
        /// <summary>
        /// Gets file name.
        /// </summary>
        /// <param name="path">Path to query against.</param>
        /// <returns>File name.</returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Gets directory name.
        /// </summary>
        /// <param name="path">Path to query against.</param>
        /// <returns>Directory name.</returns>
        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }


        /// <summary>
        /// Loads asset.
        /// </summary>
        /// <param name="assetType">Asset type.</param>
        /// <param name="absolutePath">Path to asset.</param>
        /// <returns>The asset on success; <see langword="null"/> otherwise.</returns>
        public static T LoadAsset<T>(string absolutePath) where T : class
        {
            return LoadAsset(typeof(T), absolutePath) as T;
        }

        /// <summary>
        /// Loads asset.
        /// </summary>
        /// <param name="assetType">Asset type.</param>
        /// <param name="absolutePath">Path to asset.</param>
        /// <returns>The asset on success; <see langword="null"/> otherwise.</returns>
        public static object LoadAsset(Type assetType, string absolutePath)
        {
            if (assetType == typeof(byte[]))
            {
                return File.ReadAllBytes(absolutePath);
            }
            else if (assetType == typeof(string))
            {
                return File.ReadAllText(absolutePath);
            }
            else if (assetType == typeof(Texture2D))
            {
                var texture = new Texture2D(1, 1);


                texture.LoadImage(File.ReadAllBytes(absolutePath));


                return texture;
            }


            // Fail hard.
            throw new NotSupportedException();
        }


        /// <summary>
        /// Loads config for given type.
        /// </summary>
        /// <typeparam name="T">Type to querey against</typeparam>
        /// <returns>Config.</returns>
        // HACK The whole thing is a hack for the lazy...
        public static T LoadConfig<T>() where T : class, new()
        {
            // Always return new config in editor.
            if (Application.isEditor)
            {
                return new T();
            }


            try
            {
                var serializedConfig = File.ReadAllText(Path.Combine(Application.persistentDataPath, typeof(T).Name + ".json"));


                return JsonUtility.FromJson<T>(serializedConfig); 
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// Saves config for given type.
        /// </summary>
        /// <typeparam name="T">Type to querey against</typeparam>
        /// <returns>Config.</returns>
        // HACK The whole thing is a hack for the lazy...
        public static void SaveConfig<T>(T config) where T : class
        {
            // Never save in editor.
            if (Application.isEditor)
            {
                return;
            }


            var serializedConfig = JsonUtility.ToJson(config);


            if (string.IsNullOrEmpty(serializedConfig))
            {
                return;
            }


            File.WriteAllText(Path.Combine(Application.persistentDataPath, typeof(T).Name + ".json"), serializedConfig);
        }
    }
}
