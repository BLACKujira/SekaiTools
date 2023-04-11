using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChartCharacter_Item : DynamicBarChart_Item
    {
        [Header("Components")]
        public BondsHonorSub bondsHonorSub;
        public Graphic charColor;
        public Image charIcon;
        [Header("Settings")]
        public IconSet charSmallIconSet;

        bool ifFirstUpdate = true;
        public override void UpdateData(DataFrame dataFrame, string key, float maxNumber)
        {
            base.UpdateData(dataFrame, key, maxNumber);
            if (ifFirstUpdate)
            {
                ifFirstUpdate = false;
                string[] keyArray = key.Split('_');
                int charAId = int.Parse(keyArray[0]);
                int charBId = int.Parse(keyArray[1]);
                bondsHonorSub.SetCharacter(charAId, charBId);
                charColor.color = ConstData.characters[charBId].imageColor;
                charIcon.sprite = charSmallIconSet.icons[charBId];
            }
        }

        protected override string GetNumberString(float number)
        {
            return number.ToString("0");
        }
    }
}