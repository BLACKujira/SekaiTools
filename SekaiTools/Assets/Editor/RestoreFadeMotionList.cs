using DG.Tweening.Plugins.Core.PathCore;
using Live2D.Cubism.Framework.MotionFade;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Path = System.IO.Path;

namespace SekaiTools.Editor
{
    public class RestoreFadeMotionList : EditorWindow
    {
        [MenuItem("Void/RestoreFadeMotionList")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(RestoreFadeMotionList));
            window.Show();
        }

        string pathFadeMorionEditor;
        string pathFadeMorionOrigin;

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            pathFadeMorionEditor = EditorGUILayout.TextField("pathFadeMorionEditor", pathFadeMorionEditor);
            pathFadeMorionOrigin = EditorGUILayout.TextField("pathFadeMorionOrigin", pathFadeMorionOrigin);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Apply"))
            {
                Apply();
            }

            EditorGUILayout.EndVertical();
        }

        void Apply()
        {
            int updatedFileCount = 0;

            string[] folderOrigin = Directory.GetDirectories(pathFadeMorionOrigin);
            string[] folderEditor = Directory.GetDirectories(pathFadeMorionEditor);

            foreach (var fo in folderOrigin)
            {
                foreach (var fe in folderEditor)
                {
                    string feName = Path.GetFileName(fe);
                    string foName = Path.GetFileName(fo);
                    if (feName.Equals(foName))
                    {
                        List<string> motionsOri = new List<string>();
                        foreach (var folder in Directory.GetDirectories(fo))
                        {
                            motionsOri.AddRange(
                                Directory.GetFiles(folder)
                                .Where((file) => Path.GetExtension(file).Equals(".json")));
                        }
                        foreach (var jsonPath in motionsOri)
                        {
                            string fadeMotionData = Path.Combine(fe, Path.GetFileNameWithoutExtension(jsonPath) + ".fade.asset");
                            if (File.Exists(fadeMotionData))
                            {
                                CubismFadeMotionData cubismFadeMotionData = AssetDatabase.LoadAssetAtPath<CubismFadeMotionData>(fadeMotionData);
                                OriginFadeMotionData originFadeMotionData = JsonUtility.FromJson<OriginFadeMotionData>(File.ReadAllText(jsonPath));
                                //if((cubismFadeMotionData.MotionName).Equals())
                                {
                                    cubismFadeMotionData.MotionName = originFadeMotionData.m_Name;
                                    cubismFadeMotionData.FadeInTime = originFadeMotionData.FadeInTime;
                                    cubismFadeMotionData.FadeOutTime = originFadeMotionData.FadeOutTime;
                                    EditorUtility.SetDirty(cubismFadeMotionData);
                                    updatedFileCount++;
                                }
                            }
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Updated Files : {updatedFileCount}");
        }

        [System.Serializable]
        public class OriginFadeMotionData
        {
            public string m_Name;
            public float FadeInTime;
            public float FadeOutTime;
        }
    }
}