using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SekaiTools.DecompiledClass;
using System.IO;

namespace SekaiTools.Editor
{
    public class EventLogoRenamer : EditorWindow
    {
        public TextAsset eventsJson;
        public string loadPath;
        public string savePath;

        [MenuItem("Void/EventLogoRenamer")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(EventLogoRenamer));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            eventsJson = (TextAsset)EditorGUILayout.ObjectField(eventsJson, typeof(TextAsset), false);
            loadPath = EditorGUILayout.TextField("LoadPath", loadPath);
            savePath = EditorGUILayout.TextField("SavePath", savePath);
            if (GUILayout.Button("Apply"))
            {
                Apply();
            }
            EditorGUILayout.EndVertical();
        }
        void Apply()
        {
            MasterEvent[] masterEvents = JsonHelper.getJsonArray<MasterEvent>(eventsJson.text);
            foreach (var masterEvent in masterEvents)
            {
                string path = Path.Combine(loadPath, masterEvent.assetbundleName, "logo", "logo.png");
                if (File.Exists(path))
                    File.Move(path, Path.Combine(savePath, masterEvent.assetbundleName + ".png"));
            }
        }
    }
}