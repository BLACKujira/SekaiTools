using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    public class Radio_BuiltinConsole : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public InputField inputField_UserName;
        public InputField inputField_Command;

        public void Execution()
        {
            radio.ProcessRequest('/'+inputField_Command.text, inputField_UserName.text);
        }
    }
}