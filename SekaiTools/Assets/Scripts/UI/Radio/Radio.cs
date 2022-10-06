using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using SekaiTools.UI.RadioInitialize;
using SekaiTools.StringConverter;

namespace SekaiTools.UI.Radio
{
    public class Radio : MonoBehaviour
    {
        public Window window;
        MonoBehaviour currentPage;
        [Header("Components")]
        public RawImage backgroundMask;
        public RectTransform cMDInputLayer;
        public Radio_EffectController effectController;
        public Radio_WelcomeLayer welcomeLayer;
        public Radio_WelcomeLayer welcomeLayer_Blank;
        public Radio_MusicLayer musicLayer;
        public Radio_MusicListLayer musicListLayer;
        public Radio_PlayListLayer playListLayer;
        public Radio_MessageLayer messageLayer;
        public Radio_SerifQueryLayer serifQueryLayer;
        public Radio_CMDListLayer cMDListLayer;
        public Radio_CardAppreciationLayer cardAppreciationLayer;
        [Header("Components_Theme")]
        public Image image_TitleBG;
        public Image image_MusicInfoBG;
        public Image image_ProgressBar;
        [Header("Settings")]
        public float changePageFadeTime;
        public RadioThemeSet themeSet;
        [Header("Prefab")]
        public Window gSWWindowPrefab;

        StringConverter_StringAlias cMDAliases;
        RadioTheme currentTheme;
        public RadioTheme CurrentTheme => currentTheme;
        public event Action<RadioTheme> OnThemeChange;
        
        public Radio_WelcomeLayer WelcomeLayer { get; private set; }

        #region 初始化
        private void Awake()
        {
            currentTheme = themeSet[2];
            OnThemeChange += (theme) =>
            {
                image_TitleBG.color = theme.color_UI;
                image_MusicInfoBG.color = theme.color_UI;
                image_ProgressBar.color = theme.color_ProgressBar;
            };
        }

        public void Initialize(Settings settings)
        {
            cMDAliases = settings.cMDAliases;
            musicLayer.Initialize(settings.musicPlayerSettings);
            playListLayer.Initialize();
            effectController.Initialize(settings.effectControllerSettings);
            cardAppreciationLayer.Initialize(settings.cardAppreciationSettings);
            if (cardAppreciationLayer.EnableLayer)
            {
                welcomeLayer.gameObject.SetActive(false);
                welcomeLayer_Blank.Initialize(settings.welcomeLayerSettings);
                currentPage = welcomeLayer_Blank;
                WelcomeLayer = welcomeLayer_Blank;
                StartCoroutine(welcomeLayer_Blank.ShowTips());
            }
            else
            {
                welcomeLayer_Blank.gameObject.SetActive(false);
                welcomeLayer.Initialize(settings.welcomeLayerSettings);
                currentPage = welcomeLayer;
                WelcomeLayer = welcomeLayer;
                StartCoroutine(welcomeLayer.ShowTips());
            }

            if (settings.serifQuerySettings.enable) serifQueryLayer.Initialize(settings.serifQuerySettings);

            foreach (var cMDInputInitializePart in settings.radioCMDInputInitializeParts)
            {
                RadioCommandinput_Base radioCommandinput_Base = Instantiate(cMDInputInitializePart.managerObjectPrefab,cMDInputLayer);
                radioCommandinput_Base.Initialize(this, cMDInputInitializePart.settings);
            }

            musicLayer.Play();

            cMDListLayer.Initialize();
        }

        public class Settings
        {
            public StringConverter_StringAlias cMDAliases;
            public RadioCMDInputInitializePart[] radioCMDInputInitializeParts;
            public Radio_EffectController.Settings effectControllerSettings;
            public Radio_WelcomeLayer.Settings welcomeLayerSettings;
            public Radio_MusicLayer.Settings musicPlayerSettings;
            public Radio_SerifQueryLayer.Settings serifQuerySettings;
            public Radio_CardAppreciationLayer.Settings cardAppreciationSettings;
        }
        #endregion

