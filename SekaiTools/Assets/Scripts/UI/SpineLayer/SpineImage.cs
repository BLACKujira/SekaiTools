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

        List<ToggleWithModel> toggles = new List<ToggleWithModel>();
        Action<int> onSelect = null;
        Action<int> onUnselect = null;
        Action onAllUnselect = null;

        class ToggleWithModel
        {
            public readonly Toggle toggle;
            public readonly SpineControllerTypeA.ModelPair modelPair;

            public ToggleWithModel(Toggle toggle, SpineControllerTypeA.ModelPair modelPair)
            {
                this.toggle = toggle;
                this.modelPair = modelPair;
            }
        }

        public int selectedID
        {
            get
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].toggle.isOn)
                        return i;
                }
                return -1;
            }
        }

        RectTransform _rectTransform;
        ToggleGroup _toggleGroup;
        public RectTransform rectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();return _rectTransform; } }
        public ToggleGroup toggleGroup { get { if (!_toggleGroup) _toggleGroup = GetComponent<ToggleGroup>(); return _toggleGroup; } }

        public void Initialize(SpineControllerTypeA spineController,Action<int> onSelect, Action<int> onUnselect, Action onAllUnselect)
        {
            this.spineController = spineController;
            this.onSelect = onSelect;
            this.onUnselect = onUnselect;
            this.onAllUnselect = onAllUnselect;
            ResetAll();
        }

        public void ResetAll()
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
                toggles.Add(new ToggleWithModel(toggle,modelPair));
            }

            foreach (var toggle in toggles)
            {
                toggle.toggle.isOn = false;
            }
        }

        public void ResetPosition()
        {
            foreach (var toggleWithModel in toggles)
            {
                Vector2 pointInRect = RectTransformUtility.WorldToScreenPoint(
                    CameraController.SpineCamera, 
                    toggleWithModel.modelPair.Model.transform.position);
                toggleWithModel.toggle.GetComponent<RectTransform>().anchoredPosition = pointInRect;
            }
        }

        void ClearToggles()
        {
            foreach (var toggle in toggles)
            {
                Destroy(toggle.toggle.gameObject);
            }
            toggles = new List<ToggleWithModel>();
        }
    }
}