using SekaiTools.SystemLive2D;
using SekaiTools.UI.MessageLayer;
using SekaiTools.UI.SysL2DShowPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DShowEditor
{
    public class SysL2DShowEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public SysL2DShowEditor_EditArea editArea;
        public SysL2DShowPlayer_Player player;
        public AudioSource audioSource;
        public ToggleGroup toggleGroup;
        public MessageLayerTypeA messageLayerTypeA;
        public SaveTipCloseWindowButton saveTipCloseWindowButton;

        SysL2DShowData sysL2DShowData;
        public SysL2DShowData SysL2DShowData => sysL2DShowData;
        AudioData AudioData => player.AudioData;
        SysL2DShow CurrentSysL2DShow => editArea.SysL2DShow;

        public class Settings
        {
            public SysL2DShowData sysL2DShowData;
            public SysL2DShowPlayer_Player.Settings playerSettings = new SysL2DShowPlayer_Player.Settings();
        }

        public void Initialize(Settings settings)
        {
            window.OnClose.AddListener(() =>
            {
                foreach (var sekaiLive2DModel in player.l2DControllerTypeB.live2DModels)
                {
                    if (sekaiLive2DModel)
                        Destroy(sekaiLive2DModel.gameObject);
                }
            });

            sysL2DShowData = settings.sysL2DShowData;
            saveTipCloseWindowButton.Initialize(() => sysL2DShowData.SavePath);
            player.Initialize(settings.playerSettings);
            universalGenerator.Generate(settings.sysL2DShowData.sysL2DShows.Count,
                (gobj, id) =>
                {
                    SysL2DShowEditor_Item sysL2DShowEditor_Item = gobj.GetComponent<SysL2DShowEditor_Item>();
                    sysL2DShowEditor_Item.Initialize(sysL2DShowData.sysL2DShows[id]);
                    Toggle toggle = gobj.GetComponent<Toggle>();
                    toggle.onValueChanged.AddListener((v) =>
                    {
                        if(v)
                        {
                            editArea.SetData(sysL2DShowData.sysL2DShows[id]);
                        }
                    });
                    toggle.group = toggleGroup;
                });
        }

        public void PlayVoice()
        {
            string voiceKey = $"{CurrentSysL2DShow.systemLive2D.AssetbundleName}-{CurrentSysL2DShow.systemLive2D.Voice}";
            audioSource.PlayOneShot(AudioData.GetValue(voiceKey));
        }

        public void PlayPreview()
        {
            player.SetData(CurrentSysL2DShow);
            if(string.IsNullOrEmpty(CurrentSysL2DShow.translationText))
            {
                player.Play(true);
            }
            else
            {
                player.Play(false);
            }
        }

        public void SaveData()
        {
            sysL2DShowData.SaveData();
            messageLayerTypeA.ShowMessage(Message.IO.STR_SAVECOMPLETE);
        }
    }
}