using SekaiTools.Effect;
using SekaiTools.UI.Modifier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_HDRColor : BGModifierBase, IHDRColor
    {
        [Header("Target")]
        public List<MonoBehaviour> _hDRColorComponents;
        [ColorUsage(true,true)] public Color startColor;

        List<IHDRColor> hDRColorComponents = new List<IHDRColor>();

        public Color hDRColor
        {
            get
            {
                return hDRColorComponents[0].hDRColor;
            }
            set
            {
                foreach (var hDRColorParticle in hDRColorComponents)
                {
                    hDRColorParticle.hDRColor = value;
                }
            }
        }

        private void Awake()
        {
            foreach (var monoBehaviour in _hDRColorComponents)
            {
                IHDRColor iHDRColor = monoBehaviour as IHDRColor;
                if (iHDRColor == null) Debug.LogError("组件没有IHDRColor接口");
                else if (monoBehaviour == this) Debug.LogError("组件不应为自身");
                else
                {
                    hDRColorComponents.Add(iHDRColor);
                    iHDRColor.hDRColor = startColor;
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

            public SerializedModifier(BGModifier_HDRColor modifier)
            {
                hDRColor = modifier.hDRColor;
            }
        }
    }
}