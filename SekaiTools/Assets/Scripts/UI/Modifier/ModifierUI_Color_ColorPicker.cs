using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Modifier
{
    public class ModifierUI_Color_ColorPicker : MonoBehaviour
    {
        [Header("Components")]
        public ModifierUI_Color modifierUI_Color;
        public ButtonGenerator buttonGeneratorUnit;
        public ButtonGenerator buttonGeneratorCharacter;
        [Header("Settings")]
        public HDRColorSet colorSetChar;
        public HDRColorSet colorSetUnit;
        public IconSet iconSetCharacters;
        public IconSet iconSetUnits;

        public class ColorSet
        {
            public Color color;
            public Sprite sprite;

            public ColorSet(Color color, Sprite sprite)
            {
                this.color = color;
                this.sprite = sprite;
            }
        }
        public class ColorSetUnit : ColorSet
        {
            public ColorSet[] memberColors;

            public ColorSetUnit(int number,Color color, Sprite sprite) : base(color, sprite)
            {
                this.memberColors = new ColorSet[number];
            }
        }

        public List<ColorSetUnit> GetColors()
        {
            List<ColorSetUnit> colorSetUnits = new List<ColorSetUnit>();
            ColorSetUnit colorSetUnit_vs = new ColorSetUnit(
                6,
                colorSetUnit==null?(Color)ConstData.units[1].color:colorSetUnit.colors[1], 
                iconSetUnits.icons[1]);
            for (int i = 21; i <= 26; i++)
            {
                colorSetUnit_vs.memberColors[i - 21] = new ColorSet(
                    colorSetChar==null? (Color)ConstData.characters[i].imageColor:colorSetChar.colors[i],
                    iconSetCharacters.icons[i]);
            }
            colorSetUnits.Add(colorSetUnit_vs);

            for (int i = 2; i <= 6; i++)
            {
                ColorSetUnit colorSetUnit = new ColorSetUnit(
                    4, 
                    this.colorSetUnit==null? (Color)ConstData.units[i].color:this.colorSetUnit.colors[i], 
                    iconSetUnits.icons[i]);
                for (int j = 1; j < 5; j++)
                {
                    int charID = i * 4 - 8 + j;
                    colorSetUnit.memberColors[j - 1] = new ColorSet(
                        colorSetChar==null? (Color)ConstData.characters[charID].imageColor:colorSetChar.colors[charID], 
                        iconSetCharacters.icons[charID]);
                }
                colorSetUnits.Add(colorSetUnit);
            }
            return colorSetUnits;
        }

        public void Initialize()
        {
            List<ColorSetUnit> colorSetUnits = GetColors();
            buttonGeneratorUnit.Generate(colorSetUnits.Count,
                (Button button, int id) =>
                {
                    Image targetGraphic = (Image)button.targetGraphic;
                    targetGraphic.sprite = colorSetUnits[id].sprite;
                },
                (int id) =>
                {
                    modifierUI_Color.color = colorSetUnits[id].color;
                    buttonGeneratorCharacter.ClearButtons();
                    buttonGeneratorCharacter.Generate(colorSetUnits[id].memberColors.Length,
                        (Button button, int idInner) =>
                        {
                            Image targetGraphic = (Image)button.targetGraphic;
                            targetGraphic.sprite = colorSetUnits[id].memberColors[idInner].sprite;
                        },
                        (int idInner) =>
                        {
                            modifierUI_Color.color = colorSetUnits[id].memberColors[idInner].color;
                        });
                });
        }

        private void Awake()
        {
            Initialize();
        }
    }
}