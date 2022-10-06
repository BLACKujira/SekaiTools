using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class StringListEditItem_Item : MonoBehaviour
    {
        public InputField inputField;
        public Button buttonMoveUp;
        public Button buttonMoveDown;
        public Button buttonDelete;

        public void Initialize(Func<string> getValue, Action<string> setValue, Action moveUp, Action moveDown, Action delete)
        {
            inputField.text = getValue();
            inputField.onValueChanged.AddListener((str) => setValue(str));

            if (moveUp != null)
                buttonMoveUp.onClick.AddListener(() => moveUp());
            else
                buttonMoveUp.interactable = false;

            if (moveDown != null)
                buttonMoveDown.onClick.AddListener(() => moveDown());
            else
                buttonMoveDown.interactable = false;

            buttonDelete.onClick.AddListener(() => delete());
        }
    }
}