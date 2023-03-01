using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_UnitFriend_Item : MonoBehaviour
    {
        [Header("Components")]
        public NCSScene_UnitFriend_Item_Char[] charItems = new NCSScene_UnitFriend_Item_Char[4];
        public Image imgChar;
        public Graphic[] graphicsCharColor;
        public Text txtTimes;
        [Header("Components")]
        public IconSet charImgSet;

        public void Initialize(int charId,Vector2Int[] timesChar)
        {
            imgChar.sprite = charImgSet.icons[charId];
            foreach (var graphic in graphicsCharColor)
            {
                graphic.color = ConstData.characters[charId].imageColor;
            }

            for (int i = 0; i < charItems.Length; i++)
            {
                charItems[i].Initialize(timesChar[i].x, timesChar[i].y);
            }

            txtTimes.text = timesChar
                .Take(charItems.Length)
                .Select(v=>v.y)
                .Sum()
                .ToString() + " 次";
        }
    }
}