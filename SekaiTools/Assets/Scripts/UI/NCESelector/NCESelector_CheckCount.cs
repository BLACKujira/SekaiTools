using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCESelector
{
    public class NCESelector_CheckCount : MonoBehaviour
    {
        public NCESelector nCESelector;
        [Header("Components")]
        public Graphic[] graphicsCharColor;
        public Text txtCount;
        public Image imgPercent;

        public void Initialize(int characterId)
        {
            foreach (Graphic graphic in graphicsCharColor) 
            {
                graphic.color = ConstData.characters[characterId].imageColor;
            }
        }

        public void SetData(int checkedNumber,int totalNumber)
        {
            txtCount.text = $"{checkedNumber}/{totalNumber}";
            imgPercent.fillAmount = (float)checkedNumber / totalNumber;
        }
    }
}