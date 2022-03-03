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
        string path;
        public enum ProcessType { folder, subFolder }
        public ProcessType processType;

        [MenuItem("Void/L2DAnimationSetGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(L2DAnimationSetGenerator));
            window.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            path = EditorGUILayout.TextField("Path", path);
            processType = (ProcessType)EditorGUILayout.EnumPopup("ProcessType", processType);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Apply"))
            {
                if (processType == ProcessType.folder)
                {
                    ProcessTypeFolder(path);
                }
                else
                {
                    ProcessTypeSubFolder(path);
                }
            }

            EditorGUILayout.EndVertical();
        }
        void ProcessTypeFolder(string path)
        {
            L2DAnimationSet l2DAnimationSet = L2DAnimationSet.CreateL2DAnimationSet(path);
            l2DAnimationSet.fadeMotionList = AssetDatabase.LoadAssetAtPath<CubismFadeMotionList>(Path.Combine(path, Path.GetFileName(path) + ".fadeMotionList.asset"));
            string savePath = Path.Combine(path, Path.GetFileName(path) + ".animationSet.asset");
            AssetDatabase.CreateAsset(l2DAnimationSet, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        void ProcessTypeSubFolder(string path)
        {
            string[] subFolders = Directory.GetDirectories(path);
            foreach (var subFolder in subFolders)
            {
                ProcessTypeFolder(subFolder);
            }
        }
    }
}