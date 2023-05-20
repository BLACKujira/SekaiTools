using SekaiTools.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_LineChartCharacter_Legend : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharIcon;
        public Text txtColorHex;
        public Image imgColor;
        public HDRUIController particleController;
        [Header("Settings")]
        public IconSet charSpineIconSet;
        public HDRColorSet hdrColorSet;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public void SetCharacter(int characterId, int talkerId)
        {
            int charIconID = characterId;
            if ((talkerId >= 1 && talkerId <= 20) && (characterId >= 21 && characterId <= 26))
            {
                charIconID = ConstData.GetUnitVirtualSinger(charIconID, ConstData.characters[talkerId].unit);
            }
            imgCharIcon.sprite = charSpineIconSet.icons[charIconID];


            txtColorHex.text = $"#{ExtensionTools.GetColorHEX(ConstData.characters[characterId].imageColor)}";
            imgColor.color = ConstData.characters[characterId].imageColor;

            if (!particleController.Initialized) particleController.Initialize();
            HDRColorParticle hDRColorParticle = particleController.InstantiateObject.GetComponent<HDRColorParticle>();
            hDRColorParticle.hDRColor = hdrColorSet.colors[characterId];
        }
    }
}