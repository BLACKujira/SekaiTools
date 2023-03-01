using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CoupleWithIndexSelector
{
    [RequireComponent(typeof(Toggle))]
    public class CoupleWithIndexSelector_Tab : MonoBehaviour
    {
        public Image imgCharIcon;
        public Image imgBGColor;
        [Header("Settings")]
        public IconSet charIconSet;

        Toggle toggle;
        public Toggle Toggle
        {
            get
            {
                if (!toggle)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }

        int index;
        public int Index => index;

        public void Initialize(int index)
        {
            this.index = index;
        }

        public void SetCharacter(int charId)
        {
            imgCharIcon.sprite = charIconSet.icons[charId];
            imgBGColor.color = ConstData.characters[charId].imageColor;
        }
    }
}