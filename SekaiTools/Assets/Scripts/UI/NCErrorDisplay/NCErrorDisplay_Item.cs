using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCErrorDisplay
{
    public class NCErrorDisplay_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image[] imgCharAColor;
        public Image[] imgCharBColor;
        public Image[] imgCharAIcon;
        public Image[] imgCharBIcon;
        public Text txtCharAToCharB;
        public Text txtCharBToCharA;
        public Text txtDescription;
        [Header("Settings")]
        public IconSet charIconSet;

        public void Initialize(NCError nCError)
        {
            foreach (var image in imgCharAColor) image.color = ConstData.characters[nCError.charAId].imageColor;
            foreach (var image in imgCharBColor) image.color = ConstData.characters[nCError.charBId].imageColor;
            foreach (var image in imgCharAIcon) image.sprite = charIconSet.icons[nCError.charAId];
            foreach (var image in imgCharBIcon) image.sprite = charIconSet.icons[nCError.charBId];
            txtCharAToCharB.text = nCError.timesCharAToCharB.ToString();
            txtCharBToCharA.text = nCError.timesCharBToCharA.ToString();
            txtDescription.text = nCError.description;
        }
    }
}