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
        public InputField infOriText;
        public InputField infTraText;
        public Text txtVoice;
        public Text txtTimeRange;
        public InputField infTimeOverride;
        public Button btnPlayVoice;
        [Header("Settings")]
        public IconSet charIconSet;

        SysL2DShow sysL2DShow;
        public SysL2DShow SysL2DShow => sysL2DShow;
        AudioData AudioData => sysL2DShowEditor.player.AudioData;

        private void Awake()
        {
            infTraText.onEndEdit.AddListener((str) => sysL2DShow.translationText = str);
            infTimeOverride.onEndEdit.AddListener((str) => sysL2DShow.dateTimeOverrideText = str);
        }

        public void SetData(SysL2DShow sysL2DShow)
        {
            this.sysL2DShow = sysL2DShow;

            imgCharIcon.sprite = charIconSet.icons[sysL2DShow.systemLive2D.CharacterId];
            txtBaseInfo.text =
$@"ID {string.Join("、 ",sysL2DShow.systemLive2D.masterSystemLive2Ds.Select((msl2d)=>msl2d.id.ToString()))}
表情 {sysL2DShow.systemLive2D.Expression}  动作 {sysL2DShow.systemLive2D.Motion}";
            infOriText.text = sysL2DShow.systemLive2D.Serif;
            infTraText.text = sysL2DShow.translationText;
            txtVoice.text = $"{sysL2DShow.systemLive2D.AssetbundleName} - {sysL2DShow.systemLive2D.Voice}";
            btnPlayVoice.interactable = AudioData.ContainsValue($"{sysL2DShow.systemLive2D.AssetbundleName}-{sysL2DShow.systemLive2D.Voice}");
            txtTimeRange.text = string.Join("\n", 
                sysL2DShow.systemLive2D.masterSystemLive2Ds
                .Select((msl2d) => $"{ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.publishedAt):D} 到 {ExtensionTools.UnixTimeMSToDateTimeTST(msl2d.closedAt):D}"));
            infTimeOverride.text = sysL2DShow.dateTimeOverrideText;
        }
    }
}