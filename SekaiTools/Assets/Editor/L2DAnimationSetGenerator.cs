
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using UnityEditor;
using System.IO;
using Live2D.Cubism.Framework.MotionFade;

namespace SekaiTools.UnityEditor
{
    public class L2DAnimationSetGenerator : EditorWindow
    {
        string motionPath;
        string fadePath;
        string savePath;

        [MenuItem("Void/L2DAnimationSetGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(L2DAnimationSetGenerator));
            window.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            motionPath = EditorGUILayout.TextField("motionPath", motionPath);
            fadePath = EditorGUILayout.TextField("fadePath", fadePath);
            savePath = EditorGUILayout.TextField("savePath", savePath);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Apply"))
            {
                Process();
            }

            EditorGUILayout.EndVertical();
        }
        void ProcessFolder(string path)
        {
            L2DAnimationSet l2DAnimationSet = L2DAnimationSet.CreateL2DAnimationSet(path);
            l2DAnimationSet.fadeMotionList = AssetDatabase.LoadAssetAtPath<CubismFadeMotionList>(Path.Combine(fadePath, Path.GetFileName(path) + ".fadeMotionList.asset"));
            string savePath = Path.Combine(this.savePath, Path.GetFileName(path) + ".asset");
            AssetDatabase.CreateAsset(l2DAnimationSet, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        void Process()
        {
            string[] subFolders = Directory.GetDirectories(motionPath);
            foreach (var subFolder in subFolders)
            {
                ProcessFolder(subFolder);
            }
        }
    }
}