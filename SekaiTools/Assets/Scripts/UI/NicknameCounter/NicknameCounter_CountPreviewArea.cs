using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounter
{
    public class NicknameCounter_CountPreviewArea : MonoBehaviour
    {
        public NicknameCounter nicknameCounter;
        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public Text[] countNumbers = new Text[27];
        [Header("Settings")]
        public IconSet iconSet;

        int selectedCharacterId = 1;

        public void Initialize()
        {
            InitializeToggles();
            Refresh();
        }

        public void Refresh()
        {
            for (int i = 1; i < 27; i++)
            {
                Count.NicknameCountItem nicknameCountItem = nicknameCounter.nicknameCountData[selectedCharacterId, i];
                countNumbers[i].text = nicknameCountItem.Total.ToString();
            }
        }

        public void InitializeToggles()
        {
            toggleGenerator.Generate(26,
                (Toggle toggle, int id) =>
                {
                    toggle.transform.GetChild(0).GetComponent<Image>().sprite = iconSet.icons[id+1];
                    toggle.GetComponent<Image>().color = ConstData.characters[id + 1].imageColor;
                },
                (bool value, int id) =>
                {
                    if(value)
                    {
                        selectedCharacterId = id + 1;
                        Refresh();
                    }
                });
        }


    }
}