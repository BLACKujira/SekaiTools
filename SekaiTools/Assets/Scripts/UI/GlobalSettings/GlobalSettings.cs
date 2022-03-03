using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GlobalSettings
{
    public class GlobalSettings : MonoBehaviour
    {
        public Window window;
        [Header("Prefabs")]
        public Window prefabL2DAniSetManagement;
        public Window prefabL2DModelManagement;

        public void OpenL2DAniSetManagement()
        {
            Window window = Instantiate(prefabL2DAniSetManagement);
            window.Initialize(this.window);
            window.Show();
        }
        public void OpenL2DModelManagement()
        {
            Window window = Instantiate(prefabL2DModelManagement);
            window.Initialize(this.window);
            window.Show();
        }
    }
}