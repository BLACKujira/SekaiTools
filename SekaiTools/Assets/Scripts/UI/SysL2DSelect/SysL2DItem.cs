using SekaiTools.SystemLive2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DSelect
{
    public abstract class SysL2DItem : MonoBehaviour
    {
        public Image imgBGColor;
        public Image imgCharIcon;
        public Text txtSerif;
        public Text txtInfo;
        [Header("Settings")]
        public IconSet charIconSet;

        MergedSystemLive2D mergedSystemLive2D;
        public MergedSystemLive2D MergedSystemLive2D => mergedSystemLive2D;

        public void Initialize(MergedSystemLive2D mergedSystemLive2D)
        {
            this.mergedSystemLive2D = mergedSystemLive2D;
            imgBGColor.color = ConstData.characters[mergedSystemLive2D.CharacterId].imageColor;
            if(mergedSystemLive2D.CharacterId==21)
            {
                int iconId = 21;
                switch (mergedSystemLive2D.UnitType)
                {
                    case DecompiledClass.UnitType.none:
                        break;
                    case DecompiledClass.UnitType.piapro:
                        break;
                    case DecompiledClass.UnitType.theme_park:
                        iconId = 30;
                        break;
                    case DecompiledClass.UnitType.idol:
                        iconId = 28;
                        break;
                    case DecompiledClass.UnitType.street:
                        iconId = 29;
                        break;
                    case DecompiledClass.UnitType.light_sound:
                        iconId = 27;
                        break;
                    case DecompiledClass.UnitType.school_refusal:
                        iconId = 31;
                        break;
                    case DecompiledClass.UnitType.any:
                        break;
                }
                imgCharIcon.sprite = charIconSet.icons[iconId];
            }
            else
            {
                imgCharIcon.sprite = charIconSet.icons[mergedSystemLive2D.CharacterId];
            }
            txtSerif.text = mergedSystemLive2D.Serif;
            string strMuti = (mergedSystemLive2D.IsSingle ? string.Empty : $"(+{mergedSystemLive2D.masterSystemLive2Ds.Count-1})");
            txtInfo.text = 
$@"ID {mergedSystemLive2D.masterSystemLive2Ds[0].id} {strMuti} 组合 {ConstData.units[mergedSystemLive2D.UnitType.ToUnit()].abbr}  权重 {mergedSystemLive2D.Weight}
起始 {ExtensionTools.UnixTimeMSToDateTime(mergedSystemLive2D.masterSystemLive2Ds[0].publishedAt):yy:MM:dd} 截止 {ExtensionTools.UnixTimeMSToDateTime(mergedSystemLive2D.masterSystemLive2Ds[0].closedAt):yy:MM:dd} {strMuti}
语音 {mergedSystemLive2D.AssetbundleName} - {mergedSystemLive2D.Voice}";
        }
    }

}