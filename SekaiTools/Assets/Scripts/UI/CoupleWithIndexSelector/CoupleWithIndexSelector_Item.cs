using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CoupleWithIndexSelector
{
    public class CoupleWithIndexSelector_Item : MonoBehaviour
    {
        public Image imgBGColor;
        public BondsHonorSub bondsHonorSub;
        public Toggle[] toggles;

        public SelectStatus[] Status
        {
            get
            {
                SelectStatus[] selectStatuses = new SelectStatus[toggles.Length];
                for (int i = 0; i < toggles.Length; i++)
                {
                    Toggle toggle = toggles[i];
                    if (toggle && toggle.interactable)
                    {
                        selectStatuses[i] = toggle.isOn ? SelectStatus.Checked : SelectStatus.Unchecked;
                    }
                    else
                    {
                        selectStatuses[i] = SelectStatus.Unavailable;
                    }
                }
                return selectStatuses;
            }
        }

        int index;
        public int Index => index;

        public void Initialize(int index)
        {
            this.index = index;
        }

        public void SetCharacter(int charIdL,int charIdR)
        {
            bondsHonorSub.SetCharacter(charIdL, charIdR);
            imgBGColor.color = ConstData.characters[charIdR].imageColor;
            ResetStatus();
        }

        public void ResetStatus()
        {
            foreach (var toggle in toggles)
            {
                if(toggle)
                {
                    toggle.isOn = false;
                    toggle.interactable = false;
                }    
            }
        }

        public void SetStatus(SelectStatus[] selectStatuses)
        {
            ResetStatus();
            for (int i = 0; i < selectStatuses.Length && i < toggles.Length; i++)
            {
                Toggle toggle = toggles[i];
                if (toggle)
                {
                    switch (selectStatuses[i])
                    {
                        case SelectStatus.Unchecked:
                            toggle.isOn = false;
                            toggle.interactable = true;
                            break;
                        case SelectStatus.Checked:
                            toggle.isOn = true;
                            toggle.interactable = true;
                            break;
                        case SelectStatus.Unavailable:
                            toggle.isOn = false;
                            toggle.interactable = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}