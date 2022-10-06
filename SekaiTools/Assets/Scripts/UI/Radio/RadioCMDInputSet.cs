using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.Radio;
using SekaiTools.UI.RadioInitialize;

namespace SekaiTools.UI.Radio
{
    [System.Serializable]
    public class RadioCMDInputItem
    {
        public RadioCommandinput_Base managerObjectPrefab;
        public GIP_RadioCMDInput_ItemBase configItemPrefab;
    }

    [CreateAssetMenu(menuName = "SekaiTools/Radio/RadioCMDInputSet")]
    public class RadioCMDInputSet : ScriptableObject
    {
        public List<RadioCMDInputItem> radioCMDInputItems = new List<RadioCMDInputItem>();
        public RadioCMDInputItem this[int index] => radioCMDInputItems[index]; 
    }
}