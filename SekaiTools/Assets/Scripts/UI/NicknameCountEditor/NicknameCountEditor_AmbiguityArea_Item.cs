using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountEditor
{
    public class NicknameCountEditor_AmbiguityArea_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text txtRegex;
        public Text txtCount;
        public Image[] charIcons;
        [Header("Settings")]
        public IconSet iconSet;

        public void Initialize(string regexPatten,int count)
        {
            txtRegex.text = regexPatten;
            txtCount.text = count.ToString();
        }

        public void SetCharIcons(int[] charIds)
        {
            for (int i = 0; i < charIds.Length&&i<charIcons.Length; i++)
            {
                Image image = charIcons[i];
                image.gameObject.SetActive(true);
                image.sprite = iconSet.icons[charIds[i]];
            }
        }
    }
}