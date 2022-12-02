using SekaiTools.SystemLive2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowEditor
{
    public class SysL2DShowEditor_EditArea : MonoBehaviour
    {
        public SysL2DShowEditor sysL2DShowEditor;
        [Header("Components")]
        public Image imgCharIcon;
        public Text txtBaseInfo;
        public Text txtOriText;
        public InputField infTraText;
        public Text txtVoice;
        public Text txtTimeRange;
        public InputField infTimeOverride;
        [Header("Settings")]
        public IconSet charIconSet;

        SysL2DShow sysL2DShow;
        public SysL2DShow SysL2DShow => sysL2DShow;

        private void Awake()
        {
            infTraText.onEndEdit.AddListener((str) => sysL2DShow.translationText = str);
            infTimeOverride.onEndEdit.AddListener((str) => sysL2DShow.dateTimeOverrideText = str);
        }

        public void SetData(SysL2DShow sysL2DShow)
        {
            this.sysL2DShow = sysL2DShow;

            if (sysL2DShow.systemLive2D.CharacterId == 21)
            {
                int iconId = 21;
                switch (sysL2DShow.systemLive2D.UnitType)
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
                imgCharIcon.sprite = charIconSet.icons[sysL2DShow.systemLive2D.CharacterId];
            }
            txtBaseInfo.text =
$@"ID {string.Join("、 ",sysL2DShow.systemLive2D.masterSystemLive2Ds.Select((msl2d)=>msl2d.id.ToString()))}
表情 {sysL2DShow.systemLive2D.Expression}  动作 {sysL2DShow.systemLive2D.Motion}";
            txtOriText.text = sysL2DShow.systemLive2D.Serif;
            infTraText.text = sysL2DShow.translationText;
            txtVoice.text = $"{sysL2DShow.systemLive2D.AssetbundleName} - {sysL2DShow.systemLive2D.Voice}";
            txtTimeRange.text = string.Join("\n", 
                sysL2DShow.systemLive2D.masterSystemLive2Ds
                .Select((msl2d) => $"{ExtensionTools.UnixTimeMSToDateTime(msl2d.publishedAt):D} 到 {ExtensionTools.UnixTimeMSToDateTime(msl2d.publishedAt):D}"));
            infTimeOverride.text = sysL2DShow.dateTimeOverrideText;
        }
    }
}