        public void ProcessRequest(string inputString,string userName)
        {
            Debug.Log($"收到指令 @{userName} {inputString}");
            string[] cmdArray = inputString.Split(' ');
            string cmd = cmdArray[0].Substring(1);
            cmd = cMDAliases.GetValue(cmd) ?? cmd;
            switch (cmd)
            {
                case "ordersong":
                    OrderMusic(cmdArray, userName);
                    return;
                case "serifquery":
                    if (!CheckLayerEnabled(serifQueryLayer, userName))
                        return;
                    SerifQuery(cmdArray, userName);
                    return;
                case "cmdlist":
                    ShowCMDList(userName);
                    return;
                case "musiclist":
                    MusicListQuery(cmdArray,userName);
                    return;
                case "playlist":
                    PlayList(userName);
                    return;
                case "return":
                    ReturnToMainPage(userName);
                    return;
                default:
                    messageLayer.AddMessage(new RadioMessage(userName, MessageType.error, "不支持的指令"));
                    return;
            }
        }

        bool isSwitchingPage = false;
        public void ChangePage(MonoBehaviour pageNext,Action onChange = null)
        {
            isSwitchingPage = true;
            MonoBehaviour pagePrev = currentPage;

            backgroundMask.DOFade(1, changePageFadeTime / 2).OnComplete(() =>
                {
                    pagePrev.gameObject.SetActive(false);
                    pageNext.gameObject.SetActive(true);
                    currentPage = pageNext;
                    if (onChange != null) onChange();
                    backgroundMask.DOFade(0, changePageFadeTime / 2).OnComplete(()=>
                    {
                        isSwitchingPage = false;
                    });
                });
        }

        bool CanOpenPage(string userName)
        {
            if (currentPage != WelcomeLayer)
            {
                messageLayer.AddMessage(userName, MessageType.error, "请等待当前页面显示完毕或关闭当前页面再打开新页面");
                return false;
            }
            if (isSwitchingPage)
            {
                messageLayer.AddMessage(userName, MessageType.error, "请等待页面淡入/淡出完毕再执行指令");
                return false;
            }
            return true;
        }

        bool CanReturnMainPage(string userName)
        {
            IReturnableWindow returnableWindow = currentPage as IReturnableWindow;

            if (isSwitchingPage)
            {
                messageLayer.AddMessage(userName, MessageType.error, "请等待页面淡入/淡出完毕再执行指令");
                return false;
            }

            if (returnableWindow == null)
            {
                messageLayer.AddMessage(userName, MessageType.error, "当前页面不可返回");
                return false;
            }

            if (returnableWindow.ReturnPermission == ReturnPermission.unable)
            {
                messageLayer.AddMessage(userName, MessageType.error, "现在不可返回主页");
                return false;
            }
            else if(returnableWindow.ReturnPermission == ReturnPermission.sender
                && !returnableWindow.SenderUserName.Equals(userName))
            {
                messageLayer.AddMessage(userName, MessageType.error, "当前只有打开页面的用户可以关闭此页面");
                return false;
            }
            return true;
        }

        bool CanClosePage()
        {
            if (isSwitchingPage)
                return false;
            return true;
        }

        DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> changeThemeTween;
        public void ChangeTheme(RadioTheme toTheme,float duration = 0.5f)
        {
            if (changeThemeTween != null)
                changeThemeTween.Kill();
            RadioTheme startTheme = currentTheme;
            float t = 0;
            changeThemeTween = DOTween.To(
                () => t,
                (value) =>
                {
                    t = value;
                    currentTheme = RadioTheme.Lerp(startTheme, toTheme, t);
                    OnThemeChange(currentTheme);
                },
                1, duration);
        }

        public bool CheckLayerEnabled(Radio_OptionalLayer optionalLayer,string userName)
        {
            if(!optionalLayer.EnableLayer)
                messageLayer.AddMessage(new RadioMessage(userName,MessageType.error,"当前电台没有开启此功能"));
            return optionalLayer.EnableLayer;
        }

        public void Config()
        {
            GeneralSettingsWindow.GeneralSettingsWindow generalSettingsWindow = window.OpenWindow<GeneralSettingsWindow.GeneralSettingsWindow>(gSWWindowPrefab);
            generalSettingsWindow.Initialize
                (WelcomeLayer.configUIItems);
        }

        #region 点歌
        private const string msg_OM_FormatError = "点歌指令格式错误";

