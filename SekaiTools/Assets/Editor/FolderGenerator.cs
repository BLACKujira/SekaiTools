using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SekaiTools;
using System.IO;

namespace SekaiTools.Editor
{
    public class FolderGenerator : EditorWindow
    {
        string path;
        public enum NameType { id_name, name_id, id, name, idname }
        public NameType nameType = NameType.id_name;

        [MenuItem("Void/FolderGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(FolderGenerator));
            window.Show();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            path = EditorGUILayout.TextField("Save path", path);
            nameType = (NameType)EditorGUILayout.EnumPopup("NameType", nameType);

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
            for (int i = 1; i < 27; i++)
            {
                string folderName = "";
                switch (nameType)
                {
                    case NameType.id_name:
                        folderName = $"{i.ToString("00")}_{(ConstData.Character)i}";
                        break;
                    case NameType.name_id:
                        folderName = $"{(ConstData.Character)i}_{i.ToString("00")}";
                        break;
                    case NameType.id:
                        folderName = $"{i.ToString("00")}";
                        break;
                    case NameType.name:
                        folderName = $"{(ConstData.Character)i}";
                        break;
                    case NameType.idname:
                        folderName = $"{i.ToString("00")}{(ConstData.Character)i}";
                        break;
                    default: continue;
                }
                string savePath = Path.Combine(path, folderName);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}