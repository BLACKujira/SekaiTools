using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.BackGround;
using UnityEngine.UI;
using System;

namespace SekaiTools.UI.BackGroundSettings
{
    public class BackGroundSettings_PartSetting : MonoBehaviour
    {
        [Header("Components")]
        public ToggleGenerator toggleGeneratorModifiers;

        GameObject currentModifier;

        Action onNextChange = null;

        public void SetPart(BackGroundPart backGroundPart)
        {
            if (onNextChange != null) { onNextChange();onNextChange = null; }

            toggleGeneratorModifiers.ClearToggles();
            toggleGeneratorModifiers.Generate(backGroundPart.bGModifiers.Count,
                (Toggle toggle, int id) =>
                {
                    Text text = toggle.GetComponentInChildren<Text>();
                    text.text = backGroundPart.bGModifiers[id].itemName;
                },
                (bool value, int id) =>
                {
                    if (currentModifier)
                        Destroy(currentModifier);
                    currentModifier = Instantiate(backGroundPart.bGModifiers[id].uIPrefab, transform);
                    backGroundPart.bGModifiers[id].Initialize(currentModifier);
                });

            UnityEngine.Events.UnityAction call = () => { ResetSettings(); };
            backGroundPart.onDestroy.AddListener(call);
            onNextChange = () => { backGroundPart.onDestroy.RemoveListener(call); };
        }

        private void ResetSettings()
        {
            toggleGeneratorModifiers.ClearToggles();
            if (currentModifier) Destroy(currentModifier);
        }
    }
}