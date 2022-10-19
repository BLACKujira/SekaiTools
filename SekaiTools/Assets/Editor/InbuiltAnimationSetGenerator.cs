using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using SekaiTools.Live2D;

namespace SekaiTools.Editor
{


    public class InbuiltAnimationSetGenerator : EditorWindow
    {
        string animationSetPath;
        InbuiltAnimationSet updateSet;

        [MenuItem("Void/InbuiltAnimationSetGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(InbuiltAnimationSetGenerator));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            animationSetPath = EditorGUILayout.TextField("AnimationSetPath", animationSetPath);
            updateSet = (InbuiltAnimationSet)EditorGUILayout.ObjectField("UpdateSet", updateSet, typeof(InbuiltAnimationSet),false);
            if (GUILayout.Button("Apply"))
            {
                Apply();
            }
            EditorGUILayout.EndVertical();
        }

        void Apply()
        {
            List<L2DAnimationSet> l2DAnimationSets = new List<L2DAnimationSet>();

            string[] paths = Directory.GetFiles(animationSetPath);
            foreach (var path in paths)
            {
                if (!Path.GetExtension(path).Equals(".asset")) continue;
                L2DAnimationSet l2DAnimationSet = AssetDatabase.LoadAssetAtPath<L2DAnimationSet>(path);
                l2DAnimationSets.Add(l2DAnimationSet);
            }

            updateSet.UpdateSet(l2DAnimationSets);
        }
    }
}