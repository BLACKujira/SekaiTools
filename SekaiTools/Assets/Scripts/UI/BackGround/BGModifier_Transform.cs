using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.Modifier;

namespace SekaiTools.UI.BackGround
{
    public class BGModifier_Transform : BGModifierBase
    {
        [Header("Target")]
        public Transform targetTransform;
        [Header("Settings")]
        public bool editX = true;
        public bool editY = true;
        public bool editRotation;
        public bool editScaleX;
        public bool editScaleY;
        public bool editScale = true;

        public Vector2 Position
        {
            get => (Vector2)targetTransform.position - positionOffset;
            set
            {
                targetTransform.position = positionOffset + value;
            }
        }
        public float Rotation
        {
            get => targetTransform.rotation.eulerAngles.z - rotationOffset;
            set
            {
                Vector3 eulerAngles = targetTransform.rotation.eulerAngles;
                eulerAngles.z = value + rotationOffset;
                targetTransform.rotation = Quaternion.Euler(eulerAngles);
            }
        }
        public Vector2 Scale
        {
            get => (Vector2)targetTransform.localScale/scaleOffset;
            set => targetTransform.localScale = new Vector3(scaleOffset.x*value.x,scaleOffset.y*value.y,1);
        }

        [System.NonSerialized] public Vector2 positionOffset = Vector2.one;
        [System.NonSerialized] public float rotationOffset = 0;
        [System.NonSerialized] public Vector2 scaleOffset = Vector2.one;

        private void Awake()
        {
            positionOffset = targetTransform.position;
            rotationOffset = targetTransform.rotation.eulerAngles.z;
            scaleOffset = targetTransform.localScale; 
        }

        public override void Initialize(GameObject modifierUI)
        {
            base.Initialize(modifierUI);

            ModifierUI_UniversalValueEdit universalValueEdit = modifierUI.GetComponent<ModifierUI_UniversalValueEdit>();

            List<ModifierUI_UniversalValueEdit.ValueEditSetting> valueEditSettings = new List<ModifierUI_UniversalValueEdit.ValueEditSetting>();
            if (editX)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    ()=> Position.x,
                    (value)=>
                    {
                        Position = new Vector2(
                        value,
                        Position.y
                        );
                    }));

            if (editY)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    () => Position.y,
                    (value) =>
                    {
                        Position = new Vector2(
                        Position.x,
                        value
                        );
                    }));

            if (editRotation)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    () => Rotation,
                    (value) =>
                    {
                        Rotation = value;
                    }));

            if (editScaleX)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    () => Scale.x,
                    (value) =>
                    {
                        Scale = new Vector2(
                            value,
                            Scale.y
                        );
                    }));

            if (editScaleY)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    () => Scale.y,
                    (value) =>
                    {
                        Scale = new Vector2(
                        value,
                        Scale.y
                        );
                    }));

            if (editScale)
                valueEditSettings.Add(new ModifierUI_UniversalValueEdit.ValueEditSetting(
                    () => Scale.x,
                    (value) =>
                    {
                        Scale = new Vector2(
                        value,
                        value
                        );
                    }));

            universalValueEdit.Initialize(valueEditSettings.ToArray());
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(new SerializedModifier(this));
        }

        public override void Deserialize(string serializedData)
        {
            SerializedModifier serializedModifier = JsonUtility.FromJson<SerializedModifier>(serializedData);
            Position = serializedModifier.position;
            Rotation = serializedModifier.rotation;
            Scale = serializedModifier.scale;
        }

        [System.Serializable]
        class SerializedModifier
        {
            public Vector2 position;
            public float rotation;
            public Vector2 scale;

            public SerializedModifier(BGModifier_Transform modifier)
            {
                position = modifier.Position;
                rotation = modifier.Rotation;
                scale = modifier.Scale;
            }
        }
    }
}