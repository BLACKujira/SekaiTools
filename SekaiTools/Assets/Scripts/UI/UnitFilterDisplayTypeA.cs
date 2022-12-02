using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI
{
    public class UnitFilterDisplayTypeA : MonoBehaviour
    {
        public GameObject[] selectedUnitIcons;

        public void SetMask(bool[] unitIdMask)
        {
            for (int i = 0; i < selectedUnitIcons.Length; i++)
            {
                if (selectedUnitIcons[i])
                    selectedUnitIcons[i].SetActive(unitIdMask[i]);
            }
        }

        public void SetAllSelected()
        {
            foreach (var gobj in selectedUnitIcons)
            {
                if (gobj)
                    gobj.SetActive(true);
            }
        }
    }
}