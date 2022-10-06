using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class CardRarityFilterItem : MonoBehaviour
    {
        public Toggle toggle_rarity_1;
        public Toggle toggle_rarity_2;
        public Toggle toggle_rarity_3;
        public Toggle toggle_rarity_4;
        public Toggle toggle_rarity_birthday;

        public CardRarityType[] cardRarityTypes
        {
            get
            {
                List<CardRarityType> cardRarityTypes = new List<CardRarityType>();
                if (toggle_rarity_1.isOn) cardRarityTypes.Add(CardRarityType.rarity_1);
                if (toggle_rarity_2.isOn) cardRarityTypes.Add(CardRarityType.rarity_2);
                if (toggle_rarity_3.isOn) cardRarityTypes.Add(CardRarityType.rarity_3);
                if (toggle_rarity_4.isOn) cardRarityTypes.Add(CardRarityType.rarity_4);
                if (toggle_rarity_birthday.isOn) cardRarityTypes.Add(CardRarityType.rarity_birthday);
                return cardRarityTypes.ToArray();
            }
        }
    }
}