        public void OrderMusic(string[] cmdArray, string userName)
        {
            string musicName;
            string[] fitters;

            if (cmdArray.Length < 2) 
            {
                messageLayer.AddMessage(new RadioMessage(userName, MessageType.error, msg_OM_FormatError));
                return;
            }

            musicName = cmdArray[1];
            if (cmdArray.Length > 2)
                fitters = new List<string>(cmdArray).GetRange(2, cmdArray.Length - 2).ToArray();
            else
                fitters = new string[0];

            RadioMessage radioMessage = musicLayer.OrderMusic(new MusicOrderInfo(musicName, fitters),userName);
            messageLayer.AddMessage(radioMessage);
        }
        #endregion

        #region 对话查询
        private const string msg_SQ_FormatError = "查询指令缺少参数";

        public void SerifQuery(string[] cmdArray, string userName)
        {
            if (cmdArray.Length < 3)
            {
                messageLayer.AddMessage(userName, MessageType.error, msg_SQ_FormatError);
                return;
            }

            string[] fitters = new string[0];
            if(cmdArray.Length>3)
            {
                fitters = new List<string>(cmdArray).GetRange(3, cmdArray.Length - 3).ToArray();
            }
            SerifQueryInfo serifQueryInfo = new SerifQueryInfo(cmdArray[1], cmdArray[2], fitters);
            StartCoroutine(ISerifQuery(serifQueryInfo, userName));
        }

        IEnumerator ISerifQuery(SerifQueryInfo serifQueryInfo, string userName)
        {
            Debug.Log("开始查询");

            bool keepWaiting = true;
            SerifQueryResult serifQueryResult = null;
            Thread thread = new Thread(() =>
            {
                serifQueryResult = serifQueryLayer.Query(serifQueryInfo);
                keepWaiting = false;
            });
            thread.Start();

            while (keepWaiting)
                yield return 1;

            Debug.Log("查询完成");

            if (serifQueryResult.resultCountMatrices.Count == 0)
                messageLayer.AddMessage(userName, MessageType.success, "未查询到指定的对话");
            else
            {
                if (CanOpenPage(userName))
                {
                    serifQueryLayer.SetDisplayQuery(serifQueryResult);
                    ChangePage(serifQueryLayer, () => 
                    { 
                        serifQueryLayer.Play(userName,()=>
                        {
                            if(CanClosePage())
                                ChangePage(WelcomeLayer);
                        }); 
                    });
                }
                else
                    messageLayer.AddMessage(userName, MessageType.error, $"请等待当前查询结果显示完毕再次查询");
            }
        }
        #endregion

        #region 指令列表
        public void ShowCMDList(string userName)
        {
            if (CanOpenPage(userName))
            {
                ChangePage(cMDListLayer, () =>
                {
                    cMDListLayer.Play(() =>
                    {
                        if (CanClosePage())
                            ChangePage(WelcomeLayer);
                    });
             });
            }
        }
        #endregion

        #region 返回主页
        public void ReturnToMainPage(string userName)
        {
            if (CanReturnMainPage(userName))
                ChangePage(WelcomeLayer);
        }
        #endregion

        #region 歌曲列表
        public void MusicListQuery(string[] cmdArray, string userName)
        {
            string[] fitters;

            if (cmdArray.Length > 1)
                fitters = new List<string>(cmdArray).GetRange(1, cmdArray.Length - 1).ToArray();
            else
                fitters = new string[0];

            MusicListQueryResult musicListQueryResult = musicListLayer.Query(new MusicListQueryInfo(fitters));
            if (musicListQueryResult.resultItems.Count == 0)
                messageLayer.AddMessage(userName, MessageType.error, "未查询到指定分类的歌曲");
            else
            {
                if(CanOpenPage(userName))
                {
                    ChangePage(musicListLayer,()=>
                    {
                        musicListLayer.SetDisplayQuery(musicListQueryResult);
                        musicListLayer.Play(userName,() =>
                        {
                            if (CanClosePage())
                                ChangePage(WelcomeLayer);
                        });
                    });
                }
            }
        }
        #endregion

        #region 播放列表
        public void PlayList(string userName)
        {
            if(musicLayer.PlayList.Length <= 0)
            {
                messageLayer.AddMessage(userName, MessageType.error, "当前播放列表为空");
                return;
            }

            if (CanOpenPage(userName))
            {
                ChangePage(playListLayer, () =>
                {
                    playListLayer.Play(userName, () =>
                    {
                        if (CanClosePage())
                            ChangePage(WelcomeLayer);
                    });
                });
            }
        }
        #endregion
    }
}