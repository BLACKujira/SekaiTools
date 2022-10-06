using SekaiTools.DecompiledClass;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.BPMDataCSVGeneratorInitialize
{
    public class BPMDataCSVGeneratorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_PathSelect gIP_PathSelect_Input;
        public GIP_PathSelect gIP_PathSelect_Output;

        private void Awake()
        {
            gIP_PathSelect_Input.pathSelectItems[0].defaultPath = $"{EnvPath.assets}\\music\\music_score";
            gIP_PathSelect_Input.pathSelectItems[0].ResetPath();

            gIP_PathSelect_Output.pathSelectItems[0].defaultPath = $"{EnvPath.output}\\MusicBPMCSV.txt";
            gIP_PathSelect_Output.pathSelectItems[0].ResetPath();
        }

        public void Apply()
        {
            MasterMusic[] masterMusics = JsonHelper.getJsonArray<MasterMusic>(
                File.ReadAllText(
                    Path.Combine(EnvPath.sekai_master_db_diff, "musics.json")));

            List<string> outputLines = new List<string>();
            foreach (var masterMusic in masterMusics)
            {
                List<string> outputValues = new List<string>();
                outputValues.Add(masterMusic.id.ToString("0000"));
                outputValues.Add(masterMusic.title);
                string path = $"{gIP_PathSelect_Input.pathSelectItems[0].SelectedPath}\\{masterMusic.id.ToString("0000")}_01_rip\\master.txt";
                if (File.Exists(path))
                {
                    IEnumerable<string> lines = File.ReadLines(path);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("#BPM01:"))
                        {
                            outputValues.Add(
                                line.Split(':')[1].Trim(' ')
                                );
                            break;
                        }
                    }
                }
                if(outputValues.Count<=2)
                    outputValues.Add(string.Empty);
                outputLines.Add(string.Join(",", outputValues));
            }

            File.WriteAllLines(gIP_PathSelect_Output.pathSelectItems[0].SelectedPath, outputLines);
            WindowController.ShowMessage("消息", "生成完成");
        }
    }
}