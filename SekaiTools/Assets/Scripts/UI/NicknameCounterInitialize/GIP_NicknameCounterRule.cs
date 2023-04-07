using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class GIP_NicknameCounterRule : MonoBehaviour
    {
        [Header("Components")]
        public StringListEditItem stringListEditItem;
        public Toggle tog_RemoveString;

        public bool UseExcludeStrings => tog_RemoveString.isOn;
        List<string> excludeStrings = new List<string>(new string[] { "ー", "〜", "～" });
        public List<string> ExcludeStrings => excludeStrings;

        public bool RemoveString => tog_RemoveString.enabled;

        private void Awake()
        {
            stringListEditItem.Initialize(excludeStrings);
        }
    }
}