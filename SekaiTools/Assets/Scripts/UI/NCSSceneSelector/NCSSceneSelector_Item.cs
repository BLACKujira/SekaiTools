using SekaiTools.UI.NicknameCountShowcase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCSSceneSelector
{
    public class NCSSceneSelector_Item : MonoBehaviour
    {
        public Text titleText;
        public Image previewImage;
        public Text descriptionText;

        public void Initialize(NCSScene nCSScene)
        {
            titleText.text = nCSScene.itemName;
            previewImage.sprite = nCSScene.preview;
            descriptionText.text = nCSScene.description;
        }
    }
}