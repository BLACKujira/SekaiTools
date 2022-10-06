using SekaiTools.DecompiledClass;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.EmptyMusicMetadataGeneratorInitialize
{
    public class EmptyMusicMetadataGeneratorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_PathSelect gIP_PathSelect_Input;
        public GIP_PathSelect gIP_PathSelect_Output;

        private void Awake()
        {
            gIP_PathSelect_Output.pathSelectItems[0].defaultPath = $"{EnvPath.output}\\EmptyMusicMetadata.txt";
            gIP_PathSelect_Output.pathSelectItems[0].ResetPath();
        }

        public void Apply()
        {
            MasterMusic[] masterMusics = JsonHelper.getJsonArray<MasterMusic>(
                File.ReadAllText(
                    Path.Combine(EnvPath.sekai_master_db_diff, "musics.json")));

            string inputFile = gIP_PathSelect_Input.pathSelectItems[0].SelectedPath;
            List<string> outputLines;
            if (string.IsNullOrEmpty(inputFile))
                outputLines = GetLines_New(masterMusics);
            else
                outputLines = GetLines_Copy(masterMusics,
                    CSVTools.LoadCSV(File.ReadAllText(inputFile)));

            File.WriteAllLines(gIP_PathSelect_Output.pathSelectItems[0].SelectedPath, outputLines);
            WindowController.ShowMessage("消息", "生成完成");
        }

        private static List<string> GetLines_New(MasterMusic[] masterMusics)
        {
            List<string> outputLines = new List<string>();
            foreach (var masterMusic in masterMusics)
            {
                List<string> outputValues = new List<string>();
                string key = masterMusic.id.ToString("0000");

                outputValues.Add(key);
                outputValues.Add(masterMusic.title);
                outputLines.Add(string.Join(",", outputValues));
            }

            return outputLines;
        }

        private static List<string> GetLines_Copy(MasterMusic[] masterMusics,string[][] oldData)
        {
            Dictionary<string, string> oldDataLines = new Dictionary<string, string>();
            foreach (var values in oldData)
            {
                oldDataLines[values[0]] = string.Join(",", values);
            }

            List<string> outputLines = new List<string>();
            foreach (var masterMusic in masterMusics)
            {
                List<string> outputValues = new List<string>();
                string key = masterMusic.id.ToString("0000");

                if(oldDataLines.ContainsKey(key))
                {
                    outputLines.Add(oldDataLines[key]);
                    oldDataLines.Remove(key);
                }
                else
                {
                    outputValues.Add(key);
                    outputValues.Add(masterMusic.title);
                    outputLines.Add(string.Join(",", outputValues));
                }
            }

            foreach (var keyValuePair in oldDataLines)
            {
                outputLines.Add(keyValuePair.Value);
            }

            return outputLines;
        }
    }
}