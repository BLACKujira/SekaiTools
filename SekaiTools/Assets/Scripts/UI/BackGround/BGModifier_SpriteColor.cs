using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.Modifier;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_SpriteColor : BGModifierBase,IColor
    {
        [Header("Target")]
        public SpriteRenderer targetSpriteRenderer;

        public Color color { get => targetSpriteRenderer.color; set => targetSpriteRenderer.color = value; }

        public override void Initialize(GameObject modifierUI)
        {
            base.Initialize(modifierUI);
            ModifierUI_Color modifierUI_Color = modifierUI.GetComponent<ModifierUI_Color>();
            modifierUI_Color.Initialize(() => targetSpriteRenderer.color,(value)=> { targetSpriteRenderer.color = value; });
            modifierUI_Color.onColorChange.AddListener(() => targetSpriteRenderer.color = modifierUI_Color.color);
        }


        public override string Serialize()
        {
            return JsonUtility.ToJson(new SerializedModifier(this));
        }

        public override void Deserialize(string serializedData)
        {
            SerializedModifier serializedModifier = JsonUtility.FromJson<SerializedModifier>(serializedData);
            color = serializedModifier.color;
        }

        [System.Serializable]
        class SerializedModifier
        {
            public Color color;

            public SerializedModifier(BGModifier_SpriteColor modifier)
            {
                color = modifier.color;
            }
        }
    }
}