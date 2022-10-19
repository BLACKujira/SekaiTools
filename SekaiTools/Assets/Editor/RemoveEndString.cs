using UnityEditor;
using UnityEngine;
using System.IO;

namespace SekaiTools.Editor
{
    public class RemoveEndString : EditorWindow
    {
        public enum Mode { File, Folder, FileAndFolder }
        public string removeString;
        public string path;
        public Mode mode;

        [MenuItem("Void/RemoveEndString")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(RemoveEndString));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            removeString = EditorGUILayout.TextField("RemoveString", removeString);
            path = EditorGUILayout.TextField("Path", path);
            mode = (Mode)EditorGUILayout.EnumPopup("Mode", mode);
            if (GUILayout.Button("Apply"))
            {
                Apply();
            }
            EditorGUILayout.EndVertical();
        }

        void Apply()
        {
            switch (mode)
            {
                case Mode.File:
                    RemoveFileNameStr();
                    break;
                case Mode.Folder:
                    RemoveFolderNameStr();
                    break;
                case Mode.FileAndFolder:
                    RemoveFileNameStr();
                    RemoveFolderNameStr();
                    break;
                default:
                    break;
            }
            AssetDatabase.Refresh();
        }

        void RemoveFileNameStr()
        {
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (Path.GetExtension(file).Equals(".meta")) continue;

                string fileName = Path.GetFileName(file);
                if (IfNeedRename(fileName))
                {
                    File.Move(file,
                        Path.Combine(
                            Path.GetDirectoryName(file), Rename(fileName)));

                    string metaFile = file + ".meta";
                    if(File.Exists(metaFile))
                        File.Move(metaFile,
                            Path.Combine(
                                Path.GetDirectoryName(metaFile), Rename(Path.GetFileName(metaFile))));
                }
            }
        }

        void RemoveFolderNameStr()
        {
            string[] folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                string folderName = Path.GetFileName(folder);
                if (IfNeedRename(folderName))
                {
                    File.Move(folder,
                        Path.Combine(
                            Path.GetDirectoryName(folder), Rename(folderName)));

                    string metaFile = folder + ".meta";
                    if (File.Exists(metaFile))
                        File.Move(metaFile,
                            Path.Combine(
                                Path.GetDirectoryName(metaFile), Rename(Path.GetFileName(metaFile))));
                }
            }
        }

        string Rename(string oldName)
        {
            string[] nameArray = oldName.Split('.');
            string newName;
            if (nameArray.Length==1)
            {
                newName = oldName.Substring(0, oldName.Length - removeString.Length);
            }
            else
            {
                nameArray[0] = nameArray[0].Substring(0, nameArray[0].Length - removeString.Length);
                newName = string.Join(".", nameArray);
            }
            return newName;
        }

        bool IfNeedRename(string name)
        {
            string[] nameArray = name.Split('.');
            if (nameArray[0].EndsWith(removeString))
                return true;
            return false;
        }
    }
}