using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DSelect
{
    public class SysL2DItemAddable : SysL2DItem
    {
        [Header("ComponentsII")]
        public GameObject goEdgeUsed;
        public Button btnAddItem;

        public void SetUnusedMode()
        {
            goEdgeUsed.SetActive(false);
            btnAddItem.interactable = true;
        }

        public void SetUsedMode()
        {
            goEdgeUsed.SetActive(true);
            btnAddItem.interactable = false;
        }
    }

}