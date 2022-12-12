using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelManagement
{

    public class L2DModelManagement_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text textName;
        public Image imageEdgeColor;
        public Image imageCharColor;
        public Image characterIcon;
        [Header("Settings")]
        public IconSet iconSet;

        public void Initialize(string modelName)
        {
            textName.text = modelName;
            int charId = ConstData.IsLive2DModelOfCharacter(modelName, false);
            if(charId>=1&&charId<=56)
            {
                characterIcon.sprite = iconSet.icons[charId];
                imageCharColor.color = ConstData.characters[charId].imageColor;
            }
            else
            {
                characterIcon.gameObject.SetActive(false);
            }
        }
    }
}