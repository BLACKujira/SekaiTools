using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SekaiTools.Spine;
using Spine.Unity;

namespace SekaiTools.Editor
{
    public class SpineModelSetGenerator : EditorWindow
    {
        string path;

        [MenuItem("Void/SpineModelSetGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(SpineModelSetGenerator));
            window.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            path = EditorGUILayout.TextField("model folder", path);

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
            InbuiltSpineModelSet inbuiltSpineModelSet = CreateInstance<InbuiltSpineModelSet>();

            for (int i = 0; i < 58; i++)
            {
                inbuiltSpineModelSet.characters[i] = new InbuiltSpineModelSet.Character();
            }

            string[] folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                string folderName = Path.GetFileName(folder);
                int id = ConstData.IsSpineModelOfCharacter(folderName);
                if (id == 0) continue;
                string[] nameArray = folderName.Split('_');
                if (nameArray.Length == 3)
                {
                    AtlasAsset atlasAsset = AssetDatabase.LoadAssetAtPath<AtlasAsset>(Path.Combine(folder, "sekai_atlas_Atlas.asset"));
                    AtlasAsset atlasAsset_r = AssetDatabase.LoadAssetAtPath<AtlasAsset>(Path.Combine(folder + "_r", "sekai_atlas_Atlas.asset"));
                    inbuiltSpineModelSet.characters[id].atlasAssets.Add(new AtlasAssetPair(folderName, atlasAsset, atlasAsset_r));
                }
            }
            string savePath = Path.Combine(path, Path.GetFileName(path) + "SpineModelSet.asset");
            AssetDatabase.CreateAsset(inbuiltSpineModelSet, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
