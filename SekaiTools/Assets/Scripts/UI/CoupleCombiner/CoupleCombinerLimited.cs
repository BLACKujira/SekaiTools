using System;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CoupleCombiner
{
    public class CoupleCombinerLimited : CoupleCombiner
    {
        [Header("Settings")]
        public Color colorTier1;
        public Color colorTier2;

        int tier = 0;
        CoupleAvailableStatus coupleAvailableStatus;

        public void Initialize(CoupleAvailableStatus coupleAvailableStatus, Vector2Int[] vector2Ints = null, Action<Vector2Int[]> onApply = null)
        {
            this.coupleAvailableStatus = coupleAvailableStatus;
            base.Initialize(vector2Ints, onApply);
        }

        void SetAllTogglesInteractable()
        {
            foreach (var toggle in toggles)
            {
                if (toggle)
                    toggle.interactable = true;
            }
        }

        void SetAllTogglesNonInteractable()
        {
            foreach (var toggle in toggles)
            {
                if (toggle)
                    toggle.interactable = false;
            }
        }

        private void Awake()
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                int id = i;
                Toggle toggle = toggles[id];
                if (toggle!=null)
                {
                    toggle.onValueChanged.AddListener((value) =>
                    {
                        if (value)
                        {
                            if(tier == 0)
                            {
                                SetAllTogglesNonInteractable();
                                for (int j = 0; j < toggles.Count ; j++)
                                {
                                    int id2 = j;
                                    if (id >= coupleAvailableStatus.Rows.Length || coupleAvailableStatus[id].Items == null) continue;
                                    if (id2 >= coupleAvailableStatus[id].Items.Length) break;
                                    if (coupleAvailableStatus[id].Items[id2] && toggles[id])
                                        toggles[id2].interactable = true;
                                }
                                toggle.interactable = true;
                                toggle.graphic.color = colorTier1;
                            }
                            else
                            {
                                SetAllTogglesNonInteractable();
                                toggle.interactable = true;
                                toggle.graphic.color = colorTier2;
                            }
                            tier++;
                        }
                        else
                        {
                            if (tier == 2)
                            {
                                SetAllTogglesNonInteractable();
                                for (int j = 0; j < toggles.Count ; j++)
                                {
                                    int id2 = j;
                                    if (id >= coupleAvailableStatus.Rows.Length || coupleAvailableStatus[id].Items == null) continue;
                                    if (id2 >= coupleAvailableStatus[id].Items.Length) break;
                                    if (coupleAvailableStatus[id].Items[id2] && toggles[id])
                                        toggles[id2].interactable = true;
                                }
                                toggle.interactable = true;
                            }
                            else
                            {
                                SetAllTogglesInteractable();
                            }
                            tier--;
                        }
                    });
                }
            }
        }
    }

    public class CoupleAvailableStatus : CoupleMatrix<bool>
    {
        public CoupleAvailableStatus(int length) : base(length)
        {
        }
    }
}