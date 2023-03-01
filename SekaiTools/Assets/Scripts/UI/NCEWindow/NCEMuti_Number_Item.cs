using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCEWindow
{
    [RequireComponent(typeof(RectTransform))]
    public class NCEMuti_Number_Item : MonoBehaviour
    {
        public Image imgBgColor;
        public Text txtNumber;

        RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public void SetData(int charId, int times)
        {
            imgBgColor.color = ConstData.characters[charId].imageColor;
            txtNumber.text = times.ToString();
        }
    }
}