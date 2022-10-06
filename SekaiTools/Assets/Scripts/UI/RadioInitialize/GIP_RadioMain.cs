using SekaiTools.StringConverter;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioMain : MonoBehaviour
    {
        public LoadFileSelectItem file_CMDAliases;

        public StringConverter_StringAlias cMDAliases => new StringConverter_StringAlias(
                        CSVTools.LoadCSV(File.ReadAllText(file_CMDAliases.SelectedPath)));

        public void Initialize()
        {
            file_CMDAliases.defaultPath = Path.Combine(EnvPath.Inbuilt, "CMDListCSV.txt");
            file_CMDAliases.ResetPath();
        }
    }
}