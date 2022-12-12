using SekaiTools.DecompiledClass;
using SekaiTools.SekaiViewerInterface;
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
        [Header("Prefab")]
        public Window tableGetterPrefab;
        public Window toolBoxPrefab;

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

        public void SetServerTranslate()
        {
            MutiServerTableGetter.MutiServerTableGetter mutiServerTableGetter
                = window.OpenWindow<MutiServerTableGetter.MutiServerTableGetter>(tableGetterPrefab);
            string t_sysl2d = "systemLive2ds";
            mutiServerTableGetter.Initialize(ServerRegion.tw, t_sysl2d, (sr) =>
             {
                 WindowController.ShowCancelOK(Message.Error.STR_WARNING_DO, "此操作会覆盖现有的翻译，确定要继续吗？", () =>
                   {
                       MasterSystemLive2D[] msl2d_tra;
                       try
                       {
                           msl2d_tra = EnvPath.GetTable<MasterSystemLive2D>(t_sysl2d,sr);
                       }
                       catch
                       {
                           WindowController.ShowMessage(Message.Error.STR_ERROR, Message.Error.STR_TABLE_CORRUPTION);
                           return;
                       }
                       Dictionary<int, MasterSystemLive2D> dic_msl2d_tra = new Dictionary<int, MasterSystemLive2D>();
                       foreach (var msl2d in msl2d_tra)
                       {
                           dic_msl2d_tra[msl2d.id] = msl2d;
                       }
                       foreach (var sysL2DShow in sysL2DShowData.sysL2DShows)
                       {
                           if(dic_msl2d_tra.ContainsKey(sysL2DShow.systemLive2D.FirstId))
                           {
                               sysL2DShow.translationText = dic_msl2d_tra[sysL2DShow.systemLive2D.FirstId].serif;
                           }
                       }
                       editArea.SetData(CurrentSysL2DShow);
                   });
             });
        }

        public void SetOverrideTime()
        {
            WindowController.ShowCancelOK(Message.Error.STR_WARNING_DO, "此操作会覆盖现有的覆写，确定要继续吗？", () =>
            {
                foreach (var sysL2DShow in sysL2DShowData.sysL2DShows)
                {
                    string ot = AutoOverrideDatetime.GetOverrideText(sysL2DShow);
                    if (!string.IsNullOrEmpty(ot))
                    {
                        sysL2DShow.dateTimeOverrideText = ot;
                    }
                    editArea.SetData(CurrentSysL2DShow);
                }
            });
        }

        public void OpenToolBox()
        {
            FunctionSelector.FunctionSelector functionSelector
                = window.OpenWindow<FunctionSelector.FunctionSelector>(toolBoxPrefab);
            functionSelector.Initialize(SetServerTranslate, SetOverrideTime);
        }
    }
}