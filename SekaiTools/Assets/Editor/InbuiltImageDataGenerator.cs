using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SekaiTools.Editor
{
    public class InbuiltImageDataGenerator : EditorWindow
    {
        string path;

        [MenuItem("Void/InbuiltImageDataGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(InbuiltImageDataGenerator));
            window.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            path = EditorGUILayout.TextField("Image folder", path);

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
            InbuiltImageData inbuiltImageData= CreateInstance<InbuiltImageData>();
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if(Path.GetExtension(file).ToLower().Equals(".png"))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(file);
                    inbuiltImageData.sprites.Add(sprite);
                }
            }
            string savePath = Path.Combine(path, Path.GetFileName(path) + ".asset");
            AssetDatabase.CreateAsset(inbuiltImageData, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}