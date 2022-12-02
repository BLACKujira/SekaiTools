using SekaiTools.SystemLive2D;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowEditor
{
    public class SysL2DShowEditor_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgColor;
        public Image imgCharIcon;
        public Text txtId;
        [Header("Settings")]
        public IconSet charIconSet;

        public void Initialize(SysL2DShow sysL2DShow)
        {
            imgColor.color = ConstData.characters[ConstData.MergeVirtualSinger(sysL2DShow.systemLive2D.CharacterId)].imageColor;
            if(sysL2DShow.systemLive2D.CharacterId==21)
            {
                imgCharIcon.sprite = charIconSet.icons[ConstData.GetUnitVirtualSinger(21, sysL2DShow.systemLive2D.UnitType)];
            }
            else
            {
                imgCharIcon.sprite = charIconSet.icons[sysL2DShow.systemLive2D.CharacterId];
            }
            txtId.text = sysL2DShow.systemLive2D.FirstId.ToString();
        }
    }
}