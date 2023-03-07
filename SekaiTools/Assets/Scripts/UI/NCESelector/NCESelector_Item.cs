using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NCESelector
{
    public class NCESelector_Item : MonoBehaviour
    {
        public Text labelFileName;
        public Text countAll;
        public TextWithBG[] countCharacter;


        [System.Serializable]
        public struct TextWithBG
        {
            public Text text;
            public Image bg;
        }

        /// <summary>
        /// Vector2Int.x character,Vector2Int.y count
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="countAll"></param>
        /// <param name="countCharacter"></param>
        public void Initialize(string fileName, int countAll, StoryType storyType, StoryDescriptionGetter storyDescriptionGetter, params Vector2Int[] countCharacter)
        {
            List<Vector2Int> vector2Ints = new List<Vector2Int>(countCharacter);
            vector2Ints.Sort((x, y) => x.y.CompareTo(y.y));
            if (vector2Ints.Count > this.countCharacter.Length) vector2Ints = vector2Ints.GetRange(0, this.countCharacter.Length);

            labelFileName.text = storyDescriptionGetter == null ? fileName : storyDescriptionGetter.GetStroyDescription(storyType, fileName);
            this.countAll.text = countAll.ToString();
            for (int i = 0; (this.countCharacter.Length - vector2Ints.Count + i) < this.countCharacter.Length; i++)
            {
                int currentId = this.countCharacter.Length - vector2Ints.Count + i;
                this.countCharacter[currentId].bg.gameObject.SetActive(true);
                this.countCharacter[currentId].text.text = vector2Ints[i].y.ToString();
                this.countCharacter[currentId].bg.color = ConstData.characters[vector2Ints[i].x].imageColor;
            }
        }

        public void Initialize(string fileName, int countAll, params Vector2Int[] countCharacter)
        {
            Initialize(fileName, countAll, StoryType.OtherStory, null, countCharacter);
        }

    }
}