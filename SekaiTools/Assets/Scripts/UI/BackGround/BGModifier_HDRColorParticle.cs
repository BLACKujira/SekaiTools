using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Effect;
using SekaiTools.UI.Modifier;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_HDRColorParticle : BGModifierBase,IHDRColor
    {
        [Header("Target")]
        public List<HDRColorParticle> targetHDRColorParticles;

        public Color hDRColor
        {
            get
            {
                return targetHDRColorParticles[0].hDRColor;
            }
            set
            {
                foreach (var hDRColorParticle in targetHDRColorParticles)
                {
                    hDRColorParticle.hDRColor = value;
                }
            }
        }

        public override void Initialize(GameObject modifierUI)
        {
            base.Initialize(modifierUI);

            ModifierUI_Color modifierUI_Color = modifierUI.GetComponent<ModifierUI_Color>();
            modifierUI_Color.Initialize(
                () => hDRColor,
                (value) => hDRColor = value,
                () => modifierUI_Color.SetValue()
                );
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(new SerializedModifier(this));
        }

        public override void Deserialize(string serializedData)
        {
            SerializedModifier serializedModifier = JsonUtility.FromJson<SerializedModifier>(serializedData);
            hDRColor = serializedModifier.hDRColor;
        }

        [System.Serializable]
        class SerializedModifier
        {
            public Color hDRColor;

            public SerializedModifier(BGModifier_HDRColorParticle modifier)
            {
                hDRColor = modifier.hDRColor;
            }
        }
    }
}