using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class UniversalToolSelector : MonoBehaviour
    {
        public Window window;
        public List<ToolButton> toolButtons = new List<ToolButton>();
        
        [System.Serializable]
        public class ToolButton
        {
            public Button button;
            public Window openWindow;
        }

        private void Awake()
        {
            foreach (var toolButton in toolButtons)
            {
                toolButton.button.onClick.AddListener(() =>
                {
                    window.OpenWindow(toolButton.openWindow);
                });
            }
        }
    }
}