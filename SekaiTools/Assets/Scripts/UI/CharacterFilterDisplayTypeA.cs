using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class CharacterFilterDisplayTypeA : MonoBehaviour
    {
        public GameObject[] selectedCharIcons;

        public void SetMask(bool[] characterIdMask)
        {
            for (int i = 0; i < selectedCharIcons.Length; i++)
            {
                if(selectedCharIcons[i])
                    selectedCharIcons[i].SetActive(characterIdMask[i]);
            }
        }

        public void SetAllSelected()
        {
            foreach (var gobj in selectedCharIcons)
            {
                if(gobj)
                    gobj.SetActive(true);
            }
        }
    }
}