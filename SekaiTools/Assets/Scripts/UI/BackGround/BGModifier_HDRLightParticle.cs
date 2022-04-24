using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Effect;
using SekaiTools.UI.Modifier;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_HDRLightParticle : BGModifierBase,IHDRLight
    {
        [Header("Target")]
        public List<HDRLightParticle> targetHDRLightParticles;

        public float lightStrength
        {
            get => targetHDRLightParticles[0].lightStrength;
            set
            {
                foreach (var hDRLightParticle in targetHDRLightParticles)
                {
                    hDRLightParticle.lightStrength = value;
                }
            }
        }

        public override void Initialize(GameObject modifierUI)
        {
            base.Initialize(modifierUI);

            ModifierUI_UniversalValueEdit modifierUI_UniversalValueEdit = modifierUI.GetComponent<ModifierUI_UniversalValueEdit>();
            modifierUI_UniversalValueEdit.Initialize(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                () => lightStrength,
                (value) => lightStrength = value
                ));
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(new SerializedModifier(this));    
        }

        public override void Deserialize(string serializedData) 
        {
            SerializedModifier serializedModifier = JsonUtility.FromJson<SerializedModifier>(serializedData);
            lightStrength = serializedModifier.lightStrength;
        }

        class SerializedModifier
        {
            public float lightStrength;
            public SerializedModifier(BGModifier_HDRLightParticle modifier)
            {
                lightStrength = modifier.lightStrength;
            }
        }
    }
}