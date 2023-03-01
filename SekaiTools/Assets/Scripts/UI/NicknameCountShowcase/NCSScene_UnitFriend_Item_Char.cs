using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_UnitFriend_Item_Char : MonoBehaviour
    {
        [Header("Components")]
        public Text txtTimes;
        public Image imgCharIcon;
        public Graphic[] graphicsCharColor;
        [Header("Components")]
        public IconSet charIconSet;

        public void Initialize(int charId, int times)
        {
            txtTimes.text = times.ToString();
            imgCharIcon.sprite = charIconSet.icons[charId];
            foreach (var graphic in graphicsCharColor)
            {
                graphic.color = ConstData.characters[charId].imageColor;
            }
        }
    }
}