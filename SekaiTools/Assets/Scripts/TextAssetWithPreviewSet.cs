using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    [CreateAssetMenu(menuName = "SekaiTools/TextAssetWithPreviewSet")]
    public class TextAssetWithPreviewSet : ScriptableObject
    {
        public TextAssetWithPreview[] values;

        public TextAssetWithPreview this[int index] => values[index];

        [System.Serializable]
        public class TextAssetWithPreview
        {
            public string name;
            public TextAsset textAsset;
            public Sprite sprite;
        }
    }
}
