using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class SetSizeFitterLayoutOnEnable : MonoBehaviour
    {
        public List<SizeFitterItem> sizeFitterItems = new List<SizeFitterItem>();

        [System.Serializable]
        public class SizeFitterItem
        {
            public bool setLayoutHorizontal = true;
            public bool setLayoutVertical = true;
            public ContentSizeFitter contentSizeFitter;
        }

        private void OnEnable()
        {
            foreach (var sizeFitterItem in sizeFitterItems)
            {
                if (sizeFitterItem.setLayoutHorizontal)
                    sizeFitterItem.contentSizeFitter.SetLayoutHorizontal();
                if (sizeFitterItem.setLayoutVertical)
                    sizeFitterItem.contentSizeFitter.SetLayoutVertical();
            }
        }
    }
}