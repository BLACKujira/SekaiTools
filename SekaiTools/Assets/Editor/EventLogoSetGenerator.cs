using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SekaiTools.DecompiledClass;
using System.IO;
using SekaiTools.UI;

namespace SekaiTools.Editor
{
    public class EventLogoSetGenerator : EditorWindow
    {
        public TextAsset eventsJson;
        public string loadPath;

        [MenuItem("Void/EventLogoSetGenerator")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(EventLogoSetGenerator));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            eventsJson = (TextAsset)EditorGUILayout.ObjectField(eventsJson, typeof(TextAsset), false);
            loadPath = EditorGUILayout.TextField("LoadPath", loadPath);
            if (GUILayout.Button("Apply"))
            {
                Apply();
            }
            EditorGUILayout.EndVertical();
        }
        void Apply()
        {
            MasterEvent[] masterEvents = JsonHelper.getJsonArray<MasterEvent>(eventsJson.text);
            IconSet iconSet = CreateInstance<IconSet>();
            iconSet.icons = new Sprite[masterEvents.Length+1];
            foreach (MasterEvent masterEvent in masterEvents)
            {
                string path = Path.Combine(loadPath, masterEvent.assetbundleName + ".png");
                if(File.Exists(path))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    iconSet.icons[masterEvent.id] = sprite;
                }
            }
            AssetDatabase.CreateAsset(iconSet, Path.Combine(loadPath, "EventLogoSet.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}