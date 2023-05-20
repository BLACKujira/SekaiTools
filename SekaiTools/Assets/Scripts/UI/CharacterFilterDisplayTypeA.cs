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
                if (selectedCharIcons[i])
                    selectedCharIcons[i].SetActive(characterIdMask[i]);
            }
        }

        public void SetMask(int[] characterIds)
        {
            foreach (var gobj in selectedCharIcons)
            {
                if (gobj != null)
                    gobj.SetActive(false);
            }

            for (int i = 0; i < characterIds.Length; i++)
            {
                int charId = characterIds[i];

                if (charId < selectedCharIcons.Length && selectedCharIcons[charId])
                    selectedCharIcons[charId].SetActive(true);
            }
        }

        public void SetAllSelected()
        {
            foreach (var gobj in selectedCharIcons)
            {
                if (gobj)
                    gobj.SetActive(true);
            }
        }
    }
}