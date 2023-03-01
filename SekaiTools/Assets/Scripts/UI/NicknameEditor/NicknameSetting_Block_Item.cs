using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameSetting
{
    public class NicknameSetting_Block_Item : MonoBehaviour
    {
        public InputField inputField;
        public Button removeButton;
        public Image imageError;
        public Image imageGlobal;

        public void SetErrorMode()
        {
            imageError.gameObject.SetActive(true);
        }
        public void SetGlobalMode()
        {
            imageGlobal.gameObject.SetActive(true);
            inputField.readOnly = true;
            removeButton.interactable = false;
        }
        public void SetDefaultMode()
        {
            if (imageError.gameObject.activeSelf) imageError.gameObject.SetActive(false);
            if (imageGlobal.gameObject.activeSelf) imageGlobal.gameObject.SetActive(false);
        }

        public void Initialize(string startText,Action<string> setText)
        {
            inputField.text = startText;

            if (!ExtensionTools.RegexCheck(startText)) SetErrorMode();

            inputField.onValueChanged.AddListener((string str) =>
            {
                if (ExtensionTools.RegexCheck(str))
                {
                    SetDefaultMode();
                    if(setText!=null) setText(str);
                }
                else
                {
                    SetErrorMode();
                }
            });
        }
    }
}