using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Spine;
using System;

namespace SekaiTools.UI.SpineLayer
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ToggleGroup))]
    public class SpineImage : MonoBehaviour
    {
        [Header("Components")]
        public SpineControllerTypeA spineController;
        [Header("Prefabs")]
        public Toggle hitboxPrefab;

        List<Toggle> toggles = new List<Toggle>();
        Action<int> onSelect = null;
        Action<int> onUnselect = null;
        Action onAllUnselect = null;

        public int selectedID
        {
            get
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].isOn)
                        return i;
                }
                return -1;
            }
        }

        RectTransform _rectTransform;
        ToggleGroup _toggleGroup;
        public RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();return _rectTransform; } }
        public ToggleGroup toggleGroup { get { if (!_toggleGroup) _toggleGroup = GetComponent<ToggleGroup>(); return _toggleGroup; } }

        public void Initialize(Action<int> onSelect, Action<int> onUnselect, Action onAllUnselect)
        {
            this.onSelect = onSelect;
            this.onUnselect = onUnselect;
            this.onAllUnselect = onAllUnselect;
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            ClearToggles();
            for (int i = 0; i < spineController.models.Count; i++)
            {
                int id = i;
                SpineControllerTypeA.ModelPair modelPair = spineController.models[i];
                Vector2 pointInRect = RectTransformUtility.WorldToScreenPoint(CameraController.SpineCamera, modelPair.Model.transform.position);
                Toggle toggle = Instantiate(hitboxPrefab, rectTransform);
                RectTransform toggleRTF = toggle.GetComponent<RectTransform>();
                toggleRTF.anchoredPosition = pointInRect;
                toggle.onValueChanged.AddListener((bool value) =>
                {
                    if (value)
                    {
                        if (onSelect != null) onSelect(id); 
                    }
                    else
                    {
                        if (onUnselect != null) { onUnselect(id); }
                        if (id == selectedID)
                        {
                            if (onAllUnselect != null) onAllUnselect();
                        }
                    }
                });

                toggle.group = toggleGroup;
                toggles.Add(toggle);
            }

            foreach (var toggle in toggles)
            {
                toggle.isOn = false;
            }
        }   

        void ClearToggles()
        {
            foreach (var toggle in toggles)
            {
                Destroy(toggle.gameObject);
            }
            toggles = new List<Toggle>();
        }
    }
}