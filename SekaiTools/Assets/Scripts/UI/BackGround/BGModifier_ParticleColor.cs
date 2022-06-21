using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.Modifier;

namespace SekaiTools.UI.BackGround
{
    public interface IColor
    {
        Color color { get; set; }
    }

    public class BGModifier_ParticleColor : BGModifierBase,IColor
    {
        [Header("Target")]
        public ParticleSystem targetParticleSystem;

        ParticleSystem.MainModule main;

        public  Color color { get => main.startColor.color; set => main.startColor = value; }

        public override void Initialize(GameObject modifierUI)
        {
            base.Initialize(modifierUI);

            main = targetParticleSystem.main;

            ModifierUI_ColorBase modifierUI_ColorBase = modifierUI.GetComponent<ModifierUI_ColorBase>();
            modifierUI_ColorBase.Initialize(()=> main.startColor.color,(value)=> { main.startColor = value; });
            modifierUI_ColorBase.onColorChange.AddListener(()=>
            {
                main.startColor = modifierUI_ColorBase.color;
                targetParticleSystem.Stop();
                targetParticleSystem.Play();
            });
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(new SerializedModifier(this));
        }

        public override void Deserialize(string serializedData)
        {
            main = targetParticleSystem.main;
            SerializedModifier serializedModifier = JsonUtility.FromJson<SerializedModifier>(serializedData);
            color = serializedModifier.color;
            targetParticleSystem.Stop();
            targetParticleSystem.Play();
        }

        [System.Serializable]
        class SerializedModifier
        {
            public Color color;

            public SerializedModifier(BGModifier_ParticleColor modifier)
            {
                color = modifier.color;
            }
        }
    }
}