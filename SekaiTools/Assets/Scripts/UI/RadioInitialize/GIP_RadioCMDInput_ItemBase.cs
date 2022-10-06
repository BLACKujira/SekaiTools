using SekaiTools.UI.Radio;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RadioInitialize
{
    public abstract class GIP_RadioCMDInput_ItemBase : MonoBehaviour 
    {
        public Button button_remove;
        RadioCommandinput_Base managerObjectPrefab;

        public abstract RadioCommandinputSettingsBase Settings { get; }
        public RadioCommandinput_Base ManagerObjectPrefab { get => managerObjectPrefab; }

        public void Initialize(RadioCommandinput_Base managerObjectPrefab, Action remove)
        {
            this.managerObjectPrefab = managerObjectPrefab;
            button_remove.onClick.AddListener(() => remove());
        }
    }